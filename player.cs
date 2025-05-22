using Godot;
using System;
using static Godot.GD;

public partial class player : CharacterBody2D
{
    [Export]
    public int Speed = 115; // How fast the player will move (pixels/sec).

    private Vector2 _screenSize; // Size of the game window.

    public override void _Ready()
    {
        _screenSize = GetViewport().GetVisibleRect().Size;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = new Vector2(); // The player's movement vector.
        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (Input.IsActionPressed("ui_right"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.Y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
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
            animatedSprite.Animation = "left";
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.X < 0;
        }
        else if (velocity.X < 0)
        {
            animatedSprite.Animation = "right";
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.X > 0;
        }
        else if (velocity.Y < 0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.Y > 0;
        }
        else if (velocity.Y > 0)
        {
            animatedSprite.Animation = "down";
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

