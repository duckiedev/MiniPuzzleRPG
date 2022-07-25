using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    private Player player;
    public Node2D target;
    public override void _Ready()
    {
        player = GetNode<Player>("/root/Player");
        target = player;
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position = target.Position;
    }

    public void ResetTarget()
    {
        target = player;
    }
}
