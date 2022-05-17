using Godot;
using System;

public class Controls : OptionLabel
{
    public override void Run()
    {
        GetNode<PauseScene>("/root/PauseScene").pauseState = PauseScene.PauseStates.CONTROLS;
        GetNode<MainScreen>("/root/PauseScene/MainScreen").Visible = false;
        GetNode<Node2D>("/root/PauseScene/ControlsScreen").Visible = true;
    }
}
