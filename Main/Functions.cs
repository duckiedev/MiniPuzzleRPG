using Godot;
using System;

public class Functions : Node
{
    public static void LoadControlMenu()
    {
        var controlsPS = ResourceLoader.Load<PackedScene>("res://Main/UI/ControlsScreen.tscn");
        var controlsScreen = controlsPS.Instance<ControlsScreen>();
    }

}
