# Version and Package Checks

Use this reference whenever an implementation depends on Unity version,
packages, render pipeline, input backend, or an optional Editor provider.

## Contents

- Source priority and release-line selection
- Package and project-configuration traps
- Unity 6.2–6.5 object identity
- Authoring AI, Sentis, and ML-Agents boundaries
- Editor-control provider checks

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

Use only public UnityCsReference and package source under their current terms as
agent research. Do not expose subscription, paid, or confidential Unity source
to a coding assistant unless current source-code terms and organization policy
explicitly authorize that use.

As of the 2026 maintenance pass, Unity 6.3 is an LTS line and Unity 6.5 is a
Supported Update release. This is routing context, not an upgrade instruction:
always inspect the project's exact editor patch and package lock before choosing
an API or migration path.

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

### Object identity in Unity 6.2–6.5

Unity introduced `EntityId` in 6.2, expanded deprecations in 6.3–6.4, and makes
more old integer-ID use fail in 6.5. Route code by the exact patch rather than a
single cutoff. Neither `InstanceID` nor `EntityId` is a durable serialized
identity: do not persist either across Editor sessions or domain reloads, cast
IDs to `int`, or use sign/sort order as semantics. Reacquire current handles
after reload. For persistence, use `GlobalObjectId` only within its documented
lifetime, an asset GUID plus local file ID, a scene path, or another locator the
exact Unity/provider version documents as stable.

### Unity authoring and runtime AI

Treat these as separate stacks:

- `com.unity.ai.assistant`: Editor authoring, Assistant, Gateway, Unity MCP,
  generators, and Unity Assistant skills; availability and entitlement are not
  implied by Unity 6 alone.
- `com.unity.ai.inference`: Sentis runtime inference. Current 2.x code can use
  the `Unity.InferenceEngine` namespace even when Package Manager displays
  “Sentis.”
- `com.unity.ml-agents`: training-environment instrumentation and embedded
  policies, paired with a compatible Python `mlagents` release and inference
  package.

Read the manifest, lock file, installed package documentation, namespaces in
source, imported model format, and target-device backend before proposing code.
Do not migrate package IDs or namespaces from product naming alone.

### Editor-control providers

Confirm connection, provider version, available commands, and the loaded project
instead of treating an MCP server as part of Unity itself. Provider availability
does not prove that the Editor is idle, compiled, entitled, authorized by current
project policy, or safe to mutate. For Unity's official MCP, check the installed
Assistant version and access requirements. For community providers, also check
the exact release, enabled tool groups, transport, object-ID strategy, package
provenance, and current Unity Core Standards guidance.
