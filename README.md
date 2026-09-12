# Xenon

A 2D top-down scrolling shoot 'em up built for COSC2349 (Games Studio 2), inspired by the Amiga/Atari ST classic *Xenon 2: The Megablast*. Fly through parallax-scrolling levels, fight enemy waves and a boss, spend earned score in a shop between levels, and try to survive.

## Assignment Requirements

Built against the COSC2349 Assignment 2 spec (`2d-top-down/docs/SPECIFICATION.md`).

**Required features**
- Parallax scrolling — layered `ParallaxBackground`/`ParallaxLayer` (stars, nebula, animated meteors) in both levels
- Enemy waves — batched `PatternMover` spawns (`Main.cs`), stationary `WallHugger` shooters, and a multi-part `BossEnemy` fight on level 2
- Power-ups — `PowerUp.cs`: 5 temporary pickups (triple-shot, speed, laser, shield, invincibility) + 3 permanent shop upgrades (missile, drone, dodge)
- Player-controlled spaceship — `Player.cs`, full movement/shoot/dodge/health kit

**Core mechanics**
- *Physics:* `CharacterBody2D` + `MoveAndSlide` for the player; everything else is signal-driven `Area2D`/`StaticBody2D` overlap detection — no unused `RigidBody2D` simulation
- *Rendering:* sprites/tiles built from shared texture atlases for automatic draw-call batching, plus `ParallaxBackground` layers and `CanvasLayer` UI overlays
- *Collision:* layer-separated (`Terrain`/`Ship`), `"enemies"` group filtering, engine broadphase via `area_entered`/`body_entered` signals rather than manual per-frame checks
- *Optimization:* off-screen bullets/enemies self-free via `VisibleOnScreenNotifier2D`; stationary enemies use `StaticBody2D` instead of a simulated body; shared atlases keep draw calls low

**Aiming for the top rubric band**
- *Functionality (13–16):* all four required features implemented and working end-to-end across 2 levels, a shop, and a boss fight — fully playable
- *Technical Implementation (10–12):* layered project structure (`scenes/`, `autoload/`, `effects/`, `resources/`), signal-driven collision, batched rendering, no needless physics overhead — targets a smooth 60 FPS
- *Creativity & Polish (3–4):* original music/SFX, animated parallax, pulsing UI, shield/pause/high-score polish, chroma-key shader on meteor sprites

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

## Future Work

- A piercing/splash power-up, so awkwardly-angled `WallHugger`s tucked into corners don't require a direct hit to clear
- More enemy types beyond `PatternMover`/`WallHugger`/`BossEnemy` — e.g. a mine layer, a shielded enemy, or one that only dies to a specific weapon
- Additional levels beyond the current two, with their own boss and difficulty curve
- Fix backwards-movement handling (see Known Issues) so levels support non-linear player movement
- Per-level or endless difficulty scaling instead of fixed spawn parameters

## Credits

- Graphics starter kit (ships, enemies, pickups, UI): provided by COSC2349 (Games Studio 2)
- Parallax background art: [Nebula Parallax Vol. 2](https://nebulakit.itch.io/nebula-parallax-vol2)
- Sci-fi game music pack: [30 Sci-fi Space Tracks Music Pack](https://alkakrab.itch.io/sci-fi-music-pack-vol-3)
- Short laser gun shot: [Mixkit — Space Shooter Sound Effects](https://mixkit.co/free-sound-effects/space-shooter/)
- Continuous laser: [Pixabay - freesound_community](https://pixabay.com/sound-effects/search/laser/)

### Team

Aaron Tran · Andrew Loi · Donovan Ong · Jayden Bach
