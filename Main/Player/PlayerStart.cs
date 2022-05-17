using Godot;
using System;

public class PlayerStart : Node2D
{
    public async override void _Ready()
    {
        //PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://Main/Player/Player.tscn");
        Player playerNode = GetNode<Player>("/root/Player");
        playerNode.Position = GlobalPosition;
        await ToSignal(GetNode<Level>("/root/Level"),"ready");
        GetTree().Root.MoveChild(playerNode,GetTree().Root.GetChildCount());
    }

}
