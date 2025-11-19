using System.Numerics; // Used for math vectors (x, y coordinates)

namespace pong;

/// <summary>
/// The MODEL stores all the data about the game.
/// It knows "what" is happening, but not "how" to draw it or "how" to change it based on input.
/// </summary>
public class PongModel
{
    // --- Game Constants (Settings) ---
    public const int ScreenWidth = 800;
    public const int ScreenHeight = 480;
    public const int MenuHeight = 40; // Reserved space at the top
    
    public const int PaddleWidth = 20;
    public const int PaddleHeight = 100;
    public const int BallSize = 20;
    
    // public const int PaddleSpeed = 400; // REMOVED
    public const int BallSpeedInitial = 400;

    // --- Menu / UI Settings ---
    public bool IsPaused = false;
    public bool ExitRequested = false;
    
    // Slider settings (0.0 to 1.0)
    public float SpeedSliderValue = 0.1f; 
    public const float MinBallSpeed = 200f;
    public const float MaxBallSpeed = 1000f;

    // UI Layout Configuration
    // We put the slider in the top menu bar
    public const int UISliderX = 550; 
    public const int UISliderY = 10;
    public const int UISliderWidth = 150;
    public const int UISliderHeight = 20; 

    // Helper to calculate actual speed from slider value
    public float CurrentBallSpeed => MinBallSpeed + (SpeedSliderValue * (MaxBallSpeed - MinBallSpeed));

    // --- Game State (Data that changes) ---
    
    public float CurrentPaddleSpeed; // Dynamic paddle speed

    // Positions are Vector2 (X, Y)
    public Vector2 PlayerPos;
    public Vector2 ComputerPos;
    public Vector2 BallPos;
    public Vector2 BallVelocity; // Direction and speed of the ball

    // Scores
    public int PlayerScore;
    public int ComputerScore;

    /// <summary>
    /// Constructor: Sets up the game when it starts.
    /// </summary>
    public PongModel()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        PlayerScore = 0;
        ComputerScore = 0;
        ResetRound();
    }

    /// <summary>
    /// Puts the paddles and ball back to their starting spots.
    /// </summary>
    public void ResetRound()
    {
        // Sync Paddle speed with the slider setting at start of round
        CurrentPaddleSpeed = CurrentBallSpeed;

        // Calculate center of the PLAYABLE area (ScreenHeight - MenuHeight)
        float playableCenterY = MenuHeight + (ScreenHeight - MenuHeight) / 2f;

        // Place Player on the left, vertically centered in playable area
        PlayerPos = new Vector2(50, playableCenterY - PaddleHeight / 2f);

        // Place Computer on the right, vertically centered in playable area
        ComputerPos = new Vector2(ScreenWidth - 50 - PaddleWidth, playableCenterY - PaddleHeight / 2f);

        // Place Ball in the exact center of playable area
        BallPos = new Vector2((ScreenWidth - BallSize) / 2f, playableCenterY - BallSize / 2f);

        // Randomize ball start direction
        // We use a random number generator to decide if it goes left or right, up or down.
        var rand = new Random();
        float dirX = rand.Next(0, 2) == 0 ? -1 : 1; // Left or Right
        float dirY = rand.Next(0, 2) == 0 ? -1 : 1; // Up or Down

        BallVelocity = new Vector2(CurrentBallSpeed * dirX, CurrentBallSpeed * dirY);
    }
}
