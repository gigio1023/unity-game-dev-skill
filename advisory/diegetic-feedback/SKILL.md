---
name: unity-diegetic-feedback
description: "Diegetic suspicion and tension feedback system for Unity URP. Replace UI meters with layered environmental cues: audio (heartbeat, FMOD parameter-driven layers), visual (vignette, color temperature, chromatic aberration via URP Post-Processing Volume), NPC behavioral signals (gaze, clustering). 4-tier escalation. Use when implementing suspicion, tension, stealth feedback, or environmental storytelling through post-processing. Triggers: suspicion, tension, diegetic, stealth feedback, vignette, heartbeat, post-processing mood, environmental cue"
---

# Diegetic Suspicion / Tension Feedback System

Replace traditional UI meters with layered environmental cues that communicate game state without breaking immersion. The player FEELS danger rather than reading a number.

## Core Principle

Every feedback channel (audio, visual, NPC behavior) escalates independently through a shared tension parameter (0.0-1.0). Combine channels for compounding dread.

## 4-Tier Escalation Model

| Tier | Name | Tension Range | Player Feeling |
|------|------|--------------|----------------|
| 0 | Normal | 0.0 - 0.25 | Safe, exploring freely |
| 1 | Noticed | 0.25 - 0.50 | Something feels off |
| 2 | Watched | 0.50 - 0.75 | Clearly under scrutiny |
| 3 | Suspected | 0.75 - 0.90 | One mistake away from failure |
| 4 | Blown | 0.90 - 1.00 | Cover is blown, consequences |

---

## TensionManager (Central Hub -- Create First)

Single source of truth. All channels read from this. Game systems write to it.

```csharp
public class TensionManager : MonoBehaviour
{
    public static TensionManager Instance { get; private set; }

    [Range(0f, 1f)] public float CurrentTension;

    [SerializeField] private float _smoothSpeed = 2f;
    private float _targetTension;

    public void AddTension(float amount) =>
        _targetTension = Mathf.Clamp01(_targetTension + amount);
    public void SetTension(float value) =>
        _targetTension = Mathf.Clamp01(value);
    public void RelieveTension(float amount) =>
        _targetTension = Mathf.Clamp01(_targetTension - amount);

    void Awake() => Instance = this;

    void Update()
    {
        CurrentTension = Mathf.MoveTowards(
            CurrentTension, _targetTension, _smoothSpeed * Time.deltaTime);
    }

    // Tier threshold crossing events
    public event System.Action<int> OnTierChanged;
    private int _lastTier = -1;

    void LateUpdate()
    {
        int tier = CurrentTension switch
        {
            < 0.25f => 0, < 0.50f => 1, < 0.75f => 2, < 0.90f => 3, _ => 4
        };
        if (tier != _lastTier)
        {
            _lastTier = tier;
            OnTierChanged?.Invoke(tier);
        }
    }
}
```

**Why smoothing matters:** Without `MoveTowards`, tension spikes cause jarring visual pops (vignette snapping, color flashing). Smooth speed of 2f means a full 0-to-1 ramp takes 0.5s -- fast enough to feel responsive, slow enough to avoid seizure-inducing flashes.

---

## Channel 1: Audio (FMOD Parameter-Driven)

**Setup:** Single FMOD event with a continuous parameter `Tension` (0-1). Each layer is a track in FMOD Studio with automation on the Tension parameter.

| Layer | Tier | FMOD Implementation |
|-------|------|-------------------|
| Ambient drone | 0+ | Base track, always playing. Automate reverb wet from warm to cold |
| Heartbeat | 1+ | Multi Instrument on separate track. Automate tempo: 60bpm at 0.25, 120bpm at 1.0 |
| Crowd silence | 2+ | Automate ambient chatter volume to 0. Add isolated footstep echoes track |
| Dissonant strings | 3+ | Stinger track, random trigger interval 4-8s, volume automated by Tension |
| Alarm / siren | 4 | One-shot triggered programmatically on tier 4 threshold |

