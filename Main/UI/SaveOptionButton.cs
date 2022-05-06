using Godot;
using System;


public class SaveOptionButton : Button
{
    [Export] int saveFile = 0;
    Data data;
    AudioManager audioManager;
    Sprite sprite;
    AnimationPlayer animationPlayer;
    SceneChanger sceneChanger;
    Label numberLabel;
    Label progressLabel;
    public override void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        sprite = GetNode<Sprite>("PlayerSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        numberLabel = GetNode<Label>("NumberLabel");
        progressLabel = GetNode<Label>("ProgressLabel");
        sceneChanger = GetTree().Root.GetNode<SceneChanger>("SceneChanger");

        sprite.Visible = false;
        animationPlayer.Stop();
        numberLabel.Text = (saveFile+1).ToString();
        progressLabel.Text = ParseSaveGameData();
    }

    public string ParseSaveGameData()
    {
        GD.Print("BINGO");
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
    public void _on_SaveOptionButton_focus_entered()
    {
        sprite.Visible = true;
        animationPlayer.Play("spin");
    }

    public void _on_SaveOptionButton_focus_exited()
    {
        audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
        sprite.Visible = false;
        animationPlayer.Stop();
    }

    public void _on_SaveOptionButton_pressed()
    {
        if (progressLabel.Text == "New Game")
        {
            data.NewGame(saveFile);
            sceneChanger.ChangeScene(data.currentLevel,true);
            audioManager.PlaySFX(data.sfxTree.selectSFX);
        }
        if (progressLabel.Text != "ERROR")
        {
            data.LoadGame(saveFile);
            sceneChanger.ChangeScene(data.currentLevel,true);
            audioManager.PlaySFX(data.sfxTree.selectSFX);
        }
    }
}
