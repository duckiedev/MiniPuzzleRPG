using Godot;
using System;

public class SceneChanger : CanvasLayer
{

    [Signal] delegate void SceneChanged();

    Data data;
    AnimationPlayer animationPlayer;
    AudioManager audioManager;

    ColorRect blackRect;
    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        audioManager = GetNode<AudioManager>("/root/AudioManager");

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        blackRect = GetNode<ColorRect>("Control/BlackRect");
        blackRect.Visible = false;
    }

    public async void ChangeScene(String path, Boolean level = false, float delay = 0.2f)
    {
        await ToSignal(GetTree().CreateTimer(delay), "timeout");
        audioManager.sfxDisabled = true;
        // Fade out music
        audioManager.FadeMusic("out","long");
        await ToSignal(audioManager.musicPlayer.Fader, "animation_finished");
        audioManager.musicPlayer.Stop();
        // Fade out screen
        blackRect.Visible = true;
        animationPlayer.Play("fade");
        await ToSignal(animationPlayer, "animation_finished");
        // Change scene
        if (level) path = $"res://Main/Levels/Level{path}.tscn";
        GetTree().ChangeScene(path);
        // Fade in screen
        animationPlayer.PlayBackwards("fade");
        await ToSignal(animationPlayer, "animation_finished");
        // Fade in music
        var newScene = GetNode<Level>("/root/Level");
        AudioStream sceneMusic = data.musicTree.Get(newScene.levelMusic) as AudioStream;
        audioManager.ResetMusicVol();
        audioManager.PlayMusic(sceneMusic);

        blackRect.Visible = false;

        EmitSignal("SceneChanged");
        audioManager.sfxDisabled = false;
    }
}