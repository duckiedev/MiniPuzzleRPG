using Godot;
using System;

public class BreakableTile : Area2D
{
    [Export(PropertyHint.Enum,"On Enter,On Exit")] private int breakTime;
    private Data data;
    private AudioManager audioManager;
    private TileMap tileMap;

    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        tileMap = GetNode("/root/Level/TileMap") as TileMap;
    }

    public void BreakTile(Node body)
    {
        if (body.IsInGroup("Player")) {
            audioManager.PlaySFX(data.sfxTree.boxWaterSFX);
            TileMap.tiles tileIndex = TileMap.tiles.WATER_LEDGE;
            tileMap.SwapTile(body,this.Position,tileIndex);
            Godot.Collections.Array list = GetChildren();
            for (int i = 0; i < list.Count; i++)
            {
                Node item = (Node)list[i];
                if (item.Name.StartsWith("TileTarget")) {
                    Vector2 itemPos = this.Position + (Vector2)item.Get("position");
                    TileMap.tiles itemTileIndex = (TileMap.tiles)item.Get("tileIndex");
                    tileMap.SwapTile(body,itemPos,itemTileIndex);
                }
            }
            if (GetParentOrNull<ProgTracker>() != null) {
                GetParent<ProgTracker>().checkProgress();
            }
            CallDeferred("queue_free");
        }
    }

    public void _on_BreakLeave()
    {

    }
    public void _on_BreakableTile_body_entered(Node body) {
        if (!body.IsInGroup("Player")) return;
        if (breakTime == 1) return;
        if (breakTime == 0) BreakTile(body);
    }

    public void _on_BreakableTile_body_exited(Node body) {
        if (!body.IsInGroup("Player")) return;
        if (breakTime == 0) return;
        if (breakTime == 1) BreakTile(body);
    }
}
