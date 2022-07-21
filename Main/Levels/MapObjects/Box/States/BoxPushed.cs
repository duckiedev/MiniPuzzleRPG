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
            if (box.fall)
            {
                GD.Print("Beep!");
                Camera2D camera = GetNode<Camera2D>("/root/Level/Camera2D");
                HeightMap heightMap = GetNode<HeightMap>("/root/Level/HeightMap");
        
                int cameraLimitV = camera.LimitBottom/Data.gridSize;
                GD.Print(cameraLimitV);
                int boxZ = box.ZIndex;
                GD.Print(boxZ);
                for (int i = 0; i < cameraLimitV; i++)
                {
                    int tileCheck = (int)heightMap.GetCellv(box.Position+(vectorPos*i));
                    GD.Print("check pos : " + (box.Position+(vectorPos*i)));
                    GD.Print("Tile : " + tileCheck);
                    if (tileCheck == boxZ-1)
                    {
                        vectorPos = vectorPos * i;
                        GD.Print("vectorPos : " + vectorPos);
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
                0.1f,
                Tween.TransitionType.Sine,
                Tween.EaseType.InOut
            );
            tween.Start();
            await ToSignal(tween,"tween_completed");
            GD.Print("gobackidle");
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
