using Godot;
using System;

public class Box : KinematicBody2D
{
    public StateMachine stateMachine;
    public enum boxState {
        IDLE,
        PUSH,
        DISABLED
    }
    public boxState state = boxState.IDLE;
    private Boolean colTileMap;
    private Boolean colSpot;
    public Boolean reset = false;
    private RayCast2D ray;
    private Vector2 originalPosition;
    private Vector2 vectorPos;
    public GridMoveTween tween;
    private Level level;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        stateMachine = GetNode<StateMachine>("StateMachine");
        ray = GetNode<RayCast2D>("RayCast2D");
        tween = GetNode<GridMoveTween>("GridMoveTween");

        originalPosition = Position;
        AddToGroup("Box",true);
    }
    public bool CheckCollision(Vector2 dir)
    {
        var objArray = new Godot.Collections.Array<Godot.Object>{};
        colTileMap = false;
        colSpot = false;

        vectorPos = dir * Data.gridSize;
        ray.CastTo = vectorPos;

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

        if (colTileMap == false && colSpot == false)
        {
            return true;
        }
        // if theres both a tilemap collision and a spot collision
        // ignore the spot, move anyway if the tile allows it
        if (colTileMap == true && colSpot == true) {
            TileMap tileMap = (TileMap)objArray[0];
            switch (tileMap.GetCellv((Position + vectorPos)/Data.gridSize))
            {
                case 2: // water
                case 3:  // water ledge
                case 13: // ground hole
                    return true;
                //break;
            }
        }

        if (colTileMap == true && colSpot == false) {
            TileMap tileMap = (TileMap)objArray[0];
            switch (tileMap.GetCellv((Position + vectorPos)/Data.gridSize))
            {
                case 2: // water
                case 3:  // water ledge
                case 13: // ground hole
                    reset = true;
                return true;
            }
        } 

        if (colTileMap == false && colSpot == true) 
        {
            return true;
        }
        return false;
    }
    public void ResetBox()
    {
        Position = originalPosition;
        reset = false;
    }

    public void Destroy()
    {
        GD.Print("desTROY!");
        var player = GetNode<Player>("/root/Player");
        player.stateMachine.TransitionTo("PlayerStates/Idle");
        CallDeferred("queue_free");
    }

}
