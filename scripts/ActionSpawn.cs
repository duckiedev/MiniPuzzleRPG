using Godot;
using System;

public class ActionSpawn : Action
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.File)] private PackedScene spawnFile;
    //private PackedScene spawnResource; 
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //spawnResource = (PackedScene)GD.Load(spawnFile.ResourcePath);
    }

    public override void Run()
    {
        GD.Print("eh?");
        var spawnInstance = spawnFile.Instance();
        GetParent().CallDeferred("add_child",spawnInstance);
        spawnInstance.Set("position",this.Position);
        
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
