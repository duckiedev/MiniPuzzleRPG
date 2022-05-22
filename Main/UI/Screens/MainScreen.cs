using Godot;
using System;

public class MainScreen : Node2D
{
    public override void _Ready()
    {
        var data = GetNode<Data>("/root/Data");
        GetNode<Label>("LevelLabel").Text = $"Level {data.currentLevel}";
    }


}
