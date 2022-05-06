using Godot;
using System;

public class OptionButton : Button
{

    [Export] string defaultText = "";
    Data data;
    AudioManager audioManager;

    public override void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
    }

    public void _on_New_focus_entered()
    {
        Text = $"*{defaultText}";    
    }

    public void _on_New_focus_exited()
    {
        Text = defaultText;
        audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
    }
}
