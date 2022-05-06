using Godot;
using System;

public class SaveSelect : Level
{
    public override void _Ready()
    {
        base._Ready();
        var saveSelect = GetNode<SaveOptionButton>("CanvasLayer/Saves/Save1");
        saveSelect.GrabFocus();
    }

}
