using Godot;
using System;

public class sndFXTree : Resource
{
    [Export] public AudioStream playerMoveSFX;
    [Export] public AudioStream boxMoveSFX;
    [Export] public AudioStream boxWaterSFX;
    [Export] public AudioStream boxGroundHoleSFX;
    [Export] public AudioStream stepSwitchOn;
    [Export] public AudioStream stepSwitchOff;
    [Export] public AudioStream victoryDoor;
}