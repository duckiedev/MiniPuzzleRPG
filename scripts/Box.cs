using Godot;
using System;

public class Box : KinematicBody2D
{
    private int gridSize = 16;
    private Boolean canMove;
    //private Vector2 snapVec = new Vector2(16, 16);
    private RayCast2D ray;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ray = this.GetNode<RayCast2D>("RayCast2D");
        //Position = Position.Snapped(snapVec);
    }

    public bool move(Vector2 dir)
    {
        canMove = false;
        Vector2 vectorPos = dir * gridSize;
        ray.CastTo = vectorPos;
        ray.ForceRaycastUpdate();
        if (ray.IsColliding())
        {
            Node collider = (Node)ray.GetCollider();
            GD.Print("Collided with " + collider.Name);
            switch (collider.Name)
            {
                case "TileMap":
                    TileMap tilemap = (TileMap)collider;
                    switch (tilemap.GetCellv((Position + vectorPos)/gridSize))
                    {
                        case 2: // water
                        case 3:  // water ledge
                        case 13: // ground hole

                            canMove = true;
                        break;

                        default:

                        break;
                    }
                    break;
                case "Spot":
                    canMove = true;
                break;
                default:

                    break;
            }
        }
        else
        {
            canMove = true;
            
        }

        if (!canMove)
            return false;
        GD.Print("Move");
        Position += vectorPos;
        return true;
    }
}
