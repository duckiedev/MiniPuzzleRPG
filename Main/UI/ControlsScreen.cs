using Godot;
using System;

public class ControlsScreen : Node2D
{
    PauseScene pauseScreen;
    public override void _Ready()
    {
        pauseScreen = GetNode<PauseScene>("/root/PauseScreen");
    }
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
           pauseScreen.PauseMode = PauseModeEnum.Process;
           QueueFree();
        }
    }
}
