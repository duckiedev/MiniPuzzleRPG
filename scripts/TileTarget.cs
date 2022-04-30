using Godot;
using System;

public class TileTarget : Area2D
{
    [Export(PropertyHint.Range)] public int tileIndex;
}
