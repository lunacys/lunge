# LunarisGameEngine (lunge)

A MonoGame + MonoGame.Extended Game Engine. The project aims to be as simple to use as possible to create 2D games. The game engine provides a lot of useful instruments for creating games.

**CURRENT STAGE:** Moving and adapting code from [BeatTheNotes Library](https://github.com/lunacys/BeatTheNotes)

## Description

Every component of the game engine is separated into its own module. A module is a different project that implements some important component. The list of the modules is below.

### Modules

 - ```lunge.Library``` - the main libary that contains the misc API.
 - ```lunge.Library.Debugging``` - module that implements some useful stuff in order to make debugging easier.
 - ```lunge.Library.Gui``` - GUI module.
 - ```lunge.Library.Physics``` - impulse-based physics.
 - ```lunge.Library.Collisions``` - the lite version of the ```lunge.Physics``` module. Should be used when a game doesn't need complicated physics. Contains AABB collision detection and collision responses.
 - ```lunge.Library.Ai``` - AI module. Contains behaviors (e.g. steering) and path finding algorithms.
 - ```lunge.Library.Scripting``` - this module contains an easy to use scripting API in either runtime-compiling C# scripts.
 - ```lunge.Library.ContentPipelineExtensions``` - content pipeline extensions for better experience.
 
### Tools

In addition to the modules the game engine contains the following tools:

  - ```lunge.MapEditor``` - a complex tool that allows an user create maps for their games;
  - ```lunge.EntityEditor``` - an easy way to create game enities and then use them using the map editor;
  - ```lunge.ParticleEditor``` - create particle systems, emitters, particles, etc;
  - ```lunge.ShaderEditor``` - an easy-to-use shader editor;
  - ```lunge.LocalizationManager``` - a simple tool for creating localizations for games.
  
  
  ### Tests
  
  Every module of the game engine is covered by unit tests. The tests are placed in the ```lunge.Tests``` project. The project uses NUnit framework.
