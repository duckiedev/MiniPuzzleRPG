using Godot;
using System;

public class Spot : Area2D
{
    private Data data;
    private TileMap tileMap;
    private AudioManager audioManager;

    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        await ToSignal(Owner, "ready");
        tileMap = GetTree().Root.GetNode<TileMap>("Level/TileMap");
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
                    audioManager.PlaySFX(data.sfxTree.boxWaterSFX);
                    tileIndex = TileMap.tiles.BOX_BRIDGE_WATER;
                break;

                case (int)TileMap.tiles.WATER_LEDGE:
                    audioManager.PlaySFX(data.sfxTree.boxWaterSFX);
                    tileIndex = TileMap.tiles.BOX_BRIDGE_LEDGE;
                break;

                case (int)TileMap.tiles.LEDGE:
                    audioManager.PlaySFX(data.sfxTree.boxGroundHoleSFX);
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