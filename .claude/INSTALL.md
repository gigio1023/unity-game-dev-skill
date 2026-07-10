# Optional link for Claude Code

The supported local setup for this repository is the Codex symlink documented
in `.codex/INSTALL.md`. For an explicit cross-harness evaluation, the same
portable package can also be linked into Claude Code:

```bash
mkdir -p ~/.claude/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.claude/skills/unity-game-dev
```

Do not overwrite an existing destination. This optional link does not replace
the `~/.agents/skills/unity-game-dev` link used by Codex.
