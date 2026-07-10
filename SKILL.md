---
name: unity-game-dev
description: >
  Use when planning, implementing, debugging, or reviewing gameplay in an
  existing Unity project, including C# under Assets, scenes, prefabs, packages,
  input, UI, physics, NavMesh, Editor automation, tests, and performance.
  Adapts to the project's Unity version and available Editor-control capability.
  NOT for engine-neutral game design, ordinary non-Unity C#, standalone asset
  creation, or contributing to the Unity engine source.
---

# Unity Game Development

Deliver the smallest project-aligned result that satisfies the request.
Ground decisions in the repository and prove only what the Unity
version, Editor access, and tests actually establish.

## Quick Path

Classify the request, establish the local Unity baseline, choose the least
invasive execution path, make a project-aligned change, validate with the exact
toolchain when available, and report the observed evidence and unknowns.

For a bounded task, read at most one primary reference unless the work genuinely
spans domains. The normal path is complete in this file; references add detail.

## Authority Boundary

- A review, explanation, diagnosis, or status request does not authorize file
  edits, package changes, Editor mutations, or scene saves.
- A build, fix, change, or implementation request authorizes in-scope local
  edits and non-destructive validation.
- Ask before downloading or importing assets, adding or upgrading packages,
  changing project-wide settings, deleting assets or scenes, overwriting
  authored content, or performing external writes.
- Preserve unrelated worktree and unsaved Editor changes. If the available
  Editor-control path cannot distinguish them, inspect first and stop before a
  risky save or destructive mutation.

## Establish the Project Baseline

Read the smallest relevant set:

- `ProjectSettings/ProjectVersion.txt` for the Unity editor version;
- `Packages/manifest.json` and, when dependency resolution matters,
  `Packages/packages-lock.json`;
- relevant input, rendering, physics, quality, and build settings;
- nearby scripts, asmdefs, tests, prefabs, scenes, and their `.meta` files;
- repository instructions and current worktree state.

Local configuration outranks assumptions. A package in the manifest does not
prove it is enabled or configured. A familiar asset name does not prove its
version or API. Preserve the project's active input backend unless the user
explicitly authorizes a migration.

Use [version and package checks](references/version-and-package-checks.md) when
an API depends on Unity, package, render-pipeline, or input configuration.

## Choose an Execution Path

### Repository-only

Use source and serialized-asset edits when the task can be completed safely
without live Editor state. Treat scene and prefab YAML as authored data:
understand ownership and references before editing, preserve `.meta` files, and
avoid speculative serialization changes.

### Matching Unity Editor or project-native automation

Prefer the exact editor version declared by the project. Use existing build,
test, batchmode, or CI entry points when present. Do not synthesize an ad-hoc
compile using assemblies from another Unity installation, cached template, or
unrelated project and present it as Unity validation.

### Connected Editor control

If a configured provider can inspect and mutate the open project, first verify
the connection, loaded project, active scene, play/edit state, console state,
and provider capabilities. Name capabilities in the workflow—inspect hierarchy,
read components, mutate objects, save, read logs—not assumed tool names.

For scene or prefab work, inspect first, make one bounded mutation, save only
the intended assets, re-inspect references, check compile and console results,
and enter Play Mode only when runtime evidence is required and safe.

Provider-specific setup and optional diagnostics live in the
[Coplay Unity MCP adapter](adapters/coplay-unity-mcp.md). The core workflow must
still work when no Editor provider is available.

## Implementation Rules

- Match existing namespaces, folder boundaries, naming, serialization patterns,
  assembly definitions, and test conventions.
- Keep ownership explicit. Prefer one authoritative component or state owner
  over several scripts reacting independently to the same event.
- Choose architecture in proportion to project scale and change pressure.
  Direct references are often better than a new framework for a local feature.
- Add abstraction only for a visible dependency, testing, lifecycle, or
  authoring need. Avoid hidden initialization order.
- Respect Unity object lifetime and main-thread constraints. Do not treat a
  destroyed `UnityEngine.Object` like ordinary managed null.
- Decide whether time-dependent behavior uses scaled or unscaled time.
- For physics, distinguish `Update` input sampling from `FixedUpdate` physics
  work and avoid competing movement owners.
- For performance, profile before adding pooling, custom update loops, jobified
  code, or rendering complexity.

Read [architecture](references/architecture.md) for dependency and state choices,
or [gameplay systems](references/gameplay-systems.md) for input, movement,
camera, UI, audio, saving, and NavMesh guidance.

Use [content, rendering, and media](references/content-rendering-and-media.md)
for asset/media pipelines; [platform, networking, services, and XR](references/platform-networking-and-xr.md)
for target integrations; and [Editor, serialization, and source control](references/editor-serialization-and-source-control.md)
for YAML, Git/LFS, merge, and CI rules.

## Validation

Use the lowest evidence level that proves the requested result, and state the
level reached:

1. static inspection of source and serialized references;
2. project-native compile or script reload in the matching Unity version;
3. EditMode tests;
4. PlayMode or connected-Editor scenario;
5. target-platform build or runtime profiling.

Do not claim a compile, scene save, package resolution, Play Mode result, or
target build without observing it. If the exact editor or provider is missing,
finish repository-safe work when possible, run available static checks, and
identify the smallest remaining Editor verification.

Read [project workflow](references/project-workflow.md) for evidence levels and
scene mutation, or [performance and testing](references/performance-and-testing.md)
for profiling, EditMode, PlayMode, CI, and target-device checks.

## Design and Review

For feedback work, tie cues to one owned state or event. See
[game feel](references/game-feel.md).

Independent review is optional. Use it only for genuinely separate judgment,
specialized inspection, or a fresh verification pass; direct work is the
default and sequential execution must remain valid. Delegation never expands
authority.

When a focused review would help, use these compact briefs:

- [game-design review](references/game-design-review.md)
- [implementation review](references/implementation-review.md)
- [QA review](references/qa-review.md)

For third-party content, verify the exact asset, current source, and license;
do not describe an asset as “risk-free.” See
[assets and licensing](references/assets-and-licensing.md).

## Output

Lead with the user-visible result. Name changed files/assets or review findings,
the validation and evidence level observed, material assumptions, unresolved
Editor/runtime checks, and the smallest next action when something remains.

## Gotchas

- Moving or regenerating a Unity asset without its `.meta` file changes GUID
  identity and can break serialized references.
- `Time.timeScale = 0` does not pause unscaled time, animation modes, audio, or
  every custom system.
- Text edits to scene or prefab YAML can create valid-looking but broken object
  references; prefer Editor-backed mutation when relationships are non-trivial.
- NavMesh behavior depends on the installed AI Navigation package, baked data,
  agent type, area mask, and runtime placement—not merely a `NavMeshAgent`.
- A clean static review is not evidence that the Editor compiled or that the
  runtime behavior works.
