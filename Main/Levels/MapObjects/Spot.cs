using Godot;
using System;

public class Spot : Area2D
{
    private Data data;
    private AudioManager audioManager;
    private TileMapGBC tileMap;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        tileMap = GetNode<TileMapGBC>("/root/Level/TileMap");
    }

    public async void _on_Spot_body_entered(Node body)
    {
        if (!body.IsInGroup("Box")) return;
        foreach (Node item in GetTree().Root.GetChildren())
        {
            //GD.Print(item.Name + " : " + item.GetPath());
            if (item is Level) 
            {
                foreach (Node childnode in item.GetChildren())
                {
                    if (childnode is TileMapGBC)
                    {
                        //tileMap = childnode as TileMapGBC;
                        GD.Print(childnode);
                        GD.Print(tileMap);
                        break;
                    }
                }
            }
        }
        Boolean destroy = true;
        TileMapGBC.tiles tileIndex = 0;
        var tileCheck = tileMap.GetCellv((Position)/tileMap.CellSize);

        switch (tileCheck)
        {
            case (int)TileMapGBC.tiles.WATER:
                audioManager.PlaySFX(data.sfxTree.boxWaterSFX);
                tileIndex = TileMapGBC.tiles.BOX_BRIDGE_WATER;
            break;

            case (int)TileMapGBC.tiles.WATER_LEDGE:
                audioManager.PlaySFX(data.sfxTree.boxWaterSFX);
                tileIndex = TileMapGBC.tiles.BOX_BRIDGE_LEDGE;
            break;

            case (int)TileMapGBC.tiles.LEDGE:
                audioManager.PlaySFX(data.sfxTree.boxGroundHoleSFX);
                tileIndex = TileMapGBC.tiles.BOX_BRIDGE_LEDGE;
            break;

            case (int)TileMapGBC.tiles.SPOT_TOGGLE_UP:
                audioManager.PlaySFX(data.sfxTree.stepSwitchOn);
                destroy = false;
            break;
        }

        await ToSignal(body.GetNode<GridMoveTween>("GridMoveTween"),"tween_completed");
        if (tileIndex != 0) tileMap.SwapTile(body,this.Position,tileIndex);

        if (destroy) 
        {
            var box = body as Box;
            box.Destroy();
            destroy = false;
            this.CallDeferred("queue_free");
        }
        
        if (GetParentOrNull<ProgTracker>() != null) 
        {
            GetParent<ProgTracker>().checkProgress();
        }

    }
}