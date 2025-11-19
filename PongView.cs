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
        // Start net from below the menu
        Raylib.DrawLine(
            PongModel.ScreenWidth / 2, PongModel.MenuHeight, 
            PongModel.ScreenWidth / 2, PongModel.ScreenHeight, 
            Color.LightGray
        );

        // 4. Draw Scores
        // Move scores down a bit to clear the menu
        int scoreY = PongModel.MenuHeight + 20;
        Raylib.DrawText(model.PlayerScore.ToString(), PongModel.ScreenWidth / 4, scoreY, 40, Color.Gray);
        Raylib.DrawText(model.ComputerScore.ToString(), 3 * PongModel.ScreenWidth / 4, scoreY, 40, Color.Gray);

        // --- Draw Top Menu Bar ---
        // Background
        Raylib.DrawRectangle(0, 0, PongModel.ScreenWidth, PongModel.MenuHeight, Color.DarkGray);
        
        // Menu Text
        int textY = 10;
        Raylib.DrawText("CONTROLS: P-Pause  R-Restart  Q-Quit", 20, textY, 20, Color.White);
        
        // Slider Logic visualization
        Raylib.DrawText("Ball Speed:", 450, textY, 20, Color.LightGray);

        // Slider Track
        Raylib.DrawRectangle(PongModel.UISliderX, PongModel.UISliderY + 5, PongModel.UISliderWidth, 4, Color.LightGray);
        
        // Slider Knob
        int knobX = (int)(PongModel.UISliderX + (model.SpeedSliderValue * PongModel.UISliderWidth));
        Raylib.DrawCircle(knobX, PongModel.UISliderY + 7, 8, Color.SkyBlue);
        Raylib.DrawCircleLines(knobX, PongModel.UISliderY + 7, 8, Color.White);

        
        if (model.IsPaused)
        {
            Raylib.DrawText("PAUSED", PongModel.ScreenWidth / 2 - 60, PongModel.ScreenHeight / 2 - 20, 50, Color.Red);
            Raylib.DrawText("Press 'P' to Resume", PongModel.ScreenWidth / 2 - 70, PongModel.ScreenHeight / 2 + 40, 20, Color.Gray);
        }

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
