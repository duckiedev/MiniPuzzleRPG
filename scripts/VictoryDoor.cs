using Godot;
using System;

public class VictoryDoor : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private SoundFX soundPlayer;
    private AudioStreamPlayer musicPlayer;
    private SceneChanger sceneChanger;
    private bool playerFinished = false;
    public async override void _Ready()
    {
        await ToSignal(Owner, "ready");
        soundPlayer = GetTree().Root.GetNode<SoundFX>("SoundFX");
        musicPlayer = GetTree().Root.GetNode("musicPlayer") as Godot.AudioStreamPlayer;
        sceneChanger = GetTree().Root.GetNode<SceneChanger>("SceneChanger");
    }
    public void _on_Area2D_body_entered(Node body)
    {
        if (body.IsInGroup("Player")) {
            GetTree().Paused = true;
            musicPlayer.Stop();
            soundPlayer.SetSFX(soundPlayer.sndfxtree.victoryDoor);
            playerFinished = true;
        }
    }

    public void _on_soundFX_finished()
    {
        if (!playerFinished)
        {
            return; 
        } else {
            GD.Print("Test");
            sceneChanger.ChangeScene("res://Levels/Level2.tscn");
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
