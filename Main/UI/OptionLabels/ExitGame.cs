using Godot;
using System;

public class ExitGame : OptionLabel
{
    public override void Run()
    {
        var pauseScene = GetNode<PauseScene>("/root/PauseScene");
        pauseScene.QueueFree();
        pauseScene.pauseState = PauseScene.PauseStates.EXIT;
        GetNode<Data>("/root/Data").PauseGame();
        GetNode<SceneChanger>("/root/SceneChanger").ChangeScene("res://Main/Levels/TitleScreen.tscn");
    }
}
