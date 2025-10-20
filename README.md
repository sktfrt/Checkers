# Checkers Game ♟️

A classic Checkers game implemented in C# with Windows Forms.

## Requirements

### Windows
- .NET 6.0 SDK or later
- Visual Studio 2022 or VS Code

### Linux
- Mono runtime
- mono-complete package

## How to Run

### On Windows
```bash
dotnet build
dotnet run
```

### On Linux
```bash
mcs -r:System.Windows.Forms -r:System.Drawing -r:System.Data *.cs
mono Board.exe
```

## Game rules
[rules](https://ru.wikipedia.org/wiki/%D0%A0%D1%83%D1%81%D1%81%D0%BA%D0%B8%D0%B5_%D1%88%D0%B0%D1%88%D0%BA%D0%B8)

## Controls
- Click on a piece to select it
- Click on highlighted square to move
- Game automatically handles turns and captures