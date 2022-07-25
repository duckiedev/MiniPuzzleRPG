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
            player.vectorPos = dir * Data.gridSize;
        }
        MovePlayer(player.vectorPos);
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
        var fallTime = 0.1f;
        var sfx = player.data.sfxTree.playerMoveSFX;
        if (player.fall)
        {
            fallTime = 0.25f;
            sfx = player.data.sfxTree.fall;
        }
        if (!tween.IsActive())
        {
            tween.InterpolateProperty(
                player,
                "position",
                player.Position,
                player.Position + vectorPos,
                fallTime,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            tweenStarted = true;
            audioManager.PlaySFX(sfx);
        }
    }
}
