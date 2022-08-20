using Godot;
using System;

public class BoxState : State
{
    public Data data;
    public Box box;
    public CollisionShape2D boxCollision;
    public AudioManager audioManager;
    public RayCast2D ray;
    public GridMoveTween tween;
    public Player player;

    public override void _Ready()
    {
        base._Ready();
        data = GetTree().Root.GetNode<Data>("Data");
        box = Owner as Box;
        boxCollision = box.GetNode<CollisionShape2D>("CollisionShape2D");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        tween = box.GetNode<GridMoveTween>("GridMoveTween");
        player = GetTree().Root.GetNode<Player>("Player");
    }
}
