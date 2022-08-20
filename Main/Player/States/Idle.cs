using Godot;
using System;

public class Idle : PlayerState
{
    public override void _Ready()
    {
        base._Ready();

    }

    public override void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.IDLE;
        if (player.fall) player.fall = false;
        player.zCurrent = player.CheckTile(typeof(HeightMap),player.Position);
        parent.Enter(msg);
    }

    public override void UnhandledInput(InputEvent @event)
    {
        if (!player.tween.IsActive())
        {
            if (@event.IsActionPressed("select"))
            {
                GetTree().CallDeferred("reload_current_scene");
                return;
            }
            if (@event.IsActionPressed("start"))
            {
                stateMachine.TransitionTo("PlayerStates/Disabled");
                player.data.PauseGame();
                return;
            }

            foreach (String input in player.inputs.Keys)
            {
                if (@event.IsActionPressed(input))
                {
                    var dir = (Vector2)player.inputs[input];

                    UpdateFacing(dir);

                    if (player.CanMove(dir))
                    {
                        player.stateMachine.TransitionTo("PlayerStates/Move");
                    }
                }
            }
        }
    }

    public void UpdateFacing(Vector2 dir)
    {
        animationTree.Set("parameters/idle/blend_position", dir);
        animationTree.Set("parameters/push/blend_position", dir);
        animationTree.Set("parameters/walk/blend_position", dir);
    }

}