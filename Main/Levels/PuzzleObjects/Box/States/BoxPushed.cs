using Godot;
using System;

public class BoxPushed : BoxState
{
    public Boolean tweenStarted = false;
    public async override void Enter(Godot.Collections.Dictionary msg)
    {
        if (msg.Count > 0)
        {
            var vectorPos = (Vector2)msg["vectorPos"];
            //PushBox(vectorPos);
            tween.InterpolateProperty(
                box,
                "global_position",
                box.Position,
                box.Position + vectorPos,
                0.1f,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            await ToSignal(tween,"tween_completed");
            stateMachine.TransitionTo("BoxStates/BoxIdle");
            player.stateMachine.TransitionTo("PlayerStates/Idle");
        }
        parent.Enter();
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
