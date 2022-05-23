using Godot;
using System;

public class TextBox : Node2D
{
    public enum textBoxState
    {
        INITIALIZED,
        WAITFORINPUT,
        STARTED,
        PRINTING,
        PRINTING_FINISHED_MORE_LINES,
        PRINTING_FINISHED,
        TEXT_CLEARING,
        TEXT_CLEARING_FINISHED
    }
    public textBoxState state = textBoxState.PRINTING;
    public Label textLabel;
    public AnimatedSprite cursor;
    public AnimationPlayer animationPlayer;
    public string[] dialogPages;
    private int currentPage = 0;
    private int currentChunk;
    private Player player;

    private Boolean printing = false;
    private float timer = 0;
    private string textToPrint = "man, thats a lot of text to parse. not sure what to do with it all yet.";
    private int currentChar = 0;
    private int lineCharsLeft = 17;
    private int lineCurrent = 0;
    private int lineMaxDisplay = 2;
    private float SPEED = 0.1f;

    public override void _Ready()
    {
        textLabel = GetNode<Label>("CanvasLayer/NinePatchRect/Label");
        cursor = GetNode<AnimatedSprite>("CanvasLayer/NinePatchRect/Cursor");
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        player = GetNode<Player>("/root/Player");
        player.stateMachine.TransitionTo("PlayerStates/Disabled");
        printing = true;
        //GD.Print(dialogPages);
        //Start();
    }

    public override void _Input(InputEvent @event)
    {
        switch (state)
        {
            case textBoxState.STARTED:

            break;

            case textBoxState.PRINTING:
            case textBoxState.TEXT_CLEARING:
                //SPEED = ((Input.GetActionStrength("a")) * 2) * SPEED;
            break;

            case textBoxState.PRINTING_FINISHED_MORE_LINES:
                if (@event.IsActionPressed("a"))
                {
                    textLabel.LinesSkipped++;
                    cursor.Visible = false;
                    cursor.Stop();
                    state = textBoxState.PRINTING;
                }
            break;

            case textBoxState.TEXT_CLEARING_FINISHED:

            break;
        }
    }

    public override void _Process(float delta)
    {
        switch (state)
        {
            case textBoxState.PRINTING:
                timer += delta;
                if (timer > SPEED)
                {
                    // check for line break
                    // check for a space
                    if (textToPrint[currentChar] == ' ')
                    {
                        var nextWordCount = 0;
                        // go through each character after the space until the next space
                        for (int i = 0; i < textToPrint.Length; i++)
                        {
                            if (textToPrint[currentChar+1+i] == ' ')
                            {
                                nextWordCount = i;
                                break;
                            }
                        }
                        GD.Print($"i:{nextWordCount} lineMax:{lineCharsLeft}");
                        if (nextWordCount > lineCharsLeft)
                        {
                            // go to next line in the text
                            textLabel.Text = textLabel.Text + "\n";
                            // go to next line
                            lineCurrent++;
                            // reset line chars limit
                            lineCharsLeft = 17;
                            // skip the space
                            currentChar++;
                            // if we're at the b
                            if (lineCurrent > lineMaxDisplay) {
                                state = textBoxState.PRINTING_FINISHED_MORE_LINES;
                            }
                        }
                    }
                    timer = 0;
                    textLabel.Text = textLabel.Text + textToPrint[currentChar];
                    currentChar++;
                    lineCharsLeft--;
                }

                if (currentChar == textToPrint.Length)
                {
                    GD.Print("Finished");
                    state = textBoxState.PRINTING_FINISHED;
                    currentChar = 0;
                    textToPrint = "";
                    printing = false;
                    timer = 0;
                }
            break;

            case textBoxState.PRINTING_FINISHED_MORE_LINES:

            break;
            case textBoxState.PRINTING_FINISHED:
                cursor.Visible = true;
                cursor.Play();
            break;
            default:
            break;
        }
    }
    public async void Start()
    {
        GD.Print("Starting with page " + currentPage);
        state = textBoxState.STARTED;
        // reset the box
        textLabel.LinesSkipped = 0;
        textLabel.PercentVisible = 0;
        // set the text
        textLabel.Text = dialogPages[currentPage];
        // get the text vars
        if (textLabel.GetLineCount() > textLabel.MaxLinesVisible)
        {
            GD.Print("MORE TEXT");
            currentChunk = textLabel.GetTotalCharacterCount() / textLabel.GetLineCount();
            GD.Print(currentChunk);
        }

        // display the text
       //state = textBoxState.TEXT_TYPING;
        //animationPlayer.Play("type_text");
        //await ToSignal(animationPlayer,"animation_finished");
        //TypingFinished();
    }
    public void TypingFinished()
    {
        //state = textBoxState.TEXT_TYPING_FINISHED;

        // wait for _Input
    }
    public async void ClearTextbox()
    {
        cursor.Stop();
        cursor.Visible = false;
        state = textBoxState.TEXT_CLEARING;
        animationPlayer.Play("clear_textbox");
        await ToSignal(animationPlayer,"animation_finished");
        GD.Print("Clearing textbox for " + currentPage + " out of " + dialogPages.Length);
        if (currentPage != dialogPages.Length)
        {
            currentPage++;
            GD.Print(currentPage);
            Start();
        }
        else 
        {
            CloseTextbox();
        }
    }

    public void CloseTextbox()
    {
        player.stateMachine.TransitionTo("PlayerStates/Idle");
        QueueFree();
    }
}
