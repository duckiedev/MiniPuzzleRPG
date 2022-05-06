using Godot;
using System;

public class Level : Node2D
{
    [Export] public int levelWorld;
    [Export] public int levelNumber;
    [Export] public string levelNext = "0-0";
    [Export] public string levelMusic = "levelMusic";
}
