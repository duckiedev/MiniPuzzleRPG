using Godot;
using System;

public class SpotToggle : Area2D
{
    private Node2D targetNode;
    private TileMap tileMap;

    private SoundFX soundPlayer;
    // Called when the node enters the scene tree for the first time.
    public override async void _Ready()
    {
        for (int i = 0; i < GetChildCount(); i++)
        {
            var currentChild = GetChild(i);
            if (currentChild.IsInGroup("Doors")) {
                targetNode = (Node2D)currentChild;
                break;
            }
        }
        tileMap = GetParent().GetNode<TileMap>("TileMap");

        await ToSignal(Owner, "ready");
        soundPlayer = GetTree().Root.GetNode<SoundFX>("SoundFX");
    }

    public void _on_SpotToggle_body_entered(Node body) {
        //GD.Print(body.Name + " " + body.GetClass());
        if (body.IsClass("TileMap")) return;
        tileMap.SwapTile(this.Position,15);
        soundPlayer.SetSFX(soundPlayer.sndfxtree.stepSwitchOn);
        targetNode.Visible = false;
        var col = targetNode.GetNode<StaticBody2D>("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D");
        col.SetDeferred("disabled",true);
    }
    public void _on_SpotToggle_body_exited(Node body) {
        //GD.Print(body.Name + " " + body.GetClass());
        if (body.IsClass("TileMap")) return;
        tileMap.SwapTile(this.Position,16);
        soundPlayer.SetSFX(soundPlayer.sndfxtree.stepSwitchOff);
        targetNode.Visible = true;
        var col = targetNode.GetNode<StaticBody2D>("StaticBody2D").GetNode<CollisionShape2D>("CollisionShape2D");
        col.SetDeferred("disabled",false);
    }
}
