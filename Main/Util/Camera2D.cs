using Godot;
using System;

public class Camera2D : Godot.Camera2D
{
    private Player player;
    public override void _Ready()
    {
        player = GetNode<Player>("/root/Player");
    }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Position = player.Position;
    }
}
