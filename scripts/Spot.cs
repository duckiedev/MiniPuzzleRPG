using Godot;
using System;

public class Spot : Area2D
{
    private TileMap tileMap;
    private soundFX soundPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
        tileMap = GetTree().Root.GetNode("World").GetNode("Level").GetNode<TileMap>("TileMap");
    }

    public void _on_Spot_body_entered(Node body)
    {
        if (body.IsClass("TileMap")) return;
        Boolean destroy = true;
        TileMap.tiles tileIndex = 0;
        if (body.IsInGroup("Box"))
        {
            switch (tileMap.GetCellv((Position)/tileMap.CellSize))
            {
                case (int)TileMap.tiles.WATER:
                    soundPlayer.SetSFX(soundPlayer.boxWaterSFX);
                    tileIndex = TileMap.tiles.BOX_BRIDGE_WATER;
                break;

                case (int)TileMap.tiles.WATER_LEDGE:
                    soundPlayer.SetSFX(soundPlayer.boxWaterSFX);
                    tileIndex = TileMap.tiles.BOX_BRIDGE_LEDGE;
                break;

                case (int)TileMap.tiles.LEDGE:
                    soundPlayer.SetSFX(soundPlayer.boxGroundHoleSFX);
                    tileIndex = TileMap.tiles.BOX_BRIDGE_LEDGE;
                break;
            }
        }

        if (tileIndex != 0) tileMap.SwapTile(this.Position,((int)tileIndex));

        if (destroy) {
            body.QueueFree();
            this.QueueFree();
        }

        if (GetParentOrNull<ProgTracker>() != null) {
            GetParent<ProgTracker>().checkProgress();
        }
    }
}