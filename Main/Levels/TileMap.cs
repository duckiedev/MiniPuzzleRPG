using Godot;
using System;

public class TileMap : Godot.TileMap
{
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
    public override void _Ready()
    {
        
    }

    public void SwapTile(Node body, Vector2 position, int tileIndex)
    {
        SetCellv(position/CellSize, tileIndex);
    }

}
