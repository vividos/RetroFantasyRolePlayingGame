# Development Log

## 2020-11-02 Day 2: Game data

Today I thought about how to handle the game's data. I want to store the
game's state when the user closes the app, so the game can be continued from
where the user left of. Some sort of auto-save functionality. The game state
can contain the player's current stats, the monsters in the game world and
objects that can be picked up. We also have static data that never chages,
e.g. the base map, town layout or the conversation trees for NPCs.

The game state can map to properties in C# classes, and the static game data
could be modeled as C# static properties of a C# class. So today I wrote a
GameData that encapsulates both data. Currently the class is only a skeleton
class, but I wrote a `Save()`, `Load()` and a `Create()` method. For loading
and saving the game state, I use Newtonsoft.Json to just serializing the game
data to JSON. `Create()` just generates a new empty game data object, but
later will be populated with the initial state. The static game data just
contains two properties, the game's name and subtitle.

## 2020-11-01 Day 1: Creating projects

Today I set up my little side project, a retro-style, fantasy RPG game. I
chose MonoGame as the framework, since I already coded some demos and half
finished projects with it. Some of the code will end up here.

Setting up the projects in Visual Studio 2019 was rather easy. The MonoGame
3.8.0 version comes with project templates, no SDK anymore to install. The
generated projects reference NuGet projects, and the MonoGame Content Builder
can be installed as a dotnet tool.

I created an Android project that uses Xamarin.Android and a cross-platform
desktop project. I added some icons from a free icon site, in order to have a
framework to build on. Most of the code will be in the Core project from here
on. It is a .NET Standard project that can also have a Content folder with
MonoGame content.

Finally for today, I ported over a "virtual drawing area" code that provides
a 800x480 drawing area that is scaled automatically to the desktop window's
size (or the Android Activity).
