using Godot;
using System;

public class SfxTree : Resource
{
    [Export] public AudioStream selectSFX;
    [Export] public AudioStream playerMoveSFX;
    [Export] public AudioStream boxMoveSFX;
    [Export] public AudioStream boxWaterSFX;
    [Export] public AudioStream boxGroundHoleSFX;
    [Export] public AudioStream stepSwitchOn;
    [Export] public AudioStream stepSwitchOff;
    [Export] public AudioStream boxWaterFloat;
    [Export] public AudioStream dialogBlip01;
    [Export] public AudioStream dialogBlipDone;
    [Export] public AudioStream fall;
    [Export] public AudioStream crystalBreak;
}