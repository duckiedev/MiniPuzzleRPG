using Godot;
using System;

public class BoxPushed : BoxState
{
    public Boolean tweenStarted = false;
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        if (msg.Count > 0)
        {
            var vectorPos = (Vector2)msg["vectorPos"];
            PushBox(vectorPos);
        }
        parent.Enter();
    }

    public override void Process(float delta)
    {
        if (!tween.IsActive())
        {
            if (tweenStarted)
            {
                stateMachine.TransitionTo("BoxStates/BoxIdle");
                player.stateMachine.TransitionTo("PlayerStates/Idle");
                tweenStarted = false;
            }
        }
    }

    public override void Exit()
    {
        if (box.reset) box.ResetBox();
    }
    public void PushBox(Vector2 vectorPos)
    {
        if (!tween.IsActive())
        {
            //boxCollision.Disabled = true;
            tween.InterpolateProperty(
                box,
                "position",
                box.Position,
                box.Position + vectorPos,
                0.1f,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            tweenStarted = true;
        }
    }
}
