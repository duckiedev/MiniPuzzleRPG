using Godot;
using System;

public class TitleScreen : Level
{
    Data data;
    SceneChanger sceneChanger;
    AudioManager audioManager;

    AnimationPlayer animationPlayer;
    VBoxContainer options;

    Button optionNew;
    Button optionLoad;
    Button optionQuit;

    public override void _Ready()
    {
        base._Ready();
        data = GetTree().Root.GetNode<Data>("Data");

        sceneChanger = GetNode<SceneChanger>("/root/SceneChanger");

        audioManager = GetNode<AudioManager>("/root/AudioManager");
        audioManager.PlayMusic((AudioStream)data.musicTree.Get(levelMusic));
        /*
        options = GetNode<VBoxContainer>("CanvasLayer/Options");
        optionNew = options.GetNode<Button>("New");
        optionLoad = options.GetNode<Button>("Load");
        optionQuit = options.GetNode<Button>("Quit");

        optionNew.GrabFocus();
        */
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("start_flash");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            audioManager.PlaySFX(data.sfxTree.selectSFX);
            sceneChanger.ChangeScene("res://Main/Levels/SaveSelect.tscn");
        }
    }

}
