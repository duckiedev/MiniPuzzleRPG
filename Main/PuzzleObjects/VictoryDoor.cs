using Godot;
using System;

public class VictoryDoor : Node2D
{
    private Data data;

    private Level currentLevel;
    private AudioManager audioManager;
    private SceneChanger sceneChanger;
    private bool playerFinished = false;
    public async override void _Ready()
    {
        await ToSignal(Owner, "ready");
        data = GetTree().Root.GetNode<Data>("Data");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        sceneChanger = GetTree().Root.GetNode<SceneChanger>("SceneChanger");
        currentLevel = GetTree().Root.GetNode<Level>("Level");
    }
    public void _on_Area2D_body_entered(Node body)
    {
        if (body.IsInGroup("Player")) {
            //GetTree().Paused = true;
            audioManager.PlayMusic(data.musicTree.victoryMusic);
            playerFinished = true;
            OnMusicDone();
        }
    }

    public async void OnMusicDone()
    {
        await ToSignal(audioManager.musicPlayer,"finished");
        data.SaveGame(data.currentSave,currentLevel.levelNext);
        sceneChanger.ChangeScene($"res://Main/Levels/Level{currentLevel.levelNext}.tscn");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
