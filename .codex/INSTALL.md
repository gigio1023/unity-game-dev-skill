# Link for Codex

From a stable clone of this repository, create a user-level skill symlink:

```bash
mkdir -p ~/.agents/skills
ln -s /absolute/path/to/unity-game-dev-skill ~/.agents/skills/unity-game-dev
```

Do not overwrite an existing file or directory. For an existing symlink, inspect
the target before replacing it:

```bash
test -L ~/.agents/skills/unity-game-dev
readlink ~/.agents/skills/unity-game-dev
```

Codex automatically scans `$HOME/.agents/skills` and follows symlinked skill
folders. The repository root contains `SKILL.md`, so the link target is the
installable package; no copy, `npx` installation, or `~/.codex/skills` path is
needed.

Open a new task or refresh skill discovery after creating the link if the
current task's available-skill list was established before installation.
