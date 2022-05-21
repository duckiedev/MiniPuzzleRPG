using Godot;
using System;

public class OptionMenu : Control
{
    private VBoxContainer optionBox;
    private int options;
    private int selectedOption = 0;
    private Data data;
    public Sprite playerSprite;
    public AnimationPlayer animationPlayer;
    public Tween playerTween;
    private AudioManager audioManager;
    public Boolean optionSelected = false;

    public override void _Ready()
    {
        optionBox = GetNode<VBoxContainer>("Options");
        options = optionBox.GetChildCount();
        data = GetNode<Data>("/root/Data");
        playerSprite = GetNode<Sprite>("PlayerSprite");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        playerTween = playerSprite.GetNode<Tween>("Tween");
        audioManager = GetNode<AudioManager>("/root/AudioManager");

        animationPlayer.Play("spin");
    }

    public override void _Input(InputEvent @event)
    {
        if (!optionSelected)
        {
            if (@event.IsActionPressed("up"))
            {
                selectedOption -= 1;
                selectedOption = Mathf.Wrap(selectedOption,0,options);
                UpdatePlayerPosition();
                audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
            }
            else if (@event.IsActionPressed("down"))
            {
                selectedOption += 1;
                selectedOption = Mathf.Wrap(selectedOption,0,options);
                UpdatePlayerPosition();
                audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
            }
            else if (@event.IsActionPressed("a"))
            {
                optionSelected = true;
                optionBox.GetChild<OptionLabel>(selectedOption).Run();
                audioManager.PlaySFX(data.sfxTree.selectSFX);
            }
        }
    }

    public void UpdatePlayerPosition()
    {
        playerSprite.Position = new Vector2(0, selectedOption * 16);
    }
}
