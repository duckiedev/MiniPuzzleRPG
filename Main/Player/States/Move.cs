using Godot;
using System;

public class Move : PlayerState
{
    private Boolean tweenStarted = false;
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.MOVE;
        /*if (msg.Count > 0)
        {
            var dir = (Vector2)msg["dir"];
            player.vectorPos = dir * Data.gridSize;
        }*/
        MovePlayer(player.vectorPos);
        parent.Enter();
    }

    public override void Process(float delta)
    {
        if (!player.tween.IsActive())
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
        if (!player.tween.IsActive())
        {
            var fallTime = 0.1f;
            var sfx = player.data.sfxTree.playerMoveSFX;
            if (player.fall)
            {
                fallTime = 0.25f;
                sfx = player.data.sfxTree.fall;
            }
            player.tween.InterpolateProperty(
                player,
                "position",
                player.Position,
                player.Position + player.vectorPos,
                fallTime,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            player.tween.Start();
            tweenStarted = true;
            audioManager.PlaySFX(sfx);
            player.stepsTaken += 1;
            GD.Print(player.Position);
        }
    }
}
