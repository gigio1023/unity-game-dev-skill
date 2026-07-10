# unity-game-dev-skill

A portable Unity game-development skill for Codex and Claude Code.

`unity-game-dev` helps an agent inspect an existing Unity project, choose
version-appropriate APIs, implement gameplay without fighting the project's
architecture, and report only the validation it actually observed. Live Editor
control is optional rather than assumed.

## Local Codex setup

Keep this repository as the canonical source and expose it to Codex with one
user-level symlink:

```bash
mkdir -p ~/.agents/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.agents/skills/unity-game-dev
```

Do not run the command over an existing path. Verify the link first when
reinstalling:

```bash
test -L ~/.agents/skills/unity-game-dev
readlink ~/.agents/skills/unity-game-dev
```

Codex scans `$HOME/.agents/skills` and follows symlinked skill folders, so no
copy, `npx skills add` installation, or Codex-specific package directory is
needed. See [the Codex install guide](.codex/INSTALL.md) and the
[official skill location documentation](https://learn.chatgpt.com/docs/build-skills#where-to-save-skills).

The portable package can also be linked into Claude Code's user skill directory
when cross-harness testing is needed; that optional path is documented in
[the Claude Code guide](.claude/INSTALL.md).

## Design

The package has four layers:

- `SKILL.md` is the portable task contract shared by both harnesses.
- `references/` contains focused Unity guidance loaded only when the task needs
  it.
- `adapters/` contains optional provider-specific Editor-control details.
- `docs/` records the deletion audit and ignored maintenance-source checkouts.

The portable core does not require a particular MCP provider, built-in tool
name, invocation syntax, model, or subagent topology. It adapts to the Unity
version, packages, project settings, existing conventions, and evidence
available in the repository.

## What changed from the legacy package

The original package exposed many nested skills, forced a
designer-to-programmer-to-QA agent chain, assumed one Editor-control provider,
and bundled large undated documentation snapshots. That structure could
override good project-local judgment and caused the skill installer to discover
22 separate skills.

The modern package:

- exposes one installable skill;
- keeps review and mutation authority distinct;
- preserves the project's configured input and package choices;
- makes direct work the default and independent review optional;
- separates portable Unity decisions from provider-specific commands;
- uses concise references instead of raw documentation dumps;
- records frozen evaluation cases and known untested surfaces.

The raw dumps remain deleted, but their meaningful topic coverage was restored
after a source audit. See [the legacy content audit](docs/legacy-content-audit.md).

## Structure

```text
unity-game-dev-skill/
├── SKILL.md
├── README.md
├── adapters/
│   └── coplay-unity-mcp.md
├── evals/
│   ├── README.md
│   └── fixtures/minimal-unity/
├── docs/
│   ├── legacy-content-audit.md
│   └── research-sources.md
├── references/
│   ├── architecture.md
│   ├── content-rendering-and-media.md
│   ├── gameplay-systems.md
│   ├── platform-networking-and-xr.md
│   ├── project-workflow.md
│   └── ...
├── .claude/INSTALL.md
└── .codex/INSTALL.md
```

## Validate the package

From the repository root:

```bash
npx skills add . --list
npx skills add . --list --full-depth
```

Both commands should report exactly one skill named `unity-game-dev`.

The frozen evaluation notes live in [evals/README.md](evals/README.md).

## Provenance

This skill was informed by local Unity production workflows and by the broader
coverage in [Besty0728/Unity-Skills](https://github.com/Besty0728/Unity-Skills).
It intentionally focuses on project-sensitive game-development decisions rather
than reproducing a broad Unity documentation toolbox.
