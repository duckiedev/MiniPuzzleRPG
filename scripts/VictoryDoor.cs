using Godot;
using System;

public class VictoryDoor : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private soundFX soundPlayer;
    private AudioStreamPlayer musicPlayer;
    private bool playerFinished = false;
    public override void _Ready()
    {
        soundPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
        musicPlayer = GetTree().Root.GetNode("World").GetNode("soundFX") as soundFX;
    }
    public void _on_Area2D_body_entered(Node body)
    {
        if (body.IsInGroup("Player")) {
            GetTree().Paused = true;
            musicPlayer.Stop();
            soundPlayer.SetSFX(soundPlayer.victoryDoor);
            playerFinished = true;
        }
    }

    public void _on_soundFX_finished()
    {
        if (!playerFinished)
        {
            return; 
        } else {
            GetTree().ReloadCurrentScene();
            GetTree().Paused = false;
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
