using Godot;
using System;

public class AudioManager : Node
{
    public AudioStreamPlayer sfxPlayer;
    public MusicPlayer musicPlayer;
    public Boolean sfxDisabled;

    public float musicPauseSpot;
    public AudioStream musicPauseMusic;
    
    public override void _Ready()
    {
        sfxPlayer = GetTree().Root.GetNode<AudioStreamPlayer>("SoundFXPlayer");
        musicPlayer = GetTree().Root.GetNode<MusicPlayer>("MusicPlayer");
    }

    public void PlaySFX(AudioStream sfx)
    {
        if (!sfxDisabled)
        {
            if (sfxPlayer.Playing) sfxPlayer.Stop();
            sfxPlayer.Stream = sfx;
            sfxPlayer.Play();
        }
    }

    public void PlayMusic(AudioStream sfx, float fromTime = 0.0f)
    {
        if (musicPlayer.Playing) musicPlayer.Stop();
        musicPlayer.Stream = sfx;
        musicPlayer.Play(fromTime);
    }

    public void PauseMusic()
    {
        musicPauseMusic = musicPlayer.Stream;
        musicPauseSpot = musicPlayer.GetPlaybackPosition();
        musicPlayer.Stop();
    }

    public void UnpauseMusic()
    {
        PlayMusic(musicPauseMusic,musicPauseSpot);
        musicPauseMusic = null;
        musicPauseSpot = 0.0f;
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
