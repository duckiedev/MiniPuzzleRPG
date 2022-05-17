using Godot;
using System;

public class GridMoveTween : Tween
{
    public KinematicBody2D parent;
    public override void _Ready()
    {
        parent = GetParent<KinematicBody2D>();
    }

    public void SetMoveTo(Vector2 position)
    {
        
    }
}
