using Godot;
using System;

public class TileTarget : Node2D
{
    [Export(PropertyHint.Range)] public TileMapGBC.tiles tileIndex;
}
