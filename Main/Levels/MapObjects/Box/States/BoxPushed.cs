using Godot;
using System;

public class BoxPushed : BoxState
{
    public Boolean tweenStarted = false;

    public async override void Enter(Godot.Collections.Dictionary msg)
    {
        if (msg.Count > 0)
        {
            float moveSpeed = 0.1f;
            var sfx = data.sfxTree.boxMoveSFX;
            if (box.fall)
            {
                sfx = data.sfxTree.fall;
                moveSpeed = 0.25f;
            }
            //PushBox(vectorPos);
            tween.InterpolateProperty(
                box,
                "global_position",
                box.Position,
                box.Position + box.vectorPos,
                moveSpeed,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            audioManager.PlaySFX(sfx);
            await ToSignal(tween,"tween_completed");
            box.stateMachine.TransitionTo("BoxStates/BoxIdle");
            if (!box.fall) player.stateMachine.TransitionTo("PlayerStates/Idle");
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
