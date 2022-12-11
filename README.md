# lunge

## Description

**lunge** is a cross-platform 2D Game Framework written in C# using [MonoGame](https://www.monogame.net/) and released under the [MIT License](https://opensource.org/licenses/MIT). lunge provides a solid base for creating fully-featured 2D games.

### ATTENTION!

**This project is heavily under development, so breaking changes after every commit is possible! The docs may be out of date as well!**

## Features

### Completed

 - Pathfinding implementations and examples:
    - A*
    - FlowField
 - Steering behaviors:
    - Seek/Flee/Arrival
    - Evade
    - Collision Avoidance
    - Leader Following
    - Path Following
    - Pursuit
    - Queue
    - Separation
    - Wander
 - Hot-reloadable assets
 - Aseprite animation support
 - FMOD bindings and wrappers
 - Bindables
 - Debugging features:
    - Customizable logging
    - Simple profiling via `GlobalTimeManager`
 - Extended input management implementing the Command pattern
 - Platfrom-specific features, such as clipboard
 - Bitmasking support
 - A lot of utility functions
 - JavaScript scripting support

### In Progress

 - TypeScript declaration file generator (usable now, but probably needs manual adjustments after generation)
 - Particle editor (Nez format)
 - WYSIWYG UI editor (via Nez's tables)

## Samples

You can find demos here: `/src/Demos/`:

 - `AssetsLoading` - showcase of asset hot reloading
 - `InputManagement` - showcase of advanced input handling
 - `Playground` & `Playground2` - A LOT of small samples such as steering behaviors, pathfinding, world generation, etc.

## The latest update (10 Nov 2022)

We decided to redesign the project by adding, removing and replacing a lot of stuff. The list of changes:

 - Remove all the dependencies and added a bunch of new ones (see below)
 - Add `Nez` as main framework on top of which the engine is built.
 - Add `Nez.Persistence` and `Nez.ImGui` as helper libraries (the first is for serialization/deserialization, the second is for Dear ImGUI wrappers)
 - Add `jint` as scripting engine (JavaScript/TypeScript support)
 - Add `LiteNetLib` as network library
 - Remove self-made entity system which is replaced by Nez's implementation
 - Remove GUI-related stuff
 - Add JS scripting/modding support (WIP)
 - Add TypeScript declaration file generator
 - Add Steering Behaviors
 - Add Pathfinding implementations (A* and FlowField)
 - Remove Discord RPC code (to be replaced)
 - A lot of other minor changes and fixes