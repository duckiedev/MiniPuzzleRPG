using Godot;
using System;

public class MainScreenMenu : Control
{

    private VBoxContainer optionBox;
    private int options;
    private int selectedOption = 0;
    private Sprite playerSprite;
    PauseScene pauseScene;
 
    public override void _Ready()
    {
        optionBox = GetNode<VBoxContainer>("MainScreenOptions");
        options = optionBox.GetChildCount();
        playerSprite = GetNode<Sprite>("PlayerSprite");
        pauseScene = GetNode<PauseScene>("/root/PauseScene");
    }

    public override void _Input(InputEvent @event)
    {
        if (pauseScene.pauseState == PauseScene.PauseStates.MAIN)
        {
            if (@event.IsActionPressed("ui_up"))
            {
                selectedOption -= 1;
                selectedOption = Mathf.Wrap(selectedOption,0,options);
                UpdatePlayerPosition();
            }
            else if (@event.IsActionPressed("ui_down"))
            {
                selectedOption += 1;
                selectedOption = Mathf.Wrap(selectedOption,0,options);
                UpdatePlayerPosition();
            }
            else if (@event.IsActionPressed("ui_accept"))
            {
                optionBox.GetChild<OptionLabel>(selectedOption).Run();
            }
        }
    }
    public void UpdatePlayerPosition()
    {
        playerSprite.Position = new Vector2(0, selectedOption * 16);
    }

}
