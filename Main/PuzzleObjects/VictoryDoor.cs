using Godot;
using System;

public class VictoryDoor : Node2D
{
    private Data data;

    private Level currentLevel;
    private AudioManager audioManager;
    private SceneChanger sceneChanger;
    private Area2D area2D;
    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        sceneChanger = GetNode<SceneChanger>("/root/SceneChanger");
        currentLevel = GetParent<Level>();
        area2D = GetChild<Area2D>(0);
        DelayedEnable(); // This is a workaround for the VictoryDoor sending off a signal when the tree restarts
    }


    public async void DelayedEnable()
    {
        await ToSignal(GetTree().CreateTimer(0.5f,false),"timeout");
        area2D.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }

    public void _on_Area2D_body_entered(Node body)
    {
        if (body.IsInGroup("Player")) {
            Player player = body as Player;
            player.stateMachine.TransitionTo("PlayerStates/Disabled");
            audioManager.PlayMusic(data.musicTree.victoryMusic);
            OnMusicDone();
        }
    }

    public async void OnMusicDone()
    {
        await ToSignal(audioManager.musicPlayer,"finished");
        data.SaveGame(data.currentSave,currentLevel.levelNext);
        sceneChanger.ChangeScene($"res://Main/Levels/Level{currentLevel.levelNext}.tscn");
    }
}
