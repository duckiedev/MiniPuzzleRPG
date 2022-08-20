using Godot;
using System;

public class NodeGBC : KinematicBody2D
{
    public Data data;
    public StateMachine stateMachine;
    public AnimationTree animationTree;
    public AnimationNodeStateMachinePlayback animStateMachine;
    public int zCurrent = 0;
    public Boolean fall;
    public Boolean reset;
    public Vector2 vectorPos;
    public GridMoveTween tween;

    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        tween = GetNode<GridMoveTween>("GridMoveTween");
    }

    public int CheckTile(Type tileMapType, Vector2 pos)
    {
        if (tileMapType == typeof(HeightMap))
        {
            HeightMap map = GetNode<HeightMap>("/root/Level/" + tileMapType.ToString());
            int tile = map.GetCellv(pos/Data.gridSize);
            // Subtract 1 to match the height number
            tile -= 1;
            if (tile != -2) 
            {
                return tile;
            } 
            else
            { 
                return -1;
            }
        }
        else
        {
            TileMapGBC map = GetNode<TileMapGBC>("/root/Level/TileMap");
            int tile = map.GetCellv(pos/Data.gridSize);
            return tile;
        }
    }

    #region Movement
    public void SetNextPosition(Vector2 nextPos)
    {
        vectorPos = nextPos;
    }
    
    public Boolean CanMove(Vector2 dir)
    {
        var tilePos = dir * Data.gridSize;

        SetNextPosition(tilePos);

        var colliders = CheckCollision(tilePos);
        
        GD.Print(colliders);

        if (colliders.Contains("StaticBod") && !colliders.Contains("DropTrigger")) return false;

        if (colliders.Contains("Box"))
        {
            var box = (Box)colliders["Box"];
            if (!box.tween.IsActive())
            {
                if (box.CanMove(dir))
                {
                    box.stateMachine.TransitionTo("BoxStates/BoxPushed");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        if (colliders.Contains("DropTrigger") && colliders.Contains("TileMap"))
        {
            var fallPosition = GetFallPosition(tilePos,zCurrent);
            GD.Print("fallPos: " + fallPosition);
            if (CanFall(fallPosition))
            {
                fall = true;
                SetNextPosition(fallPosition);
                return true;
            }
            else
            {
                return false;
            }
        }

        if (!colliders.Contains("TileMap") && !colliders.Contains("Spot"))
        {
            return true;
        }

        if (!colliders.Contains("TileMap") && colliders.Contains("Spot")) 
        {
            return true;
        }

        var tileCheck = CheckTile(typeof(TileMapGBC),(Position + vectorPos));
        // if theres both a tilemap collision and a spot collision
        // ignore the spot, move anyway if the tile allows it
        if (this is Box)
        {
            if (colliders.Contains("TileMap") && colliders.Contains("Spot")) 
            {
                switch (tileCheck)
                {
                    case (int)TileMapGBC.tiles.WATER:
                    case (int)TileMapGBC.tiles.WATER_LEDGE:
                    case (int)TileMapGBC.tiles.LEDGE:
                        return true;
                    //break;
                }
            }

            if (colliders.Contains("TileMap") && !colliders.Contains("Spot")) 
            {
                switch (tileCheck)
                {
                    case (int)TileMapGBC.tiles.WATER:
                    case (int)TileMapGBC.tiles.WATER_LEDGE:
                    case (int)TileMapGBC.tiles.LEDGE:
                        reset = true;
                    return true;
                }
            }
        }
        return false;            
    }

    public Godot.Collections.Dictionary CheckCollision(Vector2 pos) 
    {
        var checkPos = (Position + new Vector2(8,8) + pos);
        var spaceState = GetWorld2d().DirectSpaceState;
        var intersects = spaceState.IntersectPoint(checkPos);
        if (this is Box) GD.Print("---------BOX--------");
        GD.Print("Checking collision at : " + (checkPos));
        Godot.Collections.Dictionary returns = new Godot.Collections.Dictionary();

        GD.Print(intersects);

        foreach (Godot.Collections.Dictionary item in intersects)
        {
            Godot.Node collider = (Godot.Node)item["collider"];
            if (collider is TileMapGBC && !returns.Contains("TileMap"))         returns.Add("TileMap",(TileMapGBC)collider);
            if (collider is Box && !returns.Contains("Box"))                    returns.Add("Box",(Box)collider);
            if (collider is StaticBody2D && !returns.Contains("StaticBod"))     returns.Add("StaticBod",(StaticBody2D)collider);
            //if (collider is Area2D && !returns.Contains("Area2D"))              returns.Add("Area2D",(Area2D)collider);
            if (collider is DropTrigger && !returns.Contains("DropTrigger"))    returns.Add("DropTrigger",(DropTrigger)collider);
            if (collider is Spot && !returns.Contains("Spot"))                  returns.Add("Spot",(Spot)collider);
        }
        return returns;
    }
    #endregion

    #region Falling
    public Boolean CanFall(Vector2 position)
    {
        var colliders = CheckFallCollision(position);

        foreach (Godot.Collections.Dictionary item in colliders)
        {
            if (item["collider"] is StaticBody2D) {
                if (this is Box) return true;
                return false;
            }
            if (item["collider"] is Box) return false;
        }
        return true;
    }
    public Vector2 GetFallPosition(Vector2 dir, int z)
    {
        for (int i = 1; i < 128; i++)
        {
            var checkTile = CheckTile(typeof(HeightMap),Position+(dir*i));
            if (checkTile == (z-1))
                {
                    return dir*i;
                }
            }
        return Vector2.Zero;
    }
    public Godot.Collections.Array CheckFallCollision(Vector2 pos)
    {
        var spaceState = GetWorld2d().DirectSpaceState;
        var result = spaceState.IntersectPoint(Position+new Vector2(8,8)+pos);
        return result;
    }
    #endregion
}
