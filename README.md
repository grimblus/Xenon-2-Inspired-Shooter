# Xenon

A 2D top-down scrolling shoot 'em up built for COSC2349 (Games Studio 2), inspired by the Amiga/Atari ST classic *Xenon 2: The Megablast*. Fly through parallax-scrolling levels, fight enemy waves and a boss, spend earned score in a shop between levels, and try to survive.

## Version

- **Engine:** Godot 4.7.2 (.NET / C# via `GodotSharp`)
- **Target framework:** .NET 8.0 (net9.0 for Android builds)

## How to Run

1. Open the project in the Godot editor: `2d-top-down/project.godot`.
2. Press **Play** (F5). The game boots straight into the logo sequence → main menu → gameplay; no manual scene selection needed.

Building/testing from the command line (used throughout development for CI-style verification):
```
cd 2d-top-down
dotnet build
godot --headless --path . --quit   # loads the boot scene and exits, useful for catching load errors
```

## Controls

- **Move:** WASD or Arrow Keys
- **Shoot:** Space or Left Click
- **Dodge:** double-tap a direction (requires the Dodge upgrade from the shop)
- **Pause:** Esc (in-level only) — brings up Resume / Retry / Exit

(Also shown in-game via the Controls button on the main menu.)

### Debug Hotkeys

- **`~` (backtick):** jump straight to the shop screen, for testing without playing through a level.

## Project Structure

```
2d-top-down/
├── scenes/
│   ├── player/     Player, Bullet, Missile, Laser, Drone, laser.tres
│   ├── enemies/     WallHugger, PatternMover, BossEnemy
│   ├── powerups/     PowerUp pickup
│   ├── levels/     Main, Scroller, LevelEndTrigger, main.tscn, level_2.tscn
│   └── ui/     Boot sequence, main menu, shop, pause menu, game over, high scores, victory screen
├── autoload/     GameState (score/currency, purchased upgrades), ScoreManager (high scores)
├── effects/     Parallax background decoration scripts
├── resources/     Shared materials/shaders (chroma key, etc.)
├── graphics/, music/, sfx/, fonts/, logos/     Art and audio assets
└── docs/     Assignment specification and project plan
```

## Known Issues

- Level design currently does not account for backwards (upward/downward) player movement relative to the auto-scroll direction. Moving against the scroll can cause movement bugs or leave the player stuck.

## Credits

- Graphics starter kit (ships, enemies, pickups, UI): provided by COSC2349 (Games Studio 2)
- Parallax background art: [Nebula Parallax Vol. 2](https://nebulakit.itch.io/nebula-parallax-vol2)
- Sci-fi game music pack: [30 Sci-fi Space Tracks Music Pack](https://alkakrab.itch.io/sci-fi-music-pack-vol-3)
- Short laser gun shot: [Mixkit — Space Shooter Sound Effects](https://mixkit.co/free-sound-effects/space-shooter/)
- Continuous laser: [Pixabay - freesound_community](https://pixabay.com/sound-effects/search/laser/)

### Team

Aaron Tran · Andrew Loi · Donovan Ong · Jayden Bach
