# Install for Claude Code

Install the published repository at user scope with the `skills` CLI:

```bash
npx skills add gigio1023/unity-game-dev-skill --global --agent claude-code
```

Confirm the managed installation with:

```bash
npx skills list --global --agent claude-code
```

For a shared Codex and Claude Code installation, use the cross-harness command
in the repository README and keep the CLI's default symlink mode. Use `--yes`
only for an intentionally non-interactive install.
