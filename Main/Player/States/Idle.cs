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

    public override void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.IDLE;
        var heightMap = GetNode<HeightMap>("/root/Level/HeightMap");
        var tileCheck = (int)heightMap.GetCellv(player.Position/Data.gridSize);
        player.zCurrent = heightMap.CheckTile(player.Position);
        GD.Print(player.zCurrent);
        parent.Enter(msg);
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

            foreach (string input in inputs.Keys)
            {
                if (@event.IsActionPressed(input))
                {
                    var dir = (Vector2)inputs[input];
                    UpdateFacing(dir);
                    if (CheckCollision(dir))
                    {
                        var args = new Godot.Collections.Dictionary();
                        args.Add("dir",dir);
                        stateMachine.TransitionTo("PlayerStates/Move",args);
                    }
                }
            }
        }
    }

    public void UpdateFacing(Vector2 dir)
    {
        animationTree.Set("parameters/idle/blend_position", dir);
        animationTree.Set("parameters/push/blend_position", dir);
    }
    public Boolean CheckCollision(Vector2 dir) {
        var vectorPos = dir * Data.gridSize;
        ray.CastTo = vectorPos;
        ray.ForceRaycastUpdate();
        if (!ray.IsColliding())
        {
            return true;
        }
        else
        {
            var collider = (Node)ray.GetCollider();
            if (collider.IsInGroup("Box"))
            {
                Box colliderBox = collider as Box;
                if (!colliderBox.tween.IsActive())
                {
                    if (colliderBox.CheckCollision(dir))
                    {
                        var args = new Godot.Collections.Dictionary();
                        args.Add("vectorPos",vectorPos);
                        colliderBox.stateMachine.TransitionTo("BoxStates/BoxPushed",args);

                        audioManager.PlaySFX(data.sfxTree.boxMoveSFX);
                        
                        return true;
                    }
                }
            }
        }
        return false;
    }
}