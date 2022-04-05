using Godot;
using System;

public class TileMap : Godot.TileMap
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    private static TileMap instance = null;
    

    public static TileMap GetTileMap(Node n) {
        if (instance == null) {
            instance = n.GetTree().Root.GetNode("World").GetNode("Level").GetNode<TileMap>("TileMap");
        }

        return instance;
    }

    public void SwapTile(Vector2 position, int tileIndex) {
        SetCellv(position/CellSize, tileIndex);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
