using Godot;
using System;

public class BoxIdle : BoxState
{
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        box.state = Box.boxState.IDLE;

        if (box.fall) 
        {

            ray.CastTo = box.Position;
            // Check for Body (MapObject)
            ray.CollideWithBodies = true;
            ray.ForceRaycastUpdate();
            if (ray.IsColliding()) {
                var obj = ray.GetCollider();
                Node objNode = (Node)obj;
                GD.Print("colliding with " + objNode + " with parent object class " + objNode.GetParent().GetClass());
                if (objNode.GetParent().Name.BeginsWith("MapObject"))
                {
                    MapObject mapObject = (MapObject)objNode.GetParent();
                    GD.Print(objNode);
                    GD.Print(mapObject);
                    mapObject.BreakCrystals();
                }
            }
            box.fall = false;
        }
        box.zCurrent = box.CheckTile(typeof(HeightMap),box.Position);
        parent.Enter();
    }

}
