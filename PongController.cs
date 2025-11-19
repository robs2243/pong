using System.Numerics;
using Raylib_cs;

namespace pong;

/// <summary>
/// The CONTROLLER handles logic and input.
/// It changes the Model based on rules and user actions.
/// </summary>
public class PongController
{
    // Reference to the data we are modifying
    private PongModel _model;

    public PongController(PongModel model)
    {
        _model = model;
    }

    /// <summary>
    /// Updates the game state. Called every frame.
    /// </summary>
    public void Update()
    {
        // Get the time that passed since the last frame (usually ~0.016 seconds)
        // This ensures movement is smooth regardless of computer speed.
        float deltaTime = Raylib.GetFrameTime();

        // --- 1. Player Input ---
        // If 'W' is pressed, move UP (negative Y)
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            _model.PlayerPos.Y -= PongModel.PaddleSpeed * deltaTime;
        }
        // If 'S' is pressed, move DOWN (positive Y)
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            _model.PlayerPos.Y += PongModel.PaddleSpeed * deltaTime;
        }

        // Clamp Player Paddle to screen bounds (don't let it go off screen)
        if (_model.PlayerPos.Y < 0) _model.PlayerPos.Y = 0;
        if (_model.PlayerPos.Y > PongModel.ScreenHeight - PongModel.PaddleHeight) 
            _model.PlayerPos.Y = PongModel.ScreenHeight - PongModel.PaddleHeight;


        // --- 2. Computer AI ---
        // Simple AI: The paddle tries to match the Ball's Y position
        // We add a "reaction speed" factor so it's not unbeatable
        float aiCenter = _model.ComputerPos.Y + PongModel.PaddleHeight / 2;
        float ballCenter = _model.BallPos.Y + PongModel.BallSize / 2;

        if (aiCenter < ballCenter - 10)
        {
            _model.ComputerPos.Y += (PongModel.PaddleSpeed * 0.85f) * deltaTime; // 0.85f makes it slightly slower than player
        }
        else if (aiCenter > ballCenter + 10)
        {
            _model.ComputerPos.Y -= (PongModel.PaddleSpeed * 0.85f) * deltaTime;
        }

        // Clamp Computer Paddle
        if (_model.ComputerPos.Y < 0) _model.ComputerPos.Y = 0;
        if (_model.ComputerPos.Y > PongModel.ScreenHeight - PongModel.PaddleHeight) 
            _model.ComputerPos.Y = PongModel.ScreenHeight - PongModel.PaddleHeight;


        // --- 3. Ball Physics ---
        
        // Move the ball
        _model.BallPos += _model.BallVelocity * deltaTime;

        // Wall Collisions (Top and Bottom)
        if (_model.BallPos.Y < 0)
        {
            _model.BallPos.Y = 0; // Fix position
            _model.BallVelocity.Y *= -1; // Reverse Y direction
        }
        else if (_model.BallPos.Y > PongModel.ScreenHeight - PongModel.BallSize)
        {
            _model.BallPos.Y = PongModel.ScreenHeight - PongModel.BallSize;
            _model.BallVelocity.Y *= -1;
        }

        // Paddle Collisions
        // We create Rectangles to check for intersection
        Rectangle ballRect = new Rectangle(_model.BallPos.X, _model.BallPos.Y, PongModel.BallSize, PongModel.BallSize);
        Rectangle playerRect = new Rectangle(_model.PlayerPos.X, _model.PlayerPos.Y, PongModel.PaddleWidth, PongModel.PaddleHeight);
        Rectangle computerRect = new Rectangle(_model.ComputerPos.X, _model.ComputerPos.Y, PongModel.PaddleWidth, PongModel.PaddleHeight);

        if (Raylib.CheckCollisionRecs(ballRect, playerRect))
        {
            _model.BallVelocity.X *= -1; // Bounce horizontally
            _model.BallPos.X = _model.PlayerPos.X + PongModel.PaddleWidth + 1; // Push out of paddle
            
            // Add a little speed up on hit for excitement
            _model.BallVelocity *= 1.05f; 
        }
        
        if (Raylib.CheckCollisionRecs(ballRect, computerRect))
        {
            _model.BallVelocity.X *= -1;
            _model.BallPos.X = _model.ComputerPos.X - PongModel.BallSize - 1;
            
             // Add a little speed up on hit
            _model.BallVelocity *= 1.05f;
        }


        // --- 4. Scoring ---
        
        // Ball went off the Left side (Computer scores)
        if (_model.BallPos.X < -PongModel.BallSize)
        {
            _model.ComputerScore++;
            _model.ResetRound();
        }
        // Ball went off the Right side (Player scores)
        else if (_model.BallPos.X > PongModel.ScreenWidth)
        {
            _model.PlayerScore++;
            _model.ResetRound();
        }
    }
}
