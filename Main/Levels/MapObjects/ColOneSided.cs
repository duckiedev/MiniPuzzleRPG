using Godot;
using System;

public class ColOneSided : Node2D
{

    [Export(PropertyHint.Enum,"Up,Down,Left,Right")] public String fromSide;

    public override void _Ready()
    {
        
    }

}
