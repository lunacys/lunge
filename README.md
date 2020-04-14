# LunarisGameEngine (lunge)

A cross-platform 2D and 3D Game Engine written in C# using **MonoGame** and **MonoGame.Extended**.

[![Build status](https://ci.appveyor.com/api/projects/status/1jkjxg3iupocpniy?svg=true)](https://ci.appveyor.com/project/lunacys/lunge)

| Branch | Badge |
| --- | --- |
| `master` | ![Cake CI](https://github.com/lunacys/lunge/workflows/Cake%20CI/badge.svg?branch=master) |
| `develop` | ![CI](https://github.com/lunacys/lunge/workflows/Cake%20CI/badge.svg?branch=develop) |

## Description

### Documentation

Compiled documentation can be found [here](http://loonacuse.link/lunge).

The sources are in the `docs/` directory. You probably need files with `*.md` extension. The rest are compiled sources.

### Dependencies

Every required dependency is taken as a NuGet package. So everything will be ok after just restoring them using this command:

```bash
dotnet restore
```

### Build

#### Projects

In order to build the engine and the demos, you need to download **.NET Core 3.1 SDK** or higher. Next, go to the root directory of the repo, and run `build.ps1` if you're on Windows, or `build.sh` if you're on Linux or MacOS.

Engine sources are located at `./Source/lunge.Library`.

Engine Tests are located at `./Source/Tests/lunge.Lib`.

Demos are at `./Source/Demos/`.

#### Docs

To build the docs you will need NPM to be installed on your PC. After NPM is ready, install `gitbook-cli` package globally using the following command in your command shell (e.g. git bash):

```bash
npm install -g gitbook-cli
```

Next, go to the Docs directory and run there:

```bash
gitbook build
```

After that, the book is ready to use from `_books/` directory as `index.html` file.

### Current roadmap

#### TODO

- [x] Game Engine Architecture (On paper)
- [ ] ECS Basis/Architecture
- [ ] Main Components/Systems:
  - [ ] Input handlers
    - [x] Keyboard
    - [x] Mouse
    - [ ] GamePad
    - [ ] Handler as component
    - [ ] Input commands
  - [ ] Entity components:
    - [x] Sprite
    - [x] Animation
  - [x] Resource Manager/Dispatcher
  - [ ] System container
  - [ ] States
  - [x] World
- [x] Asset manager
- [ ] GUI
  - [x] Parent/Child controls
  - [ ] Controls:
    - [x] Canvas
    - [x] Label
    - [ ] Chart
      - [x] Base data view (points + lines)
      - [ ] Bar view
      - [ ] Tooltips with formatted data
      - [ ] Helper buttons & labels (clear, average value, etc.)
      - [ ] Move left/right
      - [ ] Take snapshot
    - [x] Button
    - [x] Tooltip
    - [x] Panel
    - [ ] TreeView
    - [ ] TextBox
    - [ ] RadioButton
    - [ ] CheckBox
    - [ ] ComboBox
    - [ ] GroupBox
    - [ ] ScrollBar
    - [ ] LinkLabel
    - [ ] ProgressBar
    - [ ] TrackBar
    - [ ] GridView
      - [x] Basic output: draw a grid with attached controls (e.g. label, button, etc)
      - [ ] Data source support
  - [ ] Make all the properties of a control changeable
- [ ] Steering behaviors & Steering behavior manager
- [ ] Basic AI
- [ ] Shadows/Lights system
- [ ] Collision detection/Collision resolving
- [ ] Voxel graphics support
- [ ] 2D Physics
- [ ] 3D Physics
- [ ] Map editor
- [ ] Entity editor
- [ ] Particle editor
- [ ] Scripting (modding) with C#
- [ ] Localization editor