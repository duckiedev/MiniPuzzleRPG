using Godot;
using System;

public class SceneChanger : CanvasLayer
{

    [Signal] delegate void SceneChanged();

    AnimationPlayer animationPlayer;
    ColorRect blackRect;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        blackRect = GetNode<ColorRect>("Control/BlackRect");
        blackRect.Visible = false;
    }

    public async void ChangeScene(String path, float delay = 0.5f)
    {
        await ToSignal(GetTree().CreateTimer(delay), "timeout");
        blackRect.Visible = true;
        animationPlayer.Play("fade");
        await ToSignal(animationPlayer, "animation_finished");
        GetTree().ChangeScene(path);
        animationPlayer.PlayBackwards("fade");
        await ToSignal(animationPlayer, "animation_finished");
        blackRect.Visible = false;
        EmitSignal("SceneChanged");
    }
}
