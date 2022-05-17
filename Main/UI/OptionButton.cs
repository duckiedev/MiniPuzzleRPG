using Godot;
using System;

public class OptionButton : Button
{
    [Export] public String defaultText = "";
    public Data data;
    public AudioManager audioManager;
    public Sprite sprite;
    public AnimationPlayer animationPlayer;
    public SceneChanger sceneChanger;
    public Label mainLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        sceneChanger = GetNode<SceneChanger>("/root/SceneChanger");

        sprite = GetNode<Sprite>("PlayerSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        mainLabel = GetNode<Label>("MainLabel");

        sprite.Visible = false;
        animationPlayer.Stop();
    }

    public virtual void _on_OptionButton_focus_entered()
    {
        sprite.Visible = true;
        animationPlayer.Play("spin");
    }

    public virtual void _on_OptionButton_focus_exited()
    {
        audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
        sprite.Visible = false;
        animationPlayer.Stop();
    }

    public virtual void _on_OptionButton_pressed()
    {
        
    }
}
