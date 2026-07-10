# Architecture

Use this reference when a Unity task requires a dependency, state, lifecycle,
assembly, or testability decision.

## Start with scale

Choose the smallest structure that keeps ownership clear:

- one component and direct serialized references for a local feature;
- an interface when multiple implementations or a test seam already matter;
- an event when several independent observers react and lifetime is controlled;
- a ScriptableObject for authored reusable data or an intentional shared asset;
- a service/composition root when cross-scene lifetime and construction are
  already project concerns;
- a framework only when the project already uses it or the request justifies
  migration cost.

Do not add dependency injection, a global event bus, service locator, or
ScriptableObject event channel solely to make a small feature look scalable.

## Roles

- `MonoBehaviour` owns Unity lifecycle, scene references, and component access.
- `ScriptableObject` is best for authored data or deliberately shared asset
  state; distinguish immutable configuration from runtime mutation.
- Plain C# objects work well for deterministic rules and tests without Unity
  lifecycle.
- Presenter/controller components translate domain state into view behavior.
- A composition root owns construction rather than scattering `Find*` calls.

## State

Use an enum plus one owner for a small, stable state set. Use state objects when
states have substantial enter/exit behavior, independent dependencies, or
testable transitions. Define who may transition and how cancellation works.

Avoid several components independently inferring the same game state from
booleans. Prefer one authoritative state and explicit projections.

## Lifecycle and async

- Pair subscription with unsubscription in compatible lifecycle methods.
- Decide whether disabled objects should retain work or cancel it.
- Treat scene unload and object destruction as cancellation boundaries.
- Do not access Unity objects from background threads.
- Make domain reload, static state, and enter-play-mode settings part of the
  analysis when static initialization matters.

## Assembly definitions and tests

Add or change asmdefs only when the project already uses them or the dependency
boundary is worth the migration. Check platform constraints, Editor-only
assemblies, define constraints, and references in both directions.

Prefer a plain C# core for rules that benefit from fast EditMode tests. Do not
contort simple Unity-facing behavior into abstraction solely for mocking.

Record a lightweight decision note only for choices with durable tradeoffs,
alternatives, or migration consequences.
