# Claude Code

The primary local setup is the Codex `~/.agents/skills` symlink. For an
explicit Claude Code evaluation, link the same repository root separately:

```bash
mkdir -p ~/.claude/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.claude/skills/unity-game-dev
```

The shared `SKILL.md` contains no Claude-only invocation, hook, tool, or
subagent requirement. Optional Editor providers are documented under
`adapters/`.

For manual installation, follow [`.claude/INSTALL.md`](../.claude/INSTALL.md).
