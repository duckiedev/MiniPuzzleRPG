using Godot;
using System;

public class Player : KinematicBody2D
{
    private (string, Vector2)[] inputs = 
    {
        ("ui_right", Vector2.Right), 
        ("ui_left", Vector2.Left), 
        ("ui_up", Vector2.Up), 
        ("ui_down", Vector2.Down)
    };
    private float gridSize = 16;
    private soundFX soundPlayer;
    private RayCast2D ray;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //Position = Position.Snapped(Vector2.One * gridSize);
        //Position += Vector2.One * gridSize/2;
        ray = this.GetNode<RayCast2D>("RayCast2D");
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        foreach ((string dir, Vector2 vec) in inputs)
        {
            if (@event.IsActionPressed(dir)) {
                move(vec);
            }
        }
    }

    public void move(Vector2 dir){
        Boolean canMove = false;
        Vector2 vectorPos = dir * gridSize;
        ray.CastTo = vectorPos;
        ray.ForceRaycastUpdate();
        if (!ray.IsColliding()) {
            soundPlayer.SetSFX(soundPlayer.playerMoveSFX);
            canMove = true;
        } else {
            Node collider = (Node)ray.GetCollider();
            if (!collider.IsInGroup("Box")) {
                canMove = false;
            } else {
                Box boxCollider = (Box)collider;
                if (boxCollider.move(dir)) {
                    soundPlayer.SetSFX(soundPlayer.boxMoveSFX);
                    canMove = true;
                }
            }
            if (collider.IsInGroup("NoCollision")) {
                soundPlayer.SetSFX(soundPlayer.boxMoveSFX);
                canMove = true;
            }

        }
        if (canMove) {
            Position += vectorPos;
        }
    }
}
