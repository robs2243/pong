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
    
    public const int PaddleWidth = 20;
    public const int PaddleHeight = 100;
    public const int BallSize = 20;
    
    public const int PaddleSpeed = 400; // Pixels per second
    public const int BallSpeedInitial = 400;

    // --- Game State (Data that changes) ---

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
        // Place Player on the left, vertically centered
        PlayerPos = new Vector2(50, (ScreenHeight - PaddleHeight) / 2);

        // Place Computer on the right, vertically centered
        ComputerPos = new Vector2(ScreenWidth - 50 - PaddleWidth, (ScreenHeight - PaddleHeight) / 2);

        // Place Ball in the exact center
        BallPos = new Vector2((ScreenWidth - BallSize) / 2, (ScreenHeight - BallSize) / 2);

        // Randomize ball start direction
        // We use a random number generator to decide if it goes left or right, up or down.
        var rand = new Random();
        float dirX = rand.Next(0, 2) == 0 ? -1 : 1; // Left or Right
        float dirY = rand.Next(0, 2) == 0 ? -1 : 1; // Up or Down

        BallVelocity = new Vector2(BallSpeedInitial * dirX, BallSpeedInitial * dirY);
    }
}
