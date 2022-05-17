using Godot;
using System;

public class Move : PlayerState
{
    private Boolean tweenStarted = false;
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        if (msg.Count > 0)
        {
            var dir = (Vector2)msg["dir"];
            var vectorPos = dir * Data.gridSize;
            animationTree.Set("parameters/idle/blend_position", dir);
            animationTree.Set("parameters/push/blend_position", dir);
            
            ray.CastTo = vectorPos;
            ray.ForceRaycastUpdate();
            if (!ray.IsColliding())
            {
                MovePlayer(vectorPos);
                audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
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
                            CallDeferred("MovePlayer",vectorPos);
                        }
                        else
                        {
                            stateMachine.TransitionTo("PlayerStates/Idle");
                        }
                    }
                }
                else
                {
                    stateMachine.TransitionTo("PlayerStates/Idle");
                }
            }
        }
        parent.Enter();
    }

    public override void Process(float delta)
    {
        if (!tween.IsActive())
        {
            if (tweenStarted)
            {
                stateMachine.TransitionTo("PlayerStates/Idle");
                tweenStarted = false;
            }
        }
    }
    public override void Exit()
    {
        parent.Exit();
    }

    public void MovePlayer(Vector2 vectorPos)
    {
        if (!tween.IsActive())
        {
            //playerCollision.Disabled = true;
            //tween.InterpolateMethod(player,"move_and_collide",player.Position,player.Position + vectorPos,0.1f,Tween.TransitionType.Sine,Tween.EaseType.InOut);
            
            tween.InterpolateProperty(
                player,
                "position",
                player.Position,
                player.Position + vectorPos,
                0.1f,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            tweenStarted = true;
        }
    }
}