```csharp
public class TensionAudioController : MonoBehaviour
{
    [SerializeField] private FMODUnity.EventReference _tensionEvent;
    private FMOD.Studio.EventInstance _instance;

    void Start()
    {
        _instance = FMODUnity.RuntimeManager.CreateInstance(_tensionEvent);
        _instance.start();
    }

    void Update()
    {
        float tension = TensionManager.Instance.CurrentTension;
        _instance.setParameterByName("Tension", tension);
    }

    void OnDestroy()
    {
        _instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _instance.release();
    }
}
```

**FMOD Studio setup steps:**
1. Create event with parameter "Tension" (range 0-1, default 0)
2. Add tracks for each layer
3. Use automation curves on volume for each track tied to Tension parameter
4. Heartbeat track: use AHDSR modulator on pitch, automate rate by Tension
5. Set 3D attenuation to None (this is a non-positional ambient event)

---

## Channel 2: Visual (URP Post-Processing Volume)

**Setup:** Global Volume with `VolumeProfile`. Animate via script, NOT animation curves -- script is more responsive to gameplay.

| Effect | Parameter | Tier 0 | Tier 1 | Tier 2 | Tier 3 | Tier 4 |
|--------|-----------|--------|--------|--------|--------|--------|
| Vignette | Intensity | 0.15 | 0.25 | 0.35 | 0.50 | 0.65 |
| Vignette | Color | warm gray | neutral | cool gray | deep blue | dark red |
| Color Adjustments | Color Filter | (1, 0.95, 0.9) | (1, 1, 1) | (0.85, 0.9, 1) | (0.7, 0.8, 1) | (1, 0.6, 0.6) |
| Chromatic Aberration | Intensity | 0 | 0 | 0.15 | 0.4 | 0.8 |
| Film Grain | Intensity | 0 | 0 | 0.1 | 0.25 | 0.5 |
| Depth of Field | Focus Dist | off | off | subtle bg blur | strong bg blur | tunnel vision |

```csharp
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TensionPostProcessing : MonoBehaviour
{
    [SerializeField] private Volume _globalVolume;

    private Vignette _vignette;
    private ChromaticAberration _chromatic;
    private ColorAdjustments _colorAdj;
    private FilmGrain _filmGrain;

    [SerializeField] private Gradient _vignetteColorGradient;
    [SerializeField] private Gradient _colorFilterGradient;

    void Start()
    {
        var profile = _globalVolume.profile;
        profile.TryGet(out _vignette);
        profile.TryGet(out _chromatic);
        profile.TryGet(out _colorAdj);
        profile.TryGet(out _filmGrain);
    }

    void Update()
    {
        float t = TensionManager.Instance.CurrentTension;

        if (_vignette != null)
        {
            _vignette.intensity.value = Mathf.Lerp(0.15f, 0.65f, t);
            _vignette.color.value = _vignetteColorGradient.Evaluate(t);
        }
        if (_chromatic != null)
        {
            // Only kicks in above 0.4 tension
            _chromatic.intensity.value = Mathf.Lerp(0f, 0.8f,
                Mathf.InverseLerp(0.4f, 1f, t));
        }
        if (_colorAdj != null)
        {
            _colorAdj.colorFilter.value = _colorFilterGradient.Evaluate(t);
        }
        if (_filmGrain != null)
        {
            _filmGrain.intensity.value = Mathf.Lerp(0f, 0.5f,
                Mathf.InverseLerp(0.3f, 1f, t));
        }
    }
}
```

**Gradient Inspector setup:**
- Vignette color: key 0 = (180,170,160) warm gray, key 0.5 = (140,140,150) cool, key 0.85 = (50,60,120) deep blue, key 1.0 = (120,30,30) dark red
- Color filter: key 0 = (255,242,230) warm, key 0.5 = (217,230,255) cool, key 1.0 = (255,153,153) red

