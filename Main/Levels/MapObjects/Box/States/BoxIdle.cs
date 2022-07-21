using Godot;
using System;

public class BoxIdle : BoxState
{
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        box.state = Box.boxState.IDLE;
        var heightMap = GetNode<HeightMap>("/root/Level/HeightMap");
        var tileCheck = (int)heightMap.GetCellv(box.Position/Data.gridSize);
        box.zCurrent = heightMap.CheckTile(box.Position);
        GD.Print("box z : " + box.zCurrent);
        parent.Enter();
    }

}
