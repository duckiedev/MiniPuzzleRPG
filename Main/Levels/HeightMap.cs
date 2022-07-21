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
    }

    public int CheckTile(Vector2 position)
    {
        var tile = (int)GetCellv(position/Data.gridSize)-1;
        if (tile != -2) 
        {
            return tile;
        } 
        else
        { 
            return -1;
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
