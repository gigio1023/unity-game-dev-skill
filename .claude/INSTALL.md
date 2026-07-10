# Install for Claude Code

Preferred:

```bash
npx skills add gigio1023/unity-game-dev-skill --skill unity-game-dev --agent claude-code -g -y
```

For a manual install, clone the repository to a stable location and link the
package root:

```bash
git clone https://github.com/gigio1023/unity-game-dev-skill.git ~/.local/share/unity-game-dev-skill
mkdir -p ~/.claude/skills
ln -s ~/.local/share/unity-game-dev-skill ~/.claude/skills/unity-game-dev
```

If the repository is private, authenticate Git before cloning. Replace an
existing destination intentionally rather than overwriting it blindly, then
restart Claude Code so skill discovery is refreshed.
