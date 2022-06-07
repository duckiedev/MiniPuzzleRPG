using Godot;
using System;

public class LoadSaveGame : OptionLabel
{
    [Export] int saveFile = 0;
    Data data;
    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        Text = ParseSaveGameData();
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
        return $"#{saveFile} - Level {data.saveFileProgress[saveFile]}";
    }
    public override void Run()
    {
        if (Text != "ERROR")
        {
            if (Text == "New Game")
            {
                GD.Print("newgame");
                data.NewGame(saveFile);
            } 
            else 
            {
                GD.Print("loadgame");
                data.LoadGame(saveFile);
            }
                GetNode<SceneChanger>("/root/SceneChanger").ChangeScene(data.currentLevel);
                GetNode<AudioManager>("/root/AudioManager").PlaySFX(data.sfxTree.selectSFX);
        }
    }

}
