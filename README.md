# LunarisGameEngine (lunge)

A cross-platform 2D Game Engine written in C# using **MonoGame** and **MonoGame.Extended**.

## Description



### Dependencies

The project uses MonoGame together with **MonoGame.Extended**, **MonoGame.Extended.Input** and **MonoGame.Extended.Particles**. The **MonoGame.Extended** dependencies are placed in the `~/Deps` directory. The reason why the deps are placed just inside a folder as DLL file and not as NuGet package is that there are some changes in the deps and they are compiled as .NET Standard projects.

### Build

#### Projects

In order to build the engine, you need to download **.NET Core 2.2 SDK** or higher. Next, go to the root directory of the repo, and run `build.ps1` if you're using Windows, or `build.sh` if you're using Linux or MacOS.

The sources are located at `./Source/lunge.Library`.

Tests are located at `./Source/Tests/lunge.Lib`.

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
- [ ] Physics
- [ ] Map editor
- [ ] Entity editor
- [ ] Particle editor
- [ ] Scripting (modding) with C#
- [ ] Localization editor