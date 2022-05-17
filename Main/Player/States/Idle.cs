using Godot;
using System;

public class Idle : PlayerState
{
    public Godot.Collections.Dictionary inputs;

    public override void _Ready()
    {
        base._Ready();
        inputs = new Godot.Collections.Dictionary();
        inputs.Add("ui_up",Vector2.Up);
        inputs.Add("ui_down",Vector2.Down);
        inputs.Add("ui_left",Vector2.Left);
        inputs.Add("ui_right",Vector2.Right);
    }

    public override void UnhandledInput(InputEvent @event)
    {
        if (!tween.IsActive())
        {
            if (@event.IsActionPressed("reset"))
            {
                GetTree().CallDeferred("reload_current_scene");
                return;
            }
            if (@event.IsActionPressed("pause"))
            {
                stateMachine.TransitionTo("PlayerStates/Disabled");
                GD.Print("Pause from player idle state");
                data.PauseGame();
                return;
            }
            Vector2 direction = Vector2.Zero;
            foreach (string input in inputs.Keys)
            {
                if (@event.IsActionPressed(input))
                {
                    var args = new Godot.Collections.Dictionary();
                    args.Add("dir",(Vector2)inputs[input]);
                    stateMachine.TransitionTo("PlayerStates/Move",args);
                }
            }
        }
    }

    public override void Enter(Godot.Collections.Dictionary msg)
    {
        //playerCollision.Disabled = false;
        parent.Enter(msg);
    }

}
