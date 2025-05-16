using Godot;
using System;

public partial class player : Area2D
{
    [Export]
    public int Speed = 400; // How fast the player will move (pixels/sec).

    private Vector2 _screenSize; // Size of the game window.

    public override void _Ready()
    {
        _screenSize = GetViewport().GetVisibleRect().Size;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = new Vector2(); // The player's movement vector.

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

        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

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
            x: Mathf.Clamp(Position.X, 0, _screenSize.X),
            y: Mathf.Clamp(Position.Y, 0, _screenSize.Y)
        );

        if (velocity.X < 0)
        {
            animatedSprite.Animation = "right";
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.X > 0;
        }
        else if(velocity.X > 0)
        {
            animatedSprite.Animation = "left";
            animatedSprite.FlipV = velocity.X < 0;
        }
        else if(velocity.Y < 0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipH = true;
            animatedSprite.FlipV = velocity.Y > 0;
        }
        else if(velocity.Y > 0)
        {
            animatedSprite.Animation = "down";
            animatedSprite.FlipV = velocity.Y < 0;
        }
    }
}