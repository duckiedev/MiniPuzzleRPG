using Godot;
using System;

public class Spot : Area2D
{
    [Export(PropertyHint.Enum,"Water Bridge,Water Bridge Ledge,Ground Hole")] private int spotType;
    [Export] private String progTracker;

    soundFX soundPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
    }

    public void _on_Spot_body_entered(Node body) {
        if (body.IsClass("TileMap")) return;
        Boolean destroy = true;
        GD.Print(body.Name + " go Bongle  with " + this.Name);
        switch (spotType){
            case 0:
                if (body.IsInGroup("Box")) {
                    soundPlayer.SetSFX(soundPlayer.boxWaterSFX);
                    TileMap.GetTileMap(this).SwapTile(this.Position,14);
                }
            break;
            case 1:
                if (body.IsInGroup("Box")) {
                    soundPlayer.SetSFX(soundPlayer.boxWaterSFX);
                    TileMap.GetTileMap(this).SwapTile(this.Position,12);
                }
            break;
            case 2:
                if (body.IsInGroup("Box")) {
                    soundPlayer.SetSFX(soundPlayer.boxGroundHoleSFX);
                    TileMap.GetTileMap(this).SwapTile(this.Position,12);
                }
            break;
            case 3:
                if (body.IsInGroup("Player")) {
                    soundPlayer.SetSFX(soundPlayer.boxWaterSFX);
                    TileMap.GetTileMap(this).SwapTile(this.Position,12);
                }
            break;
        }
        if (destroy) {
            body.QueueFree();
            this.QueueFree();
        }
        Node world = this.GetParent();
        world.GetNode<BoxSpawn>("BoxSpawn").Spawn();
        world.GetNode<ProgTracker>(progTracker).checkProgress();
        //GD.Print(world.GetNode<Win>("Win").winCurrent.ToString());
    }
}