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

    public async void ChangeLevel(int level, float delay = 0.2f)
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
        GD.Print("currentLevel Before: " + data.currentLevel);
        if (level != 0) data.currentLevel+=1;
        GD.Print("currentLevel After: " + data.currentLevel);
        var path = $"res://Main/Levels/Level{data.levelArr[data.currentLevel]}.tscn";
        // Change scene
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

    public void ChangeScene(int level, float delay = 0.2f)
    {
        if (level != 0) {
            data.currentLevel+=1;
            data.nextLevel+=1;
        } 
        var path = $"res://Main/Levels/Level{data.levelArr[data.currentLevel]}.tscn";
        ChangeScene(path, delay);
    }
    
    public async void ChangeScene(String path, float delay = 0.2f)
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
