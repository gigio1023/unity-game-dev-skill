# Install for Codex

Preferred:

```bash
npx skills add gigio1023/unity-game-dev-skill --skill unity-game-dev --agent codex -g -y
```

For a manual install, clone the repository to a stable location and link the
package root:

```bash
git clone https://github.com/gigio1023/unity-game-dev-skill.git ~/.local/share/unity-game-dev-skill
mkdir -p ~/.agents/skills
ln -s ~/.local/share/unity-game-dev-skill ~/.agents/skills/unity-game-dev
```

If the repository is private, authenticate Git before cloning. Replace an
existing destination intentionally rather than overwriting it blindly, then
restart Codex so skill discovery is refreshed.
