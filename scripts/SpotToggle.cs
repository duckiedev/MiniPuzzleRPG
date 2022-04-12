using Godot;
using System;

public class SpotToggle : Area2D
{
    private Node2D targetNode;

    private soundFX soundPlayer;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            var currentChild = GetChild(i);
            if (currentChild.IsInGroup("Doors")) {
                targetNode = (Node2D)currentChild;
                break;
            }
        }
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
    }

    public void _on_SpotToggle_body_entered(Node body) {
        //GD.Print(body.Name + " " + body.GetClass());
        if (body.IsClass("TileMap")) return;
        TileMap.GetTileMap(this).SwapTile(this.Position,15);
        soundPlayer.SetSFX(soundPlayer.stepSwitchOn);
        targetNode.Visible = false;
        var col = targetNode.GetNode<StaticBody2D>("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D");
        col.SetDeferred("disabled",true);
    }
    public void _on_SpotToggle_body_exited(Node body) {
        //GD.Print(body.Name + " " + body.GetClass());
        if (body.IsClass("TileMap")) return;
        TileMap.GetTileMap(this).SwapTile(this.Position,16);
        soundPlayer.SetSFX(soundPlayer.stepSwitchOff);
        targetNode.Visible = true;
        var col = targetNode.GetNode<StaticBody2D>("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D");
        col.SetDeferred("disabled",false);
    }
}
