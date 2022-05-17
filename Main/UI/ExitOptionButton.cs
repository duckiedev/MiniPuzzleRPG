using Godot;
using System;

public class ExitOptionButton : OptionButton
{
    public override void _on_OptionButton_pressed()
    {
        GD.Print("pause game from exitoptionbutton");
        GetNode<Data>("/root/Data").PauseGame();
        sceneChanger.ChangeScene("res://Main/Levels/TitleScreen.tscn");
    }
            
}
