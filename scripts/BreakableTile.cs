using Godot;
using System;

public class BreakableTile : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.Enum,"On Enter,On Exit")] private int breakTime;
    // Called when the node enters the scene tree for the first time.
    private soundFX soundPlayer;
    private TileMap tileMap;

    public override void _Ready()
    {
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
        tileMap = GetTree().Root.GetNode("World").GetNode("Level").GetNode<TileMap>("TileMap");
    }

    public void _on_BreakableTile_body_entered(Node body) {
        if (breakTime == 1) return;
        //if (breakTime == 0) {}

    }

    public void _on_BreakableTile_body_exited(Node body) {
        if (breakTime == 0) return;
        if (breakTime == 1) {
            if (body.IsInGroup("Player")) {
                soundPlayer.SetSFX(soundPlayer.boxWaterSFX);
                int tileIndex = ((int)TileMap.tiles.WATER_LEDGE);
                tileMap.SwapTile(this.Position,tileIndex);
                Godot.Collections.Array list = GetChildren();
                for (int i = 0; i < list.Count; i++)
                {
                    Node item = (Node)list[i];
                    if (item.Name.StartsWith("TileTarget")) {
                        //GD.Print(item.Get("name"));
                        GD.Print(item.Get("position").ToString());
                        GD.Print(item.Get("tileIndex"));
                        Vector2 itemPos = this.Position + (Vector2)item.Get("position");
                        int itemTileIndex = (int)item.Get("tileIndex");
                        tileMap.SwapTile(itemPos,itemTileIndex);
                    }

                }
                if (GetParentOrNull<ProgTracker>() != null) {
                    GetParent<ProgTracker>().checkProgress();
                }
                QueueFree();
            }
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
