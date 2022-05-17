using Godot;
using System;

public class RestartLevel : OptionLabel
{
    public override void Run()
    {
        GetNode<PauseScene>("/root/PauseScene").QueueFree();
        //GetNode<Player>("/root/Player").stateMachine.TransitionTo("PlayerStates/Disabled");
        GetNode<AudioManager>("/root/AudioManager").UnpauseMusic();
        GetTree().CallDeferred("reload_current_scene");
        GetTree().Paused = false;
    }
}
