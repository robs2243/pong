# Project Context: C# Pong with Raylib

## Project Overview
This is a C# software project designed to implement a Pong game. It utilizes the **Raylib-cs** library for graphics and input handling. The project is currently in an early stage of development (displaying a basic window with text).

**Technologies:**
*   **Language:** C#
*   **Framework:** .NET 9.0
*   **Graphics Library:** Raylib-cs (v7.0.2)
*   **Type:** Console Application (Exe)

## Building and Running

This project uses the standard .NET CLI.

*   **Build:**
    ```bash
    dotnet build
    ```

*   **Run:**
    ```bash
    dotnet run
    ```

*   **Restore Dependencies:**
    ```bash
    dotnet restore
    ```

## Key Files

*   **`PongGame.cs`**: The main entry point of the application. It contains the `Main` method, initializes the Raylib window, and handles the main game loop (Input -> Update -> Draw).
*   **`pong.csproj`**: The project file defining the SDK (`Microsoft.NET.Sdk`), target framework (`net9.0`), and NuGet dependencies (`Raylib-cs`).

## Development Conventions

*   **Code Style:** Follows standard C# conventions.
*   **Graphics:** All rendering is done via `Raylib-cs` static methods (e.g., `Raylib.InitWindow`, `Raylib.BeginDrawing`).
*   **Main Loop:** The application runs a standard game loop checking `Raylib.WindowShouldClose()`.
