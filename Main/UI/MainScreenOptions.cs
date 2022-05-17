using Godot;
using System;

public class MainScreenOptions : VBoxContainer
{
    public int options;
    public int selectedOption = 0;
    public override void _Ready()
    {
        options = GetChildCount();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_up"))
        {
            selectedOption = Mathf.Wrap(selectedOption +=1,0,options);
        }
        else if (@event.IsActionPressed("ui_down"))
        {
            selectedOption = Mathf.Wrap(selectedOption -=1,0,options);
        }
    }

}
