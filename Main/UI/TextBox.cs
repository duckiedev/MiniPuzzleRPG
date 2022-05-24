using Godot;
using System;

public class TextBox : Node2D
{
    public enum textBoxState
    {
        INITIALIZE,
        WAITFORINPUT,
        STARTED,
        PRINTING,
        PRINTING_FINISHED_MORE_LINES,
        PRINTING_FINISHED,
        CLEARING,
        CLEARING_FINISHED,
        CLOSE
    }
    public textBoxState state = textBoxState.INITIALIZE;
    public Label textLabel;
    public AnimatedSprite cursor;
    public AnimationPlayer animationPlayer;
    public string[] dialogPages;
    private int currentPage = 0;
    private Player player;
    private AudioManager audioManager;
    private Data data;


    private float timer = 0;
    private string textToPrint;
    private int currentChar = 0;
    private int lineCharsLeft = 17;
    private int lineCurrent = 0;
    private int lineMaxDisplay = 2;
    private int lineCleared = 0;
    private int lastCount;
    private float SPEED = 0.05f;
    private int pauseChar = 0;

    public override void _Ready()
    {
        data = GetNode<Data>("/root/Data");
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        textLabel = GetNode<Label>("CanvasLayer/NinePatchRect/Label");
        cursor = GetNode<AnimatedSprite>("CanvasLayer/NinePatchRect/Cursor");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        player = GetNode<Player>("/root/Player");
        player.stateMachine.TransitionTo("PlayerStates/Disabled");
    }

    public override void _Input(InputEvent @event)
    {
        switch (state)
        {
            case textBoxState.PRINTING_FINISHED_MORE_LINES:
                if (@event.IsActionPressed("a"))
                {
                    audioManager.PlaySFX(data.sfxTree.dialogBlipDone);
                    textLabel.LinesSkipped++;
                    HideCursor();
                    state = textBoxState.PRINTING;
                }
            break;

            case textBoxState.PRINTING_FINISHED:
                if (@event.IsActionPressed("a"))
                {
                    audioManager.PlaySFX(data.sfxTree.dialogBlipDone);
                    HideCursor();
                    state = textBoxState.CLEARING;
                }
            break;
        }
    }

    public override void _Process(float delta)
    {
        if (Input.GetActionStrength("a") == 1)
        {
            SPEED = 0.03f + (pauseChar * 0.1f);
        }
        else
        {
            SPEED = 0.1f + (pauseChar * 0.5f);
        }
        switch (state)
        {
            case textBoxState.INITIALIZE:
                // reset ALL THE THINGS
                timer = 0;
                currentChar = 0;
                lineCharsLeft = 17;
                lineCurrent = 0;
                lineMaxDisplay = 2;
                lineCleared = 0;
                textLabel.LinesSkipped = 0;
                textLabel.Text = "";
                textToPrint = dialogPages[currentPage];
                lastCount = textLabel.Text.Length;
                state = textBoxState.PRINTING;
            break;

            case textBoxState.PRINTING:
                timer += delta;
                if (timer > SPEED)
                {
                    pauseChar = 0;
                    // check for line break
                    // check for a space
                    if (textToPrint[currentChar] == ' ')
                    {
                        var nextWordCount = 0;
                        // go through each character after the space until the next space
                        for (int i = 0; i < textToPrint.Length - currentChar; i++)
                        {
                            // get the character after the previous, starting after the space
                            var futureChar = currentChar+1+i;
                            // if it's a space or the end of the string
                            if (textToPrint[futureChar] == ' ' || futureChar == textToPrint.Length-1)
                            {
                                // set the number of characters to next word count
                                nextWordCount = i;
                                break;
                            }
                        }
                        //GD.Print($"nextWordCount:{nextWordCount} lineCharsLeft:{lineCharsLeft}");
                        // if the next word count is greater than the characters left on the line
                        if (nextWordCount > lineCharsLeft)
                        {
                            // add a linebreak to the text
                            textLabel.Text = textLabel.Text + "\n";
                            // go to next line
                            lineCurrent++;
                            // reset line chars limit
                            lineCharsLeft = 17;
                            // skip the space
                            currentChar++;
                            // if we're at the end of the maxDisplay
                            if (lineCurrent > lineMaxDisplay) {
                                ShowCursor();
                                state = textBoxState.PRINTING_FINISHED_MORE_LINES;
                            }
                        }
                    }
                    if (textToPrint[currentChar] == '.' || textToPrint[currentChar] == ',' || textToPrint[currentChar] == '!' || textToPrint[currentChar] == '?')
                    {
                        pauseChar = 1;
                    }
                    timer = 0;
                    textLabel.Text = textLabel.Text + textToPrint[currentChar];
                    audioManager.CallDeferred("PlaySFX",data.sfxTree.dialogBlip01);
                    currentChar++;
                    lineCharsLeft--;
                }

                if (currentChar == textToPrint.Length)
                {
                    ShowCursor();
                    state = textBoxState.PRINTING_FINISHED;
                    pauseChar = 0;
                    currentChar = 0;
                    timer = 0;
                }
            break;

            case textBoxState.CLEARING:
                timer += delta;
                if (timer > SPEED * 2)
                {
                    if (lineCleared <= lineMaxDisplay)
                    {
                        lineCleared++;
                        textLabel.LinesSkipped++;
                        timer = 0;
                    }
                    else
                    {
                        state = textBoxState.CLEARING_FINISHED;
                    }
                }
            break;

            case textBoxState.CLEARING_FINISHED:
                if (currentPage != dialogPages.Length-1)
                {
                    currentPage++;
                    state = textBoxState.INITIALIZE;
                }
                else 
                {
                    state = textBoxState.CLOSE;
                }
            break;

            case textBoxState.CLOSE:
                player.stateMachine.TransitionTo("PlayerStates/Idle");
                QueueFree();
            break;
        }
    }
    private void ShowCursor()
    {
        cursor.Visible = true;
        cursor.Play();
    }
    private void HideCursor()
    {
        cursor.Stop();
        cursor.Visible = false;
    }

}
