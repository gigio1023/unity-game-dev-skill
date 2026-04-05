---
name: unity-free-resources
description: "Free resource pipeline for Unity game projects. Sourcing tables for Audio (Freesound, Sonniss, FMOD), 3D Models (Kenney CC0, Quaternius CC0, Poly Pizza), Animation (Mixamo), UI (game-icons.net, Kenney UI), Fonts (Noto CJK, Pretendard). License details, import workflows, batch processing. Use when finding free assets, setting up audio, importing models, or checking licenses. Triggers: free assets, CC0, royalty-free, Kenney, Mixamo, Freesound, asset pipeline, license"
---

# Free Resource Pipeline for Unity

Systematic approach to finding, importing, and attributing free game assets for Unity projects. All sources verified as genuinely free (not trial/freemium).

## Quick Reference: Best Sources by Category

| Category | Primary Source | License | URL |
|----------|---------------|---------|-----|
| SFX/Ambience | Freesound.org | CC0/CC-BY | freesound.org |
| Bulk SFX | Sonniss GDC Bundle | Royalty-free | sonniss.com/gameaudiogdc |
| BGM | FreePD / Incompetech | CC0 / CC-BY 4.0 | freepd.com / incompetech.com |
| Audio System | FMOD Studio | Free (indie <$200k) | fmod.com |
| 3D Props | Kenney | CC0 | kenney.nl/assets |
| 3D Props (alt) | Quaternius | CC0 | quaternius.com |
| 3D Models | Poly Pizza | CC-BY | poly.pizza |
| NPC Animations | Mixamo | Free (Adobe) | mixamo.com |
| Animation Plugin | Animancer Lite | Free (Asset Store) | Asset Store |
| UI Icons | game-icons.net | CC-BY 3.0 | game-icons.net |
| UI Kit | Kenney UI Pack | CC0 | kenney.nl/assets/category:UI |
| Korean Font | Noto Sans CJK KR | SIL OFL | fonts.google.com |
| Korean Font (alt) | Pretendard | SIL OFL | github.com/orioncactus/pretendard |

---

## Audio Pipeline

### Ambient/SFX from Freesound.org

Best practice: search → filter by CC0 → download → convert to Unity-compatible format.

**Recommended searches for common game scenarios:**

| Scenario | Search Terms | Recommended Uploaders |
|----------|-------------|----------------------|
| Indoor ambience | "room tone", "air conditioning hum" | InspectorJ, klankbeeld |
| Store/shop | "refrigerator hum", "door chime store" | InspectorJ |
| Footsteps | "footsteps indoor tile", "footsteps concrete" | Nox_Sound, mypantsfelldown |
| UI sounds | "click", "notification", "menu select" | Leszek_Szary |

**Import settings for Unity:**

```
Ambient/BGM loops:
  Load Type: Streaming
  Compression: Vorbis (Quality 70%)
  Sample Rate: Override → 44100 Hz
  Load In Background: true

SFX one-shots:
  Load Type: Decompress On Load
  Compression: PCM (short clips) or Vorbis (longer)
  Load In Background: false
```

### Sonniss GDC Bundle

Every year, Sonniss releases 30+ GB of royalty-free audio. Check `sonniss.com/gameaudiogdc` for current year bundle. No attribution required, commercial use OK.

### Music from FreePD / Incompetech

- **FreePD**: All CC0. Browse by mood: "Eerie", "Ambient", "Upbeat"
- **Incompetech** (Kevin MacLeod): CC-BY 4.0. Requires attribution in credits.
  - Useful genre filters: Ambient + Dark, Mystery, Tension

---

## 3D Model Pipeline

### Kenney (CC0 — Best First Stop)

kenney.nl/assets — completely free, no attribution needed, commercial OK.

**Game-relevant packs:**
- Furniture Kit — shelves, counters, tables, chairs
- Food Kit — bottles, cans, boxes, fruit
- UI Pack — buttons, panels, icons
- Nature Kit — trees, rocks, grass

**Import to Unity:**
1. Download as FBX or OBJ
2. Import into `Assets/Models/Kenney/`
3. Materials: assign URP Lit shader, adjust colors to match project palette

### Quaternius (CC0)

quaternius.com — similar style to Kenney, good supplementary source.
- Ultimate Food Pack, Furniture Pack, City Pack

### Poly Pizza (CC-BY, varies)

poly.pizza — community models. Filter by CC0 or CC-BY. Check license per model.

### Style Consistency Trick

When mixing assets from different sources, create a **palette texture** (a small texture with your project's color palette) and remap materials to use colors from this palette. This unifies the look across Kenney, Quaternius, and custom models.

---

## Animation Pipeline

### Mixamo (Free, Adobe Account)

mixamo.com — free humanoid animations, no attribution required.

**Workflow:**
1. Browse animations (search: "idle", "walk", "talk", "gesture")
2. Download as FBX → "Without Skin" (skeleton only)
3. Import to `Assets/Animations/Mixamo/`
4. Set Rig → Humanoid in import settings
5. Retarget to your characters via Unity's Avatar system

**Essential clips for NPC simulation:**
- Idle variants (breathing, looking around, weight shift) × 3
- Walk (casual pace) × 1
- Talk gesture variants × 2-3
- Reach/pick up × 1
- Work motion (wiping, stacking) × 1

### Unity Asset Store Free Packs

- **Basic Motions FREE** by Kevin Iglesias — idle, walk, run, jump
- **Starter Assets Third Person** by Unity — locomotion + character controller

---

## UI Pipeline

### game-icons.net (CC-BY 3.0)

4,000+ SVG game icons. Download as PNG, color in Unity.
- Attribution required: `Game-icons.net` in credits

### Kenney UI Pack (CC0)

Buttons, panels, sliders, checkboxes, speech bubbles. Clean minimal style. No attribution.

---

## Font Pipeline (Korean / CJK)

### Noto Sans CJK KR (SIL OFL)

For TextMeshPro:
1. Download Noto Sans CJK KR Regular
2. TMP Font Asset Creator: Atlas 4096×4096
3. **Subset to 2,780 high-frequency characters** (99% coverage, not all 11,172)
4. Set as FIRST in TMP fallback chain (TMP stops at first match)

### Pretendard (SIL OFL)

Modern Korean font, good for UI. Same TMP workflow as above.

---

## Attribution Template

Create `ATTRIBUTION.md` in project root:

```markdown
# Asset Attribution

## Audio
- [Description]: [Author] via Freesound.org ([License])
- BGM: [Track name] by [Author] via FreePD (CC0) / Incompetech (CC-BY 4.0)

## 3D Models  
- [Pack name]: Kenney.nl (CC0)
- [Pack name]: Quaternius.com (CC0)

## Animations
- NPC animations: Mixamo.com (Adobe, free for use)

## UI
- Game icons: game-icons.net (CC-BY 3.0)

## Fonts
- Noto Sans CJK KR: Google Fonts (SIL Open Font License)
```

---

## License Quick Reference

| License | Attribution? | Commercial? | Modify? |
|---------|-------------|-------------|---------|
| CC0 | No | Yes | Yes |
| CC-BY 3.0/4.0 | Yes | Yes | Yes |
| SIL OFL | Yes (font name) | Yes | Yes |
| Royalty-free | Varies | Yes | Varies |
| MIT | Yes (in source) | Yes | Yes |

**Safest for game release:** CC0 and MIT. Zero friction, zero legal risk.
