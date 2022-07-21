using Godot;
using System;

public class Move : PlayerState
{
    private Boolean tweenStarted = false;
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.MOVE;
        if (msg.Count > 0)
        {
            var dir = (Vector2)msg["dir"];
            var vectorPos = dir * Data.gridSize;
            MovePlayer(vectorPos);
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
        player.stepsTaken += 1;
        parent.Exit();
    }

    public void MovePlayer(Vector2 vectorPos)
    {
        if (!tween.IsActive())
        {
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
            audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
        }
    }
}
