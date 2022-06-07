using Godot;
using System;

public class SpotToggle : Area2D
{
    private Data data;
    private Node targetNode;
    private TileMap tileMap;
    private AudioManager audioManager;

    public override void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        tileMap = GetParent().GetNode<TileMap>("TileMap");

        for (int i = 0; i < GetChildCount(); i++)
        {
            var currentChild = GetChild(i);
            if (currentChild is Obstacle) 
            {
                targetNode = currentChild;
                break;
            }
            if (currentChild is Action)
            {
                targetNode = currentChild;
            }
        }

    }
    public void _on_SpotToggle_body_entered(Node body)
    {
        if (body.IsClass("TileMap")) return;

        tileMap.SwapTile(body,Position,TileMap.tiles.SPOT_TOGGLE_DOWN);
        audioManager.PlaySFX(data.sfxTree.stepSwitchOn);

        if (targetNode is Obstacle)
        {
            targetNode.Set("visible",false);
            var col = targetNode.GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");
            col.SetDeferred("disabled",true);
        }
        if (targetNode is Action)
        {
            targetNode.CallDeferred("Run");
        }
    }
    public void _on_SpotToggle_body_exited(Node body)
    {
        if (body.IsClass("TileMap")) return;
        tileMap.SwapTile(body,this.Position,TileMap.tiles.SPOT_TOGGLE_UP);
        audioManager.PlaySFX(data.sfxTree.stepSwitchOff);
        if (targetNode is Obstacle)
        {
            targetNode.Set("visible", true);
            var col = targetNode.GetNode<CollisionShape2D>("StaticBody2D/CollisionShape2D");
            col.SetDeferred("disabled",false);
        }
    }
}
