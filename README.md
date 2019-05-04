# LunarisGameEngine (lunge)

A C# 2D Game Engine.

## Description

### Dependencies

The project uses MonoGame together with MonoGame.Extended, MonoGame.Extended.Input and MonoGame.Extended.Particles. The MonoGame.Extended dependencies are placed in the /Deps directory.

### Build

In order to build the engine, you need to download **.NET Core 2.2 SDK** or higher. Next, go to the root directory of the repo, and run `build.ps1` if you're using Windows, or `build.sh` if you're using Linux or MacOS.

The sources are located at `./Source/lunge.Library`.

Tests are located at `./Source/Tests/lunge.Lib`.

### Current roadmap

- [ ] Game Engine Architecture (On paper)
- [ ] ECS Basis/Architecture
- [ ] Main ECS Components/Systems:
  - [ ] Input handlers: Keyboard, Mouse, GamePad
  - [ ] Entity components:
    - [ ] Sprite
    - [ ] Animation
  - [ ] Resource Manager/Dispatcher
  - [ ] System container
  - [ ] States
  - [ ] World
  - [ ] Dispatcher
  - [ ] Event channel
- [ ] Asset manager
- [ ] GUI
- [ ] Shadows/Lights system
- [ ] Collision detection
- [ ] Physics
- [ ] Map editor
- [ ] Entity editor
- [ ] Particle editor
- [ ] Scripting (modding)
- [ ] Localization editor