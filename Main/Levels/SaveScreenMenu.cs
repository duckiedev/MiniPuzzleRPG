using Godot;
using System;

public class SaveScreenMenu : OptionMenu
{
    /*
    public override void _Ready()
    {
        
    }
    */

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsActionPressed("a"))
        {
            playerTween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                0.5,
                3,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            playerTween.InterpolateProperty(
                playerSprite,
                "position",
                playerSprite.Position,
                playerSprite.Position + new Vector2(0,-96),
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            playerTween.Start();
        }
    }
}
