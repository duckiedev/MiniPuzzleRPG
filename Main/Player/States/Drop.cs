using Godot;
using System;

public class Drop : PlayerState
{
    private Boolean tweenStarted = false;
    public override void _Ready()
    {

    }
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.DROP;
        if (msg.Count > 0)
        {
            DropPlayer((Vector2) msg[0]);
        }
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

    public void DropPlayer(Vector2 vectorPos)
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
        }
    }

}
