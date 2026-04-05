# unity-game-dev-skill

Game-focused Unity agent skill for real production work.

This repo packages `unity-game-dev`: a Unity skill built for **game development workflows**, not just generic Unity Editor operations. It is optimized for feature design handoff, MCP-driven scene mutation, gameplay implementation, QA loops, and the kinds of architecture choices that come up when you are shipping a game prototype or vertical slice.

## Why this exists

The broad Unity skill ecosystem already has good coverage for editor automation and one-off utility actions. In particular, [Besty0728/Unity-Skills](https://github.com/Besty0728/Unity-Skills) is a strong reference pack for wide Unity capability coverage.

This repo exists because I wanted a different center of gravity:

- more **game-dev-oriented orchestration**
- stronger **designer → programmer → QA** flow
- heavier emphasis on **scene mutation + verification** with Unity MCP tools
- a smaller number of more opinionated, high-signal advisory modules instead of a giant flat toolbox

## Recommended setup

I recommend using this **alongside** `Besty0728/Unity-Skills`, not as a strict replacement.

- Use **Besty0728/Unity-Skills** when you want broad Unity utility coverage and many granular skills.
- Use **unity-game-dev** when you want a tighter game-development workflow with stronger architectural guidance, orchestration, and verification habits.

In practice:

- upstream is better as a **wide Unity toolbox**
- this repo is better as a **game-focused operating layer**

## Install

Preferred:

```bash
npx skills add gigio1023/unity-game-dev-skill@unity-game-dev
```

Because this repo is private, manual install docs are useful for authenticated or copy-paste setup.

### Codex

Tell Codex:

```text
Fetch and follow instructions from https://raw.githubusercontent.com/gigio1023/unity-game-dev-skill/refs/heads/main/.codex/INSTALL.md
```

Detailed docs: `docs/README.codex.md`

### Claude Code

Tell Claude Code:

```text
Fetch and follow instructions from https://raw.githubusercontent.com/gigio1023/unity-game-dev-skill/refs/heads/main/.claude/INSTALL.md
```

Detailed docs: `docs/README.claude.md`

### Gemini CLI

Tell Gemini CLI:

```text
Fetch and follow instructions from https://raw.githubusercontent.com/gigio1023/unity-game-dev-skill/refs/heads/main/.gemini/INSTALL.md
```

Detailed docs: `docs/README.gemini.md`

### Cursor

Tell Cursor:

```text
Fetch and follow instructions from https://raw.githubusercontent.com/gigio1023/unity-game-dev-skill/refs/heads/main/.cursor/INSTALL.md
```

Detailed docs: `docs/README.cursor.md`

## What this skill is for

This skill is for tasks like:

- designing and implementing gameplay features
- structuring a mini-game or vertical slice
- scene mutation through Unity MCP tools
- deciding architecture before generating large amounts of code
- debugging gameplay issues with a verification loop
- choosing between patterns like ScriptableObject, events, services, states, and installers
- building UI, dialogue, save systems, camera setups, audio feedback, and NPC logic in a game context

It is **not just “Unity skill”** in the generic sense. It is specifically a **Unity game development skill**.

## What makes it different

### 1. Agent-team workflow is built in

The top-level skill assumes a real delivery loop:

- `game-designer`
- `gameplay-programmer`
- `qa-inspector`

That means the skill is structured around:

- spec first when needed
- implementation second
- QA after every implementation pass

### 2. Router is problem-shaped, not API-shaped

Instead of presenting Unity as a giant bag of unrelated commands, this repo routes by problem:

- architecture
- performance
- NPC / physics
- camera / UI / audio
- testing / CI / Git
- project scouting
- patterns and script roles
- free resources and diegetic feedback

### 3. It is optimized for MCP mutation and verification

The repo strongly prefers:

1. query current scene/project state
2. mutate with MCP or batch commands
3. verify logs/components/play behavior

That is closer to how real Unity work succeeds than simply generating scripts in isolation.

## Repo structure

```text
unity-game-dev-skill/
├── SKILL.md
├── README.md
├── advisory/
│   ├── architecture/
│   ├── blueprints/
│   ├── camera-ui-ux/
│   ├── diegetic-feedback/
│   ├── free-resources/
│   ├── mcp-recipes/
│   ├── patterns/
│   ├── performance/
│   ├── physics-navmesh-ai/
│   ├── project-scout/
│   ├── rendering-performance/
│   ├── scene-contracts/
│   ├── script-roles/
│   ├── scriptdesign/
│   ├── senior-architecture/
│   ├── testability/
│   └── workflow-testing-ci/
├── agents/
│   ├── game-designer.md
│   ├── gameplay-programmer.md
│   └── qa-inspector.md
├── references/
├── scripts/
└── docs/
```

## Provenance

This repo is not presented as a clean-room invention.

It is informed by:

- my local Unity game-dev workflow needs
- the installed `unity-dev` skill bundle I have been using in practice
- ideas and structure that overlap with or were inspired by [Besty0728/Unity-Skills](https://github.com/Besty0728/Unity-Skills)

The goal here is not to hide that lineage. The goal is to turn it into a clearer, better-scoped, game-dev-focused private skill repo.
