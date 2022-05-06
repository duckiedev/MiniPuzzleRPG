using Godot;
using System;

public class TileMap : Godot.TileMap
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public enum tiles {
        FLOOR,
        WALL,
        WATER,
        WATER_LEDGE,
        BRIDGE_VERTICAL,
        STATUE_TOP,
        STATUE_BOTTOM,
        WALL_TOP,
        FLOOR_TL,
        FLOOR_TR,
        FLOOR_BL,
        FLOOR_BR,
        BOX_BRIDGE_LEDGE,
        LEDGE,
        BOX_BRIDGE_WATER,
        SPOT_TOGGLE_DOWN,
        SPOT_TOGGLE_UP,
        BREAKABLE_FLOOR,
        BRIDGE_HORIZONTAL
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
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
