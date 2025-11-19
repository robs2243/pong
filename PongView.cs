using Raylib_cs;

namespace pong;

/// <summary>
/// The VIEW handles drawing. 
/// It looks at the Model and draws what it sees on the screen.
/// It does NOT update the game logic.
/// </summary>
public class PongView
{
    /// <summary>
    /// Main drawing function. Called every frame.
    /// </summary>
    /// <param name="model">The data to draw</param>
    public void Draw(PongModel model)
    {
        // 1. Start the drawing phase
        Raylib.BeginDrawing();

        // 2. Clear the background to white (erases the previous frame)
        Raylib.ClearBackground(Color.White);

        // 3. Draw the net (a line in the middle)
        Raylib.DrawLine(
            PongModel.ScreenWidth / 2, 0, 
            PongModel.ScreenWidth / 2, PongModel.ScreenHeight, 
            Color.LightGray
        );

        // 4. Draw Scores
        // Text, X position, Y position, Font Size, Color
        Raylib.DrawText(model.PlayerScore.ToString(), PongModel.ScreenWidth / 4, 20, 40, Color.Gray);
        Raylib.DrawText(model.ComputerScore.ToString(), 3 * PongModel.ScreenWidth / 4, 20, 40, Color.Gray);

        // 5. Draw Paddles
        // We use Raylib.DrawRectangle for the paddles
        Raylib.DrawRectangle(
            (int)model.PlayerPos.X, (int)model.PlayerPos.Y, 
            PongModel.PaddleWidth, PongModel.PaddleHeight, 
            Color.Blue
        );

        Raylib.DrawRectangle(
            (int)model.ComputerPos.X, (int)model.ComputerPos.Y, 
            PongModel.PaddleWidth, PongModel.PaddleHeight, 
            Color.Red
        );

        // 6. Draw Ball
        // We use DrawRectangle for a "retro" square ball look, but DrawCircle works too.
        Raylib.DrawRectangle(
            (int)model.BallPos.X, (int)model.BallPos.Y, 
            PongModel.BallSize, PongModel.BallSize, 
            Color.Black
        );

        // 7. End the drawing phase
        Raylib.EndDrawing();
    }
}
