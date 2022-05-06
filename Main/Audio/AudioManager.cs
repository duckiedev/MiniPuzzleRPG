using Godot;
using System;

public class AudioManager : Node
{
    public AudioStreamPlayer sfxPlayer;
    public MusicPlayer musicPlayer;
    [Export] public float musicVolume;
    public override void _Ready()
    {
        sfxPlayer = GetTree().Root.GetNode<AudioStreamPlayer>("SoundFXPlayer");
        musicPlayer = GetTree().Root.GetNode<MusicPlayer>("MusicPlayer");
    }

    public void PlaySFX(AudioStream sfx) {
        if (sfxPlayer.Playing) sfxPlayer.Stop();
        sfxPlayer.Stream = sfx;
        sfxPlayer.Play();
    }

    public void PlayMusic(AudioStream sfx) {
        if (musicPlayer.Playing) musicPlayer.Stop();
        musicPlayer.Stream = sfx;
        musicPlayer.Play();
    }

    public void FadeMusic(string type, string length)
    {
        if (type == "out" || type == "in" && length == "long" || length == "short")
        {
            musicPlayer.Fader.Play($"fade_{type}_{length}");
        }
        else
        {
            GD.Print($"fade type {type} doesn't exist.");
        }

    }
    public void ResetMusicVol()
    {
        musicPlayer.Fader.Play("RESET");
    }
}
