# lunge

A cross-platform 2D and, in future, 3D Game Framework written in C# using **MonoGame** and **MonoGame.Extended**.

| Branch | Badge |
| --- | --- |
| `master` | ![Cake CI](https://github.com/lunacys/lunge/workflows/Cake%20CI/badge.svg?branch=master) |
| `develop` | ![CI](https://github.com/lunacys/lunge/workflows/Cake%20CI/badge.svg?branch=develop) |

## ATTENTION!

**This project is heavily under development, so breaking changes after every commit is possible! The docs may be out of date as well!**

## Description

### Documentation

Compiled documentation can be found [here](http://loonacuse.link/lunge) [WIP].

The sources are in the `docs/` directory. You probably need files with `*.md` extension. The rest are compiled sources.

### Implemented

 - Base Game class which takes care about engine internals, just inherit your game from `GameBase` class and you're ready to go;
 - Asset Management with hot-reloadable assets. Hot-reloading work only when it is specified explicitly in the GameBase class. Also, the requirement is to keep all the assets in the original format, that is, not converted to .xnb;
 - Advanced input management based on the Command pattern
 - Expendable Logging system with pre-built Console, File and Drawable (in-game) logging
 - Discord RPC (thanks to [Wobble](https://github.com/Quaver/Wobble) game framework)
 - Entity system (NOT ECS)
 - Game Timers and global Game Timer Manager
 - ImGUI integration
 - Native platform features: clipboard
 - Screen system (currently based on MonoGame.Extended implementation)
 - Utility classes: extended mathematics, Circular Array, Noise Helper, TextureStream, and other extensions

### Building

#### Dependencies

Every required dependency is linked as a NuGet package. So everything will be ok after just restoring using this command:

```bash
dotnet restore
```

#### Projects

In order to build the engine and the demos, you need **.NET Core 3.1 SDK** or higher. Next, go to the root directory of the repo, and run `build.ps1` if you're on Windows, or `build.sh` if you're on Linux or MacOS. Also you can go straight to the sources and either open the solution or run

```powershell
dotnet build
```

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

 - Stabilize the API: thinking about modular system where every subsystem (e.g. advanced input handling, physics, simple collision detection, ImGUI, etc) is a module which can be connected to the game if needed;
 - Rewrite MonoGame.Extended's functionality in order to be taking as few external dependencies as possible;
 - GUI;
 - Add advanced debugging tools: visualize cycles and memory usage for every component, add support for changing params in real time, etc;
 - Add easy to use input command manipulation which is useful for game replays, undos/redos, AI, etc;
 - Add behavioral functions, especially steering behaviors;
 - Write tests;
 - Add playable demo games which shows all engine features;
 - Collision Handling (AABB);
 - Physics;
 - Networking;
 - Scripting in Lua;
 - Dependency Injection or some kind of it (I think modular system will fit well)
 - Advanced graphics features: draw calls, sprite batching, etc
