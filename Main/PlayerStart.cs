using Godot;
using System;

public class PlayerStart : Node2D
{
    public override void _Ready()
    {
        Player playerNode = GetTree().Root.GetNode<Player>("Level/Player");
        playerNode.Position = Position;
    }

}
