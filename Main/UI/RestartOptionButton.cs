using Godot;
using System;

public class RestartOptionButton : OptionButton
{

    public override void _Ready()
    {
        base._Ready();
    }
    public override void _on_OptionButton_pressed()
    {
        GetNode<Data>("/root/Data").PauseGame();
        GetTree().CallDeferred("reload_current_scene");
    }
}
