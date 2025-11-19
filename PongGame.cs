using Raylib_cs;

namespace pong;

internal static class PongGame
{
    // STAThread is required for some Windows interop, good practice in Raylib-cs
    [System.STAThread]
    public static void Main()
    {
        // 1. Initialize the Window (The canvas we draw on)
        Raylib.InitWindow(PongModel.ScreenWidth, PongModel.ScreenHeight, "Pong - MVC for Students");
        //Raylib.SetTargetFPS(60); // Lock game to 60 Frames Per Second

        // 2. Create the MVC components
        // MODEL: Creates the data
        PongModel gameModel = new PongModel();

        // CONTROLLER: Logic needs the model to update it
        PongController gameController = new PongController(gameModel);

        // VIEW: Drawing needs nothing to start, but will need the model later to know what to draw
        PongView gameView = new PongView();


        // 3. Main Game Loop
        // This runs 60 times every second until you press ESC or close the window.
        while (!Raylib.WindowShouldClose() && !gameModel.ExitRequested)
        {
            // --- Update Phase ---
            // Calculate movement, collisions, AI, etc.
            gameController.Update();

            // --- Draw Phase ---
            // Draw everything to the screen based on the current Model data
            gameView.Draw(gameModel);
        }

        // 4. Cleanup
        Raylib.CloseWindow();
    }
}