# COSC2349 Assignment 1 — Project Plan & Responsibilities

**Group Name:** Process & Tools

> Note: Roles are flexible — while each member leads their assigned area, everyone is expected to contribute across tasks.

## Roles

| Key | Role | Responsibilities | Assigned To |
|---|---|---|---|
| GP | Gameplay/Core Programmer | Player controls, shooting, collision | Aaron Tran |
| AI | Enemy/AI Programmer | Enemy behaviours, wave spawner, difficulty scaling | Andrew Loi |
| SYS | Systems Programmer | Health, scoring, power-ups, game over/restart logic | Jayden Bach |
| VA | Visual & Audio Lead | Parallax layers, asset integration, SFX/music | Donovan Ong |
| TECH | Technical/Optimization Lead | Performance, FPS profiling, build export | Aaron Tran |
| DOC | Documentation & Project Lead | README, design doc, video edit, Gantt/RACI upkeep, repo hygiene | Donovan Ong |

## RACI Chart

R = Responsible · A = Accountable · C = Consulted · I = Informed · R/A = Responsible & Accountable

| Task | GP | AI | SYS | VA | TECH | DOC |
|---|---|---|---|---|---|---|
| Player movement & shooting | R/A | C | C | I | I | I |
| Enemy AI & wave spawning | C | R/A | I | I | I | I |
| Power-ups | C | C | R/A | I | I | I |
| Parallax background & visual assets | I | I | I | R/A | C | I |
| Audio (SFX & music) | I | I | I | R/A | I | I |
| Health, score & UI systems | I | I | R/A | C | I | I |
| Performance optimization | C | C | C | I | R/A | I |
| Version control & integration | C | C | C | C | R | A |
| QA/playtesting | R | R | R | R | R | A |
| Documentation (README, design doc) | C | C | C | C | C | R/A |
| Video demo | I | I | I | C | I | R/A |
| Project plan (Gantt & RACI) | C | C | C | C | C | R/A |
| Milestone presentation | C | C | C | C | C | R/A |

## Gantt Chart — Task List (5-week plan)

Category color key in original: Core Gameplay · Visual & Audio · Game Mechanics & UI · Technical · Deliverables (week-by-week bar placement not preserved in this extraction — refer to the original doc for exact per-week scheduling)

1. Godot 4.5 2D pipeline setup / project scaffolding
2. Version control setup (GitHub repo, branching)
3. Project plan — Gantt & RACI (Assessment 1)
4. Asset sourcing/creation (art + audio)
5. Player ship movement/controls + boundaries
6. Projectile shooting + collision detection
7. Parallax background system (3+ layers, looping)
8. Sprite/asset integration (ship, enemies, power-ups)
9. Enemy type 1: stationary/turret behaviour
10. Enemy type 2: pattern-movement behaviour
11. Enemy type 3: chasing/homing behaviour
12. Power-up 1 (e.g. fire rate)
13. Power-up 2 (e.g. shield) + spawn logic
14. Wave spawner with increasing difficulty curve
15. Lighting/texture pass for visual polish
16. Health system + damage handling
17. Score system + on-screen HUD
18. Game Over screen + restart flow
19. SFX (collisions, pickups, level complete, etc.)
20. Background music integration
21. Performance optimization (60 FPS, object collision)
22. Integration/merge pass
23. QA/playtesting pass (difficulty, collision, balance)
24. QA/playtesting pass (FPS holds under load)
25. Design document
26. README (build/run instructions)
27. Standalone executable/build export
28. Video demo — record, edit, review
29. Milestone presentation
30. Final submission — Assignment 2 (**noted as Sept 7** in this plan; the Assignment 2 brief itself states **Sept 13** — confirm which is correct)

## Notes for Agent Context
- Engine: Godot 4.5
- Repo/version control: GitHub, with branching workflow
- Donovan Ong owns: parallax layers, asset integration, SFX/music (VA), plus README, design doc, video edit, Gantt/RACI upkeep, repo hygiene (DOC)
