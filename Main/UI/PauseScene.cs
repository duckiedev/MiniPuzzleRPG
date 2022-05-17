using Godot;
using System;

public class PauseScene : Node2D
{
    public enum PauseStates {MAIN,CONTROLS,ONLYCONTROLS};
    public PauseStates pauseState = PauseStates.MAIN;
    Data data;

    Node2D mainScreen;
    Node2D controlsScreen;
    public override void _Ready()
    {
        pauseState = PauseStates.MAIN;
        data = GetNode<Data>("/root/Data");
        mainScreen = GetNode<Node2D>("MainScreen");
        controlsScreen = GetNode<Node2D>("ControlsScreen");
    }

    public override void _Input(InputEvent @event)
    {
        
        if (@event.IsActionPressed("ui_cancel"))
        {
            if (pauseState == PauseStates.MAIN)
            {
                GD.Print("unpausing game");
                data.UnpauseGame();
            }
            else if (pauseState == PauseStates.CONTROLS)
            {
                mainScreen.Visible = true;
                controlsScreen.Visible = false;
                pauseState = PauseStates.MAIN;
            }
        }
    }
}
