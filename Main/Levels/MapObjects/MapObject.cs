using Godot;
using System;

public class MapObject : Node2D
{
    [Export] public Boolean crystalSwitch;
    public override void _Ready()
    {
        
    }

    public void BreakCrystals()
    {
        var breakableCrystalNode = GetNode<Node2D>("/root/Level/BreakableCrystals");
        var breakableCrystalChildren = breakableCrystalNode.GetChildren();
        foreach (Node item in breakableCrystalChildren)
        {
            if (item.Name.BeginsWith("MapObject"))
            {
                CallDeferred("queue_free");
            }
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
