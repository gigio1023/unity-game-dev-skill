# Codex

Link the repository root into Codex's user skill directory:

```bash
mkdir -p ~/.agents/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.agents/skills/unity-game-dev
```

Codex scans `~/.agents/skills` automatically and follows symlink targets. No
copy or installer command is required. The shared `SKILL.md` contains no
Codex-only tool or subagent requirement; optional Editor providers are under
`adapters/`.

For link verification and refresh notes, follow
[`.codex/INSTALL.md`](../.codex/INSTALL.md).
