using Godot;
using System;

public class BoxIdle : BoxState
{
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        box.state = Box.boxState.IDLE;

        if (box.fall) 
        {
            GD.Print("boooops");
            ray.CastTo = box.Position;
            GD.Print(ray.CastTo);
            GD.Print(box.Position);
            GD.Print(box.GlobalPosition);
            // Check for Body (MapObject)
            ray.CollideWithBodies = true;
            ray.ForceRaycastUpdate();
            if (ray.IsColliding()) {
                var obj = ray.GetCollider();
                Node objNode = (Node)obj;
                GD.Print("colliding with " + objNode + " with parent object class " + objNode.GetParent().GetClass());
                if (objNode.GetParent().Name.BeginsWith("MapObject"))
                {
                    MapObject mapObject = objNode as MapObject;
                    mapObject.BreakCrystals();
                }
            }
            box.fall = false;
        }
        var heightMap = GetNode<HeightMap>("/root/Level/HeightMap");
        var tileCheck = (int)heightMap.GetCellv(box.Position/Data.gridSize);
        box.zCurrent = heightMap.CheckTile(box.Position);
        GD.Print("box z : " + box.zCurrent);


        parent.Enter();
    }

}