**Performance:** VolumeProfile parameter writes are ~0.02ms/frame. No GC. Safe in Update().

---

## Channel 3: NPC Behavioral Signals

The most powerful channel -- feels like AI intelligence, not a UI system.

| Tier | NPC Behavior | Implementation |
|------|-------------|----------------|
| Normal | Random idle anims, chat | Default Animator state |
| Noticed | Occasional glance toward player | Animation Rigging Look At, weight 0.3 for 1-2s |
| Watched | Stop talking when player approaches | Pause dialogue coroutine, face player 2s |
| Suspected | Cluster, whisper, stare | NavMeshAgent to nearest NPC, shared LookAt |
| Blown | Back away, point, alert | Flee behavior, alert animation trigger |

```csharp
using UnityEngine.Animations.Rigging;

public class NPCSuspicionBehavior : MonoBehaviour
{
    [SerializeField] private MultiAimConstraint _lookAtConstraint;
    [SerializeField] private Animator _animator;

    private static readonly int SuspicionTier = Animator.StringToHash("SuspicionTier");

    public void UpdateBehavior(float tension)
    {
        int tier = tension switch
        {
            < 0.25f => 0, < 0.50f => 1, < 0.75f => 2, < 0.90f => 3, _ => 4
        };

        _lookAtConstraint.weight = tier >= 1
            ? Mathf.Lerp(0f, 0.6f, (tension - 0.25f) / 0.75f)
            : 0f;

        _animator.SetInteger(SuspicionTier, tier);
    }
}
```

**Animation Rigging setup:**
1. Add `Rig Builder` to NPC root
2. Child "Rig" GameObject with `Rig` component
3. Child with `Multi-Aim Constraint` -- Constrained Object = Head bone, Source = player
4. Aim Axis = Z+, Up Axis = Y+
5. Optional: second constraint on Chest at 0.3x weight for natural upper body rotation

---

## Architecture

```
Game Systems (proximity, wrong answers, suspicious actions)
    |
    v
[TensionManager]  <-- single float 0-1, smoothed
    |
    +---> [TensionPostProcessing]  -- URP Volume params
    +---> [TensionAudioController] -- FMOD parameter
    +---> [NPCSuspicionBehavior]   -- per-NPC Animator + IK
    +---> [OnTierChanged event]    -- discrete reactions
```

---

## Integration Checklist

- [ ] Create TensionManager singleton in scene
- [ ] URP Global Volume: add Vignette, Chromatic Aberration, Color Adjustments, Film Grain
- [ ] Create 2 Gradient assets for color curves, configure keys as above
- [ ] FMOD event with `Tension` parameter (0-1) and layered tracks
- [ ] TensionPostProcessing component wired to Volume + Gradients
- [ ] TensionAudioController component wired to FMOD event
- [ ] NPCSuspicionBehavior on each NPC with Animation Rigging
- [ ] Wire game systems to AddTension() / RelieveTension()
- [ ] Test: expose _targetTension in Inspector, scrub at runtime
- [ ] Accessibility: optional traditional UI meter alongside diegetic cues

## Gotchas

- **Chromatic aberration on mobile is expensive.** Disable or cap at 0.3. Use `SystemInfo.graphicsDeviceType` to auto-detect.
- **Vignette color shifts are subtle on cheap monitors.** Always pair with audio.
- **NPC look-at needs Animation Rigging package** (`com.unity.animation.rigging`). Without it, head rotation is robotic.
- **The relief path matters as much as escalation.** Dropping tension should feel rewarding: warm colors returning, birds in ambient audio, NPCs resuming chatter.
- **Accessibility is non-negotiable.** Never rely on a single channel. Visual + audio + behavioral together.
- **Add hysteresis to tier boundaries.** Drop from tier 2 to 1 at 0.45, not 0.50, to prevent flickering.
