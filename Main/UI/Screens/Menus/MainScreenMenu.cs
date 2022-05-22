using Godot;
using System;

public class MainScreenMenu : OptionMenu
{
    PauseScene pauseScene;
 
    public override void _Ready()
    {
        base._Ready();
        pauseScene = GetNode<PauseScene>("/root/PauseScene");
    }

    public override void _Input(InputEvent @event)
    {
        if (pauseScene.pauseState == PauseScene.PauseStates.MAIN)
        {
            base._Input(@event);
        }
    }
}
