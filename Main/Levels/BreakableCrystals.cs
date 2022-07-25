using Godot;
using System;

public class BreakableCrystals : Node2D
{
    [Export] NodePath mainSwitchPath;
    public MapObject mainSwitch;
    public override void _Ready()
    {
        mainSwitch = GetNode<MapObject>(mainSwitchPath);
    }
}
