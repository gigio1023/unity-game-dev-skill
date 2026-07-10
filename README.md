# unity-game-dev-skill

A portable Unity game-development skill for Codex and Claude Code.

`unity-game-dev` helps an agent inspect an existing Unity project, choose
version-appropriate APIs, implement gameplay without fighting the project's
architecture, and report only the validation it actually observed. Live Editor
control is optional rather than assumed.

## Install

### Codex

```bash
npx skills add gigio1023/unity-game-dev-skill --skill unity-game-dev --agent codex -g -y
```

See [the Codex install guide](.codex/INSTALL.md) for a manual symlink option.

### Claude Code

```bash
npx skills add gigio1023/unity-game-dev-skill --skill unity-game-dev --agent claude-code -g -y
```

See [the Claude Code install guide](.claude/INSTALL.md) for a manual symlink
option.

If the repository is private, authenticate Git or the GitHub CLI before using
the installer. The installer must be able to clone the repository.

## Design

The package has three layers:

- `SKILL.md` is the portable task contract shared by both harnesses.
- `references/` contains focused Unity guidance loaded only when the task needs
  it.
- `adapters/` contains optional provider-specific Editor-control details.

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
├── references/
│   ├── architecture.md
│   ├── gameplay-systems.md
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
