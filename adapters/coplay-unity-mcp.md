# Coplay Unity MCP Adapter

This optional adapter maps the portable skill's connected-Editor capabilities
to the community [Coplay Unity MCP](https://github.com/CoplayDev/unity-mcp)
provider. It is not an official Unity package or a prerequisite for the skill.

Provider APIs can change. Inspect the installed provider's current tool schema
instead of relying on the names below as a stable contract.

## Capability mapping

When available, map the core workflow to the provider operations that can:

- confirm the connected Unity instance and active project;
- inspect hierarchy, GameObjects, components, scenes, and console logs;
- create or modify scripts and GameObjects;
- execute an `IRunCommand` snippet for bounded Editor-side inspection;
- save the intended scene or asset;
- enter and exit Play Mode;
- re-read logs and affected objects.

Use the provider's live schemas and return values. Do not invent a tool call from
an old example. Confirm edit/play state and the loaded project before mutation.

## Read-only diagnostic snippets

The scripts under `adapters/coplay-unity-mcp/scripts/` are examples for
Coplay's `IRunCommand` surface:

- `check-input-system.cs` inspects active input handling, action assets,
  `PlayerInput` components, and common legacy calls.
- `diagnose-scene.cs` inspects NavMesh agents, triggers, canvases, cameras,
  input handling, character controllers, and missing components.
- `inspect-navmesh.cs` reports triangulation and agent state without adding a
  surface or baking data.

Read and adapt a script to the installed provider and project version before
execution. These examples intentionally avoid scene mutation. They can still
produce console output and scan project files, so run only when relevant.

## Mutation and save gate

Before an Editor mutation:

1. inspect the target object and serialized references;
2. identify dirty scenes/assets and unrelated unsaved work;
3. make one bounded change;
4. register changes using the provider's current undo/modification mechanism;
5. save only assets authorized by the request;
6. re-inspect and check compile/console results.

If the provider cannot show unsaved state or target the intended save, stop
before overwriting authored work. Do not turn a diagnostic into an automatic
NavMesh bake, package install, input migration, or project-settings change.
