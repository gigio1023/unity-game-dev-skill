# Version and Package Checks

Use this reference whenever an implementation depends on Unity version,
packages, render pipeline, input backend, or an optional Editor provider.

## Source priority

1. Project files: `ProjectVersion.txt`, manifest, lock file, settings, asmdefs.
2. Installed package `Documentation~`, source, and local API usage in the same
   project.
3. Official Unity Manual and package documentation for the exact released
   version.
4. A matching release tag/branch in the official package repository.
5. Matching UnityCsReference source, when available.
6. Current development branches only for upcoming-change awareness.
7. Third-party examples only when the project actually uses that dependency.

Record when a source is undated or targets a different version. Do not copy an
API merely because it appears in a search result or another project.

Unity 6 has LTS and Supported Update release lines. Do not equate “latest” with
“correct for this project.” A development-branch package or alpha
UnityCsReference checkout is not evidence that a released editor exposes the
same API. The local maintenance sources and checked commits are recorded in
`docs/research-sources.md`.

## Frequent traps

### Input

Package installation and active input handling are separate facts. Also inspect
existing action assets, `PlayerInput` usage, legacy calls, UI input module, and
platform controls before proposing a backend.

### Render pipeline

Package entries do not prove which pipeline asset is assigned at every quality
level. Check graphics and quality settings plus the actual renderer assets.

### AI Navigation

Unity's navigation APIs and package ownership have changed across versions.
Check the installed `com.unity.ai.navigation` version and current project code
before using `NavMeshSurface` or baking APIs.

### Test Framework

Check package version, asmdef references, EditMode/PlayMode placement, and
repository test commands. Avoid assuming the newest attributes or CLI flags.

### Camera packages

Inspect whether the project uses a Cinemachine package and which major version.
Names and recommended components differ materially between 2.x and 3.x. Do not
apply 2.x “virtual camera” component recipes to 3.x without migration evidence.

### Rendering packages

Check the active pipeline asset and the Core/URP/HDRP/Shader Graph versions.
Renderer Features, Render Graph, compatibility paths, resource handles, and
custom-pass APIs can change between package releases.

### Editor-control providers

Confirm connection, provider version, available commands, and the loaded project
instead of treating an MCP server as part of Unity itself. Provider availability
does not prove that the Editor is idle, compiled, or safe to mutate.
