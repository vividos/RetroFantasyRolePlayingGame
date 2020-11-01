# Development Log

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
