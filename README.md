# unity-game-dev

`unity-game-dev` is one cross-harness skill for planning, implementing,
debugging, and reviewing work in an existing Unity project. It teaches the
agent to read the project before choosing an API, preserve the project's
architecture and package choices, and report only the validation it actually
observed.

The repository root is the installable package. The same portable `SKILL.md` is
designed for Codex and Claude Code; live Unity Editor control is optional and
isolated in provider adapters.

## Install

Keep one stable clone of this repository and link its root into the harness'
user skill directory. Do not maintain separate copied packages: the symlinks
keep installed harnesses on the same revision.

### Codex

```bash
mkdir -p ~/.agents/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.agents/skills/unity-game-dev
```

Do not run `ln -s` over an existing file, directory, or link. Inspect an
existing destination first:

```bash
test -L ~/.agents/skills/unity-game-dev
readlink ~/.agents/skills/unity-game-dev
```

Codex automatically discovers skill folders under `~/.agents/skills` and
follows symlinks. No `AGENTS.md` declaration, copy, `npx skills add`
installation, or `~/.codex/skills` directory is required. Start a new task or
refresh skill discovery if Codex built its available-skill list before the link
was created. See the [Codex installation notes](.codex/INSTALL.md).

### Claude Code

For Claude Code, link the same repository root separately:

```bash
mkdir -p ~/.claude/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.claude/skills/unity-game-dev
```

This does not replace the Codex link. See the
[Claude Code installation notes](.claude/INSTALL.md).

Unity Assistant uses its own skill locations and permission model. It does not
discover either user-level link above. Supporting Unity Assistant is an
optional, separate installation target—not part of the normal Codex or Claude
Code setup. See
[Unity Assistant skills as an optional runtime](references/ai-and-agent-workflows.md#unity-assistant-skills-as-an-optional-runtime).

## What the skill does

For each task, the skill first establishes the local baseline from
`ProjectSettings/ProjectVersion.txt`, package manifests and locks, project
settings, nearby assets and scripts, repository instructions, and worktree
state. It then chooses the least invasive execution path:

| Path | When it fits | Evidence it can establish |
| --- | --- | --- |
| Repository-only | Source or serialized-asset work that is safe without live Editor state | Static inspection and repository-level checks |
| Matching Editor or project automation | The exact project Editor, tests, batchmode, build scripts, or CI is available | Compile, EditMode, PlayMode, build, or profiling evidence actually run |
| Connected Editor provider | A configured provider can inspect or mutate the open project | Bounded scene or prefab changes, console state, and runtime checks supported by that provider |

Repository-only operation is a complete supported path, not a degraded mode.
The agent should finish safe work there and name the smallest remaining Editor
check instead of inventing compile or Play Mode results.

The domain guidance covers gameplay C#, architecture, input, physics, cameras,
UI, audio, animation, scenes and prefabs, serialization, rendering pipelines,
2D and 3D content, NavMesh, networking, services, XR, performance, testing,
builds, source control, and asset licensing. It also distinguishes three Unity
AI concerns that require different workflows:

- authoring agents: Unity Assistant, MCP, AI Gateway, and connected Editor
  automation;
- runtime inference: the project's installed Sentis/Inference Engine package
  and model formats;
- training gameplay agents: ML-Agents' Unity environment, Python trainer, and
  exported inference model.

The skill is version-aware through Unity 6 release lines, including Unity 6.5,
but never treats “latest” as an instruction to upgrade. The target project's
exact Editor patch, package versions, settings, and supported release line
remain authoritative. See [version and package checks](references/version-and-package-checks.md)
and [AI and agent workflows](references/ai-and-agent-workflows.md).

It is not a general game-design advisor, an ordinary C# or machine-learning
skill, a standalone asset generator, or guidance for contributing to Unity's
engine source. It also does not install, authorize, or replace an Editor
provider.

## Optional Editor providers

The portable core describes capabilities—inspect hierarchy, read components,
mutate objects, save intended assets, read logs—rather than hard-coding one
provider's tool names. Use an adapter only when that provider is already
configured or the user has authorized its setup.

- [Official Unity MCP](adapters/unity-ai-mcp.md) is an optional path for Unity
  6.0+ projects with `com.unity.ai.assistant` and the required Unity access. A
  direct MCP client starts Unity's relay, and the first connection requires
  user approval. Discover the live tool surface instead of assuming built-in
  tool names remain fixed.
- [Coplay Unity MCP](adapters/coplay-unity-mcp.md) documents the current
  provider-specific setup, capability discovery, and diagnostic workflow for
  projects that use Coplay.

Provider availability does not expand task authority. The agent still inspects
before mutation, preserves unsaved or unrelated work, saves only intended
assets, and distinguishes static inspection from observed Editor evidence.
Third-party integrations must also satisfy the project's organizational,
security, and current Unity access requirements.

## Why one skill

The legacy package exposed 22 independently discoverable skills, copied large
undated manual snapshots, assumed one Editor provider, and forced a
designer-to-programmer-to-QA agent chain. That made activation noisy and could
override better project-local judgment.

This package intentionally keeps one judgment and workflow skill instead of
generating a wrapper skill for every Unity topic or MCP tool. Focused references
load only when needed, provider contracts remain replaceable, and optional
review roles do not control execution. The underlying topic-coverage review is
recorded in the [legacy content audit](docs/legacy-content-audit.md).

## Repository map

- [`SKILL.md`](SKILL.md) — portable task contract and normal execution path
- [`references/`](references/) — focused, version-aware Unity decision guidance
- [`adapters/`](adapters/) — optional Editor-provider and runtime integration
  details
- [`evals/`](evals/) — frozen cases, fixture, rubric, observed results, and
  untested cells
- [`docs/research-sources.md`](docs/research-sources.md) — source priority and
  ignored maintenance checkouts

The installable package remains at the repository root. Research clones live
under the ignored `.research/build/` directory so upstream `SKILL.md` files do
not become package entries.

## Validate

From the repository root, verify package discovery:

```bash
npx skills add . --list
npx skills add . --list --full-depth
```

Both commands must report exactly one skill named `unity-game-dev`.

Review [the evaluation record](evals/README.md) before making compatibility
claims. It records the harness, model, fixture, observed cases, and untested
cells. In particular, repository fixtures cannot prove a matching-Editor
compile, connected-Editor mutation, Play Mode behavior, player build, or target
device performance.

## Maintain

When Unity or a package changes, update guidance from the narrowest matching
source in this order: the target project, official documentation for its exact
released version, a matching package release, matching reference source, then
development branches for upcoming changes. Community repositories are useful
for comparison and discovery, not as authority for Unity behavior.

Keep raw documentation snapshots and research clones out of the runtime
package. A removed topic must remain covered by the portable contract, a
focused reference, a provider adapter, deterministic validation, or an explicit
evidence-backed rejection. Refresh procedures and pinned research provenance
live in [the research-source record](docs/research-sources.md).
