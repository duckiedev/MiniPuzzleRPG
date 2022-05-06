using Godot;
using System;

public class SpotToggle : Area2D
{
    private Data data;
    private Node2D targetNode;
    private TileMap tileMap;
    private AudioManager audioManager;

    public override void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        tileMap = GetParent().GetNode<TileMap>("TileMap");

        //targetNode = (Node2D)GetChild(1);

        for (int i = 0; i < GetChildCount(); i++)
        {
            var currentChild = GetChild(i);
            if (currentChild is Obstacle) {
                targetNode = (Node2D)currentChild;
                break;
            }
        }

    }
    public void _on_SpotToggle_body_entered(Node body)
    {
        if (body.IsClass("TileMap")) return;
        tileMap.SwapTile(this.Position,15);
        audioManager.PlaySFX(data.sfxTree.stepSwitchOn);
        targetNode.Visible = false;
        var col = targetNode.GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");
        col.SetDeferred("disabled",true);
    }
    public void _on_SpotToggle_body_exited(Node body)
    {
        if (body.IsClass("TileMap")) return;
        tileMap.SwapTile(this.Position,16);
        audioManager.PlaySFX(data.sfxTree.stepSwitchOff);
        targetNode.Visible = true;
        var col = targetNode.GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");
        col.SetDeferred("disabled",false);
    }
}
