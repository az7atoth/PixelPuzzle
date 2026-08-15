# Pixel Puzzle

A small mobile puzzle game made with Unity and released on Google Play.

The project was developed in approximately **one week**. One of my main goals was to go through the complete development and release process of a mobile game — from initial implementation to a publicly available Google Play release.

**Google Play:** [View on Google Play](https://play.google.com/store/apps/details?id=com.Az7Games.PixelPuzzle&pcampaignid=web_share)

## Tech

- Unity / C#
- Zenject
- UniRx
- UniTask
- DOTween

## Project Structure

Despite the small scope and short development time, I tried to keep the code structured and maintainable rather than treating the project as a prototype.

The main project code is located in:

`PixelPuzzle/Assets/Core`

The codebase is separated by areas of responsibility, including gameplay, UI, save/load, analytics, audio, VFX and screen adaptation.

Some of the architectural approaches used in the project:

- **Dependency Injection with Zenject** — game systems and services are connected through interfaces and DI rather than direct dependencies.
- **Reactive event handling with UniRx** — reactive commands are used for communication between gameplay and UI systems.
- **UniTask / async-await** — used for asynchronous operations and initialization with cancellation support.
- **Game states** — the game flow is separated into individual states.
- **Separated services and controllers** — gameplay logic, presentation and supporting systems are kept in separate areas of responsibility.

## About the Project

Pixel Puzzle was deliberately kept small in scope so I could take it from an idea to a finished mobile release within a short period of time.

Unlike my game jam projects, where architectural decisions are often intentionally simplified due to strict deadlines, this project is closer to how I normally approach code organization when I have more control over the development process.

The repository can therefore be used as a compact example of my general Unity/C# coding style and approach to project architecture.
