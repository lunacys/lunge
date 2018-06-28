# LunarisGameEngine (lunGE)

A super mega game engine

## Description

Every component of the game engine is separated into its own module. A module is a different project that implements some important component. The list of the modules is below.

### Modules

 - ```LunGE.Library``` - the main libary that contains the misc API.
 - ```LunGE.Library.Debugging``` - module that implements some useful stuff in order to make debugging easier.
 - ```LunGE.Library.Gui``` - GUI module.
 - ```LunGE.Library.Physics``` - impulse-based physics.
 - ```LunGE.Library.Collisions``` - the lite version of the ```LunGE.Physics``` module. Should be used when a game doesn't need complicated physics. Contains AABB collision detection and collision responses.
 - ```LunGE.Library.Ai``` - AI module. Contains behaviors (e.g. steering) and path finding algorithms.
 - ```LunGE.Library.Scripting``` - this module contains an easy to use scripting API in either runtime-compiling C# scripts or Larg (**L**ite g**A**me sc**R**ipting lan**G**uage). Larg language will be created after the previous modules are done.
 - ```LunGE.Library.ContentPipelineExtensions``` - content pipeline extensions for better experience.
 
### Tools

In addition to the modules the game engine contains the following tools:

  - ```LunGE.MapEditor``` - allows user to create maps for his game.
  - ```LunGE.EntityEditor``` - an easy way to create game enities and then use them using the map editor
  - ```LunGE.ParticleEditor``` - creating particle systems, emitters, particles, etc.
  - ```LunGE.ShaderEditor``` - easy shader editor.
  - ```LunGE.LocalizationManager``` - a tool for creating localizations for games. 
  
 ### Sample games
 
  The repo contains some examples of game engine usage.
  
  - ```TestGame``` - a test game that uses the latest added stuff.
  
  ### Tests
  
  Every module of the game engine is covered by unit tests. The tests are placed in the ```LunGE.Tests``` project. The project uses NUnit framework.
