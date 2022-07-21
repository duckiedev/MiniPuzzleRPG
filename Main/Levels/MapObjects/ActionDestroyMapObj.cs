using Godot;
using System;
[Tool]
public class ActionDestroyMapObj : Action
{
    [Export] private NodePath mapObjPath;
    private MapObject mapObj;
    public override void Run()
    {
        mapObj = GetNode<MapObject>(mapObjPath);
        mapObj.CallDeferred("queue_free");
    }

}
