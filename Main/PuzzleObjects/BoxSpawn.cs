using Godot;
using System;

public class BoxSpawn : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.Flags)] public Boolean enabled = true;
    [Export(PropertyHint.Range, "0,16,1,or_greater")] private int amountMax;
    private int amountCurrent = 0;
    private PackedScene packedScene = GD.Load<PackedScene>("res://scenes/Box.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (enabled) {
            Node world = this.GetParent<Node>();
            if (!world.HasNode("Box")) {
                Spawn();
            }
        }
    }

    public void Spawn() {
        if (amountCurrent < amountMax) 
        {
            amountCurrent++;
            Node world = this.GetParent<Node>();
            var instance = packedScene.Instance();
            world.CallDeferred("add_child",instance);
            Box newBox = (Box)instance;
            newBox.Position = this.Position;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
