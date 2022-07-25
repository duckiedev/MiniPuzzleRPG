using Godot;
using System;

public class HeightMap : Godot.TileMap
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Player>("/root/Player").stateMachine.TransitionTo("PlayerStates/Idle");
        foreach (Node node in GetNode<Level>("/root/Level").GetChildren())
        {
            if (node.IsClass("Box"))
            {
                Box box = node as Box;
                box.stateMachine.TransitionTo("BoxStates/BoxIdle");
            }
        }
    }

 
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
