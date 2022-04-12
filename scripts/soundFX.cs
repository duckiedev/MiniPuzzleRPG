using Godot;
using System;

public class soundFX : AudioStreamPlayer
{
    [Export] public Resource playerMoveSFX;
    [Export] public Resource boxMoveSFX;
    [Export] public Resource boxWaterSFX;
    [Export] public Resource boxGroundHoleSFX;
    [Export] public Resource stepSwitchOn;
    [Export] public Resource stepSwitchOff;
    [Export] public Resource victoryDoor;

    // Called when the node enters the scene tree for the first time.
    public void SetSFX(Resource sfx) {
        if (Playing) Stop();
        Stream = (AudioStream)sfx;
        Play();
    }
}
