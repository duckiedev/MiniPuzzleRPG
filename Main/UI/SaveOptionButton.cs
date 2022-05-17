using Godot;
using System;


public class SaveOptionButton : OptionButton
{
    [Export] int saveFile = 0;
    Label numberLabel;

    public override void _Ready()
    {
        base._Ready();
        numberLabel = GetNode<Label>("NumberLabel");

        numberLabel.Text = (saveFile+1).ToString();
        mainLabel.Text = ParseSaveGameData();
    }

    public string ParseSaveGameData()
    {
        if (data.saveFileProgress[saveFile] == "ERROR")
        {
            return "ERROR";
        }
        else if (data.saveFileProgress[saveFile] == "")
        {
            return "New Game";
        }
        return $"Level {data.saveFileProgress[saveFile]}";
    }
/*
    public override void _on_OptionButton_focus_entered()
    {

    }

    public override void _on_OptionButton_focus_exited()
    {

    }
*/
    public override void _on_OptionButton_pressed()
    {
        if (mainLabel.Text == "New Game")
        {
            data.NewGame(saveFile);
            sceneChanger.ChangeScene(data.currentLevel,true);
            audioManager.PlaySFX(data.sfxTree.selectSFX);
        }
        if (mainLabel.Text != "ERROR")
        {
            data.LoadGame(saveFile);
            sceneChanger.ChangeScene(data.currentLevel,true);
            audioManager.PlaySFX(data.sfxTree.selectSFX);
        }
    }
}
