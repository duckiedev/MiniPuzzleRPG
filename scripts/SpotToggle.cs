using Godot;
using System;

public class SpotToggle : Area2D
{
    [Export] private String targetNodeName;
    [Export] private Boolean toggleSecond;
    [Export] private String targetNodeName2;
    private Node2D targetNode;
    private Node2D targetNode2;
    soundFX soundPlayer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        targetNode = GetParent().GetNode<Node2D>(targetNodeName);
        targetNode2 = GetParent().GetNode<Node2D>(targetNodeName2);
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
    }

    public void _on_SpotToggle_body_entered(Node body) {
        GD.Print(body.Name + " " + body.GetClass());
        if (body.IsClass("TileMap")) return;
        TileMap.GetTileMap(this).SwapTile(this.Position,15);
        soundPlayer.SetSFX(soundPlayer.stepSwitchOn);
        targetNode.Visible = false;
    }
    public void _on_SpotToggle_body_exited(Node body) {
        GD.Print(body.Name + " " + body.GetClass());
        if (body.IsClass("TileMap")) return;
        TileMap.GetTileMap(this).SwapTile(this.Position,16);
        soundPlayer.SetSFX(soundPlayer.stepSwitchOff);
        targetNode.Visible = true;
    }
}
