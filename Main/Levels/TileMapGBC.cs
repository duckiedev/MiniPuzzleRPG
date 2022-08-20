using Godot;
using System;

public class TileMapGBC : Godot.TileMap
{
    public enum tiles {
        FLOOR = 0,
        BREAKABLE_FLOOR = 1,
        WATER = 2,
        WATER_LEDGE = 3,
        BOX_BRIDGE_LEDGE = 4,
        LEDGE = 5,
        BOX_BRIDGE_WATER = 6,
        BRIDGE_VERTICAL = 7,
        BRIDGE_HORIZONTAL = 8,
        SPOT_TOGGLE_DOWN = 9,
        SPOT_TOGGLE_UP = 10
    }
    public override void _Ready()
    {
        
    }

    public void SwapTile(Node body, Vector2 position, TileMapGBC.tiles tileIndex)
    {
        SetCellv(position/CellSize, (int)tileIndex);
    }

}
