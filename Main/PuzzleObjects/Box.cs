using Godot;
using System;

public class Box : KinematicBody2D
{
    private int gridSize = 16;
    private Boolean canMove;
    private Boolean colTileMap;
    private Boolean colSpot;
    private Boolean reset;
    private RayCast2D ray;
    private Vector2 originalPosition;
    private Godot.Collections.Array<Godot.Object> objArray;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ray = this.GetNode<RayCast2D>("RayCast2D");
        originalPosition = Position;
        objArray = new Godot.Collections.Array<Godot.Object>{};
    }

    public bool move(Vector2 dir)
    {

        colTileMap = false;
        colSpot = false;
        canMove = false;
        reset = false;

        Vector2 vectorPos = dir * gridSize;

        ray.CastTo = vectorPos;

        if (objArray.Count > 0) objArray.Clear();

        // Check for Body (TileMap)
        ray.CollideWithBodies = true;
        ray.CollideWithAreas = false;
        ray.ForceRaycastUpdate();
        if (ray.IsColliding()) {
            var obj = ray.GetCollider();
            objArray.Add(obj);
            Node objNode = (Node)obj;
            if (objNode.IsClass("TileMap")) colTileMap = true;
        }

        // Check for Spot (Area2D)
        ray.CollideWithBodies = false;
        ray.CollideWithAreas = true;
        ray.ForceRaycastUpdate();
        if (ray.IsColliding()) {
            var obj = ray.GetCollider();
            objArray.Add(obj);
            Node objNode = (Node)obj;
            if (objNode.IsClass("Area2D")) colSpot = true;
        }

        if (colTileMap && colSpot) {

            TileMap tileMap = (TileMap)objArray[0];
            switch (tileMap.GetCellv((Position + vectorPos)/gridSize))
            {
                case 2: // water
                case 3:  // water ledge
                case 13: // ground hole
                    canMove = true;
                break;
            }
        } else
        {
            if (colTileMap && colSpot == false) {
                TileMap tileMap = (TileMap)objArray[0];
                switch (tileMap.GetCellv((Position + vectorPos)/gridSize))
                {
                    case 2: // water
                    case 3:  // water ledge
                    case 13: // ground hole
                        canMove = true;
                        reset = true;
                    break;

                    default:
                        canMove = false;
                    break;
                }
            } else 
            {
                canMove = true;
                reset = false;
            }
        }

        if (canMove) {
            if (reset) {
                Position = originalPosition;
            } else {
                Position += vectorPos;
            }
            return true;
        } else {
            return false;
        }
    }

}
