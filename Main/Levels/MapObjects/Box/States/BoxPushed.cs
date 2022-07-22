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
            var vectorPos = (Vector2)msg["vectorPos"];
            if (box.fall)
            {
                moveSpeed = 0.25f;
                HeightMap heightMap = GetNode<HeightMap>("/root/Level/HeightMap");
                int boxZ = box.zCurrent;
                GD.Print("Box z = " + boxZ);
                GD.Print("Box z needs to be " + (boxZ-1));
                for (int i = 1; i < 128; i++)
                {
                    GD.Print("Tile at " + ((box.Position+(vectorPos*i))/Data.gridSize) + " is " + heightMap.CheckTile(box.Position+(vectorPos*i)));
                    if (heightMap.CheckTile(box.Position+(vectorPos*i)) == (boxZ-1))
                    {
                        vectorPos = vectorPos * i;
                        break;
                    }
                }
            }
            //PushBox(vectorPos);
            tween.InterpolateProperty(
                box,
                "global_position",
                box.Position,
                box.Position + vectorPos,
                moveSpeed,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            await ToSignal(tween,"tween_completed");
            box.stateMachine.TransitionTo("BoxStates/BoxIdle");
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
