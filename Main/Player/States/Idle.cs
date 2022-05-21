using Godot;
using System;

public class Idle : PlayerState
{
    public Godot.Collections.Dictionary inputs;

    public override void _Ready()
    {
        base._Ready();
        inputs = new Godot.Collections.Dictionary();
        inputs.Add("up",Vector2.Up);
        inputs.Add("down",Vector2.Down);
        inputs.Add("left",Vector2.Left);
        inputs.Add("right",Vector2.Right);
    }

    public override void UnhandledInput(InputEvent @event)
    {
        if (!tween.IsActive())
        {
            if (@event.IsActionPressed("select"))
            {
                GetTree().CallDeferred("reload_current_scene");
                return;
            }
            if (@event.IsActionPressed("start"))
            {
                stateMachine.TransitionTo("PlayerStates/Disabled");
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
        player.state = Player.PlayerStates.IDLE;
        parent.Enter(msg);
    }

}
