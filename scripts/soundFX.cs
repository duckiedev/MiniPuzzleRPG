using Godot;
using System;

public class SoundFX : AudioStreamPlayer
{

    public sndFXTree sndfxtree;

    public override void _Ready()
    {
        sndfxtree = ResourceLoader.Load<sndFXTree>("res://snd/sndFXTree.tres");
    }
    public void SetSFX(Resource sfx) {
        if (Playing) Stop();
        Stream = (AudioStream)sfx;
        Play();
    }
}
