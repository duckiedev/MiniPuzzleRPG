using Godot;
using System;

public class ControlOptionButton : OptionButton
{
    PackedScene controlsScreenScene;
    ControlsScreen controlsScreen;

    public override void _Ready()
    {
        base._Ready();
        controlsScreenScene = ResourceLoader.Load<PackedScene>("res://Main/UI/ControlsScreen.tscn");
    }

    public override void _on_OptionButton_pressed()
    {
        GetNode<PauseScene>("/root/PauseScreen").PauseMode = PauseModeEnum.Stop;
        GetTree().Paused = true;
        controlsScreen = controlsScreenScene.Instance() as ControlsScreen;
        GetTree().Root.AddChild(controlsScreen);
    }

}
