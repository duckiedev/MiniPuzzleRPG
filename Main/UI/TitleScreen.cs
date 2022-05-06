using Godot;
using System;

public class TitleScreen : Node2D
{
    [Export] String music;
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
        data = GetTree().Root.GetNode<Data>("Data");

        sceneChanger = GetTree().Root.GetNode<SceneChanger>("SceneChanger");

        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        audioManager.PlayMusic((AudioStream)data.musicTree.Get(music));
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
            sceneChanger.ChangeScene("res://Main/UI/SaveSelect.tscn");
        }
    }

}
