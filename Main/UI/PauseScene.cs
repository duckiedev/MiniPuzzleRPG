using Godot;
using System;

public class PauseScene : Node2D
{
    public enum PauseStates {MAIN,CONTROLS,ONLYCONTROLS,EXIT};
    public PauseStates pauseState = PauseStates.MAIN;
    Data data;
    Node2D mainScreen;
    OptionMenu mainScreenOptionsMenu;
    Node2D controlsScreen;
    public override void _Ready()
    {
        pauseState = PauseStates.MAIN;
        data = GetNode<Data>("/root/Data");
        mainScreen = GetNode<Node2D>("MainScreen");
        mainScreenOptionsMenu = mainScreen.GetNode<OptionMenu>("MainScreenMenu");
        controlsScreen = GetNode<Node2D>("ControlsScreen");
        var camera2D = GetNodeOrNull<Camera2D>("/root/Level/Camera2D");
        if (camera2D != null)
        {
            Position = camera2D.GetCameraScreenCenter().Round() - (GetViewportRect().Size/2);
        }
        else
        {
            Position = new Vector2(0,-1);
        }
    }

    public override void _Input(InputEvent @event)
    {
        
        if (@event.IsActionPressed("b"))
        {
            if (pauseState == PauseStates.MAIN)
            {
                data.UnpauseGame();
            }
            else if (pauseState == PauseStates.CONTROLS)
            {
                mainScreen.Visible = true;
                mainScreenOptionsMenu.optionSelected = false;
                controlsScreen.Visible = false;
                pauseState = PauseStates.MAIN;
            }
        }
    }
}
