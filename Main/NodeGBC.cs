using Godot;
using System;

public class NodeGBC : KinematicBody2D
{
    public Data data;
    public StateMachine stateMachine;
    public RayCast2D ray;
    public HeightMap heightMap;
    public int zCurrent = 0;
    public Boolean fall;
    public Boolean reset;
    public Vector2 vectorPos;

    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
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
            TileMap map = GetNode<TileMap>("/root/Level/" + tileMapType.ToString());
            int tile = map.GetCellv(pos/Data.gridSize);
            return tile;
        }
    }

    public Boolean CheckCollision(Vector2 dir) {
        TileMap colTileMap = null;
        Spot colSpot = null;
        Area2D colDropTrigger = null;
        Box colBox = null;
        StaticBody2D colStaticBod = null;

        GD.Print("checking for " + Name);
        vectorPos = dir * Data.gridSize;

        ray.CastTo = vectorPos;

        // Check for Body (TileMap, Box)
        ray.CollideWithBodies = true;
        ray.CollideWithAreas = false;
        ray.ForceRaycastUpdate();
        if (ray.IsColliding()) 
        {
            var obj = ray.GetCollider();
            Node objNode = (Node)obj;
            if (objNode.IsClass("TileMap")) colTileMap = (TileMap)objNode;
            if (objNode.IsInGroup("Box")) colBox = (Box)objNode;
            if (objNode.IsClass("StaticBody2D")) colStaticBod = (StaticBody2D)objNode;
        }
        // Check for Spot (Area2Ds)
        ray.CollideWithBodies = false;
        ray.CollideWithAreas = true;
        ray.ForceRaycastUpdate();
        if (ray.IsColliding()) 
        {
            var obj = ray.GetCollider();
            Node objNode = (Node)obj;
            GD.Print(objNode.Name);
            if (objNode.Name.BeginsWith("DropTrigger")) colDropTrigger = (Area2D)objNode;
            if (objNode.IsClass("Spot")) colSpot = (Spot)objNode;
        }

        GD.Print(Name + " colTileMap : " + colTileMap);
        GD.Print(Name + " colBox : " + colBox);
        GD.Print(Name + " colStaticBod : " + colStaticBod);
        GD.Print(Name + " colDropTrigger : " + colDropTrigger);
        GD.Print(Name + " colSpot : " + colSpot);

        if (colStaticBod != null)
        {
            return false;
        }
        
        if (colBox != null)
        {
            GD.Print("BOX!");
            if (!colBox.tween.IsActive())
            {
                if (colBox.CheckCollision(dir))
                {
                    var args = new Godot.Collections.Dictionary();
                    args.Add("vectorPos",vectorPos);
                    colBox.stateMachine.TransitionTo("BoxStates/BoxPushed",args);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        if (colDropTrigger != null && colTileMap != null)
        {
            var results = CheckFallPlace(Position,vectorPos,zCurrent);
            if (IsInGroup("Player"))
            {
                GD.Print("checking fall for player");
                if (results.Contains("obj"))
                {
                    GD.Print("cant fall because of " + results["obj"].ToString());
                    return false;
                }
                else
                {
                    GD.Print("can fall");
                    vectorPos = (Vector2)results["pos"];
                    fall = true;
                    return true;
                }
            }
            if (IsInGroup("Box"))
            {
                GD.Print("Checking fall place for box");
                if (!results.Contains("obj"))
                {
                    GD.Print("boop");
                    fall = true;
                    vectorPos = (Vector2)results["pos"];
                    stateMachine.TransitionTo("BoxStates/BoxPushed");
                    return true;
                }
            }
        }

        if (colTileMap == null && colSpot == null)
        {
            return true;
        }

        if (colTileMap == null && colSpot != null) 
        {
            return true;
        }

        GD.Print(Name + " checking pos : " + (Position + vectorPos));
        var tileCheck = CheckTile(typeof(TileMap),(Position + vectorPos));
        GD.Print(tileCheck);
        // if theres both a tilemap collision and a spot collision
        // ignore the spot, move anyway if the tile allows it
        if (colTileMap != null && colSpot != null) 
        {
            switch (tileCheck)
            {
                case (int)TileMap.tiles.WATER:
                case (int)TileMap.tiles.WATER_LEDGE:
                case (int)TileMap.tiles.LEDGE:
                    return true;
                //break;
            }
        }

        if (colTileMap != null && colSpot == null) 
        {
            switch (tileCheck)
            {
                case (int)TileMap.tiles.WATER:
                case (int)TileMap.tiles.WATER_LEDGE:
                case (int)TileMap.tiles.LEDGE:
                    reset = true;
                return true;
            }
        } 
        return false;            
    }

    public Godot.Collections.Dictionary CheckFallPlace(Vector2 pos, Vector2 dir, int z)
    {
        var returns = new Godot.Collections.Dictionary();
        for (int i = 1; i < 128; i++)
        {
           if (CheckTile(typeof(HeightMap),pos+(dir*i)) == (z-1))
            {
                returns.Add("pos",dir*i);
                RayCast2D rayCast = new RayCast2D();
                rayCast.CastTo = dir*i;
                AddChild(rayCast);
                rayCast.ForceRaycastUpdate();

                if (rayCast.IsColliding())
                {
                    var collider = (Node)rayCast.GetCollider();
                    if (!collider.IsClass("TileMap")) returns.Add("obj",(Node)rayCast.GetCollider());
                }
                rayCast.QueueFree();
                break;
            }
        }
        return returns;
    }
}
