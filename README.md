# LunarisGameEngine (lunge)

A cross-platform 2D and 3D Game Engine written in C# using **MonoGame** and **MonoGame.Extended**.

[![Build status](https://ci.appveyor.com/api/projects/status/1jkjxg3iupocpniy?svg=true)](https://ci.appveyor.com/project/lunacys/lunge)

| Branch | Badge |
| --- | --- |
| `master` | ![Cake CI](https://github.com/lunacys/lunge/workflows/Cake%20CI/badge.svg?branch=master) |
| `develop` | ![CI](https://github.com/lunacys/lunge/workflows/Cake%20CI/badge.svg?branch=develop) |

## ATTENTION!

**The project is heavily under development, so breaking changes after every commit is possible! Also the docs may be out of date as well!**

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

 - Fully implement input management and measure its performance
 - Implement GUI interfaces for mine version and ImGUI. Probably need an easy switch for them
 - Add advanced debugging tools: visualize cycles and memory usage for every component, add support for changing params in real time, etc
 - Add easy to use input command manipulation which is quite useful for game replays, undos/redos, AI, etc
 - Add behavioral functions, especially steering behaviors
 - Fix GameSettings and make it more useful
 - Implement new GUI
 - Fully implement base game class (GameBase.cs)
 - Write tests
 - Add new demos for every aspect
 - Add playable demo games to show all the engine's features
 - Collision Handling
 - Physics
 - Networking
 - 3D voxel engine
 - Scripting. Thinking about Lua, C# or self-made alternative. For the last option the Bytecode pattern will be quite useful
