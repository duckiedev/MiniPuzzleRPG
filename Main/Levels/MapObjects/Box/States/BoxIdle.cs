using Godot;
using System;

public class BoxIdle : BoxState
{
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        box.state = Box.boxState.IDLE;

        if (box.fall) 
        {
            var colliders = box.CheckCollision(Vector2.Zero);
            if (colliders.Contains("StaticBod"))
            {
                Node obj = (Node)colliders["StaticBod"];
                if (obj.GetParent().Name.BeginsWith("MapObject"))
                {
                    MapObject mapObject = (MapObject)obj.GetParent();
                    mapObject.BreakCrystals();
                }
            }
            box.fall = false;
        }
        box.zCurrent = box.CheckTile(typeof(HeightMap),box.Position);
        parent.Enter();
    }

}
