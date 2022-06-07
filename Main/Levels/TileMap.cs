using Godot;
using System;

public class TileMap : Godot.TileMap
{
    public enum tiles {
        FLOOR = 2,
        WATER = 3,
        WATER_LEDGE = 4,
        BREAKABLE_FLOOR = 8,
        BRIDGE_VERTICAL = 9,
        BRIDGE_HORIZONTAL = 10,
        SPOT_TOGGLE_UP = 14,
        SPOT_TOGGLE_DOWN = 11,
        BOX_BRIDGE_LEDGE = 12,
        BOX_BRIDGE_WATER = 13,
        TABLET_LEFT = 15,
        TABLET_RIGHT = 16,
        WALL = 17,
        LEDGE = 18
    }
    public override void _Ready()
    {
        
    }

    public void SwapTile(Node body, Vector2 position, TileMap.tiles tileIndex)
    {
        SetCellv(position/CellSize, (int)tileIndex);
    }

}
