using Godot;
using System;

public class MusicPlayer : AudioStreamPlayer
{
    public AnimationPlayer Fader;
    public override void _Ready()
    {
        Fader = GetNode<AnimationPlayer>("AnimationPlayer");
    }

}
