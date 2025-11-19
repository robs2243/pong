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
        // --- Global Input (Always active) ---
        if (Raylib.IsKeyPressed(KeyboardKey.P))
        {
            _model.IsPaused = !_model.IsPaused;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.Q))
        {
            _model.ExitRequested = true;
        }

        if (Raylib.IsKeyPressed(KeyboardKey.R))
        {
            _model.ResetGame();
        }

        // --- Slider Logic (Mouse) ---
        Vector2 mousePos = Raylib.GetMousePosition();
        
        // Check if mouse is clicking/dragging the slider area
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            // Simple hit test: is mouse roughly over the slider bar?
            // We give a little vertical padding for easier grabbing
            if (mousePos.X >= PongModel.UISliderX && mousePos.X <= PongModel.UISliderX + PongModel.UISliderWidth &&
                mousePos.Y >= PongModel.UISliderY - 10 && mousePos.Y <= PongModel.UISliderY + PongModel.UISliderHeight + 10)
            {
                // Calculate new value (0.0 to 1.0)
                float newValue = (mousePos.X - PongModel.UISliderX) / (float)PongModel.UISliderWidth;
                
                // Clamp value
                if (newValue < 0) newValue = 0;
                if (newValue > 1) newValue = 1;

                _model.SpeedSliderValue = newValue;

                // Update current ball speed immediately if the ball is moving
                if (_model.BallVelocity != Vector2.Zero)
                {
                     _model.BallVelocity = Vector2.Normalize(_model.BallVelocity) * _model.CurrentBallSpeed;
                }
                // Also sync paddle speed to the new base speed
                _model.CurrentPaddleSpeed = _model.CurrentBallSpeed;
            }
        }

        if (_model.IsPaused) return;

        // Get the time that passed since the last frame (usually ~0.016 seconds)
        // This ensures movement is smooth regardless of computer speed.
        float deltaTime = Raylib.GetFrameTime();

        // --- 1. Player Input ---
        // If 'W' is pressed, move UP (negative Y)
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            _model.PlayerPos.Y -= _model.CurrentPaddleSpeed * deltaTime;
        }
        // If 'S' is pressed, move DOWN (positive Y)
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            _model.PlayerPos.Y += _model.CurrentPaddleSpeed * deltaTime;
        }

        // Clamp Player Paddle to screen bounds (don't let it go off screen)
        // Top bound is now MenuHeight
        if (_model.PlayerPos.Y < PongModel.MenuHeight) _model.PlayerPos.Y = PongModel.MenuHeight;
        if (_model.PlayerPos.Y > PongModel.ScreenHeight - PongModel.PaddleHeight) 
            _model.PlayerPos.Y = PongModel.ScreenHeight - PongModel.PaddleHeight;


        // --- 2. Computer AI ---
        // Simple AI: The paddle tries to match the Ball's Y position
        // We add a "reaction speed" factor so it's not unbeatable
        float aiCenter = _model.ComputerPos.Y + PongModel.PaddleHeight / 2;
        float ballCenter = _model.BallPos.Y + PongModel.BallSize / 2;

        if (aiCenter < ballCenter - 10)
        {
            _model.ComputerPos.Y += (_model.CurrentPaddleSpeed * 0.85f) * deltaTime; // 0.85f makes it slightly slower than player
        }
        else if (aiCenter > ballCenter + 10)
        {
            _model.ComputerPos.Y -= (_model.CurrentPaddleSpeed * 0.85f) * deltaTime;
        }

        // Clamp Computer Paddle
        // Top bound is now MenuHeight
        if (_model.ComputerPos.Y < PongModel.MenuHeight) _model.ComputerPos.Y = PongModel.MenuHeight;
        if (_model.ComputerPos.Y > PongModel.ScreenHeight - PongModel.PaddleHeight) 
            _model.ComputerPos.Y = PongModel.ScreenHeight - PongModel.PaddleHeight;


        // --- 3. Ball Physics ---
        
        // Move the ball
        _model.BallPos += _model.BallVelocity * deltaTime;

        // Wall Collisions (Top and Bottom)
        // Top wall is now MenuHeight
        if (_model.BallPos.Y < PongModel.MenuHeight)
        {
            _model.BallPos.Y = PongModel.MenuHeight; // Fix position
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
            _model.CurrentPaddleSpeed *= 1.05f; // Speed up paddles too
        }
        
        if (Raylib.CheckCollisionRecs(ballRect, computerRect))
        {
            _model.BallVelocity.X *= -1;
            _model.BallPos.X = _model.ComputerPos.X - PongModel.BallSize - 1;
            
             // Add a little speed up on hit
            _model.BallVelocity *= 1.05f;
            _model.CurrentPaddleSpeed *= 1.05f; // Speed up paddles too
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
