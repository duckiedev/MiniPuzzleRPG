using Godot;
using System;

public class BoxMoveSpawn : BoxState
{
    Boolean tweenStarted = false;
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        if (msg.Count > 0)
        {
            GD.Print("Disabled by BoxMoveSpawn");
            player.stateMachine.TransitionTo("PlayerStates/Disabled");
            var endPosition = (Vector2)msg["endPosition"];
            tween.InterpolateProperty(box,"position",box.Position,endPosition,2,Tween.TransitionType.Sine,Tween.EaseType.Out);
            tween.Start();
            tweenStarted = true;
            audioManager.PlaySFX(data.sfxTree.boxWaterFloat);
        }
        parent.Enter();
    }
    public override void Process(float delta)
    {
        if (!box.tween.IsActive())
        {
            if (tweenStarted)
            {
                stateMachine.TransitionTo("BoxStates/BoxIdle");
                tweenStarted = false;
            }
        }
    }

    public override void Exit()
    {
        player.stateMachine.TransitionTo("PlayerStates/Idle");
    }

}
