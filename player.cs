using Godot;
using System;
using static Godot.GD;

public partial class player : CharacterBody2D
{
    [Export]
    public int Speed = 115; // How fast the player will move (pixels/sec).
    public bool isDashing = false;
    public float dashDuration = 0.4f;
    public float speedDashDuration = 0.5f;
    public Vector2 dashDirection = Vector2.Zero;
    public float dashSpeed = 350f ;
    public float dashTimeLeft = 0f ;
    private Vector2 _screenSize; // Size of the game window.

    public override void _Ready()
    {
        _screenSize = GetViewport().GetVisibleRect().Size;
    }

    public override void _PhysicsProcess(double delta)
    {
        
        var velocity = new Vector2() ; // The player's movement vector.
        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (isDashing)
        {
            dashTimeLeft -= (float)delta;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                animatedSprite.Animation = "idle";
                Velocity = Vector2.Zero;
            }
            else
            {
                Velocity = dashDirection * (dashSpeed + Speed);
                MoveAndSlide();
                return;
            }
        }

        velocity = Vector2.Zero;

        if (Input.IsActionJustPressed("ui_accept")) // Dash button
        {
            // Determine current direction and dash that way
            if (Input.IsActionPressed("ui_right")) dashDirection = Vector2.Right;
            else if (Input.IsActionPressed("ui_left")) dashDirection = Vector2.Left;
            else if (Input.IsActionPressed("ui_down")) dashDirection = Vector2.Down;
            else if (Input.IsActionPressed("ui_up")) dashDirection = Vector2.Up;
            else dashDirection = Vector2.Zero;

            if (dashDirection != Vector2.Zero)
            {
                isDashing = true;
                if (Input.IsActionPressed("ui_Shift"))
                {
                    dashTimeLeft = speedDashDuration;
                    animatedSprite.SpeedScale = 1.25f;
                }
                else
                {
                    dashTimeLeft = dashDuration;
                }
                animatedSprite.FlipH = false;
                animatedSprite.Animation = "dash_right"; // Replace with your animation name
                Velocity = dashDirection * dashSpeed;
                MoveAndSlide();
                return;
            }
        }

        if (Input.IsActionPressed("ui_right"))
        {
            if (Input.IsActionPressed("ui_accept"))
            {
                animatedSprite.FlipH = true;
                animatedSprite.Animation = "dash_right";
                velocity.X += 5;
            }
            else
            {
                animatedSprite.Animation = "left";
                velocity.X += 1;
            }
        }

        else if (Input.IsActionPressed("ui_left"))
        {
            animatedSprite.Animation = "right";
            velocity.X -= 1;
        }

        else if (Input.IsActionPressed("ui_down"))
        {
            animatedSprite.Animation = "down";
            velocity.Y += 1;
        }

        else if (Input.IsActionPressed("ui_up"))
        {
            animatedSprite.Animation = "up";
            velocity.Y -= 1;
        }

        if (Input.IsActionPressed("ui_Shift"))
        {
            Speed = 200;
            animatedSprite.SpeedScale = 3;
        }
        else
        {
            Speed = 115;
            animatedSprite.SpeedScale = 2;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
        }
        else
        {
            animatedSprite.Animation = "idle";
        }

        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Position.X,
            y: Position.Y
        );

        if (velocity.X > 0)
        {
            
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.X < 0;
        }
        else if (velocity.X < 0)
        {
            
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.X > 0;
        }
        else if (velocity.Y < 0)
        {
            
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.Y > 0;
        }
        else if (velocity.Y > 0)
        { 
            animatedSprite.FlipV = velocity.Y < 0;
        }

        // Using MoveAndSlide.
        Velocity = velocity; // <-- propriété héritée de CharacterBody2D
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
        }
    }
}

