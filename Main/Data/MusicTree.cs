using Godot;
using System;

public class MusicTree : Resource
{
    [Export] public AudioStream titleMusic;
    [Export] public AudioStream saveSelectMusic;
    [Export] public AudioStream levelMusic;
    [Export] public AudioStream victoryMusic;
}
