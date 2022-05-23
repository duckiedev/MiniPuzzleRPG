using Godot;
using System;

public class Stopwatch : Node
{

    public TimeSpan timeElapsed;
    public Node2D textbox;
    public float minutes = 0.0f;
    public float seconds = 0.0f;
    private Boolean stopped = true;

    private Label label;

    public override void _Ready()
    {
        textbox = GetNode<Node2D>("CanvasLayer/TextBoxStatic");
        label = textbox.GetNode<Label>("NinePatchRect/Label");
        textbox.Visible = false;
        Reset();
    }
    public override void _Process(float delta)
    {
        if (!stopped) {
            minutes += delta / 60;
            seconds += delta % 60;
            timeElapsed = new TimeSpan(hours:0,  minutes: (int)minutes, seconds: (int)seconds);
        }
        if (textbox.Visible) label.Text = timeElapsed.ToString("'T-'mm':'ss") + "\nM-" + GetNode<Player>("/root/Player").stepsTaken;
    }

    public void Start()
    {
        stopped = false;
    }

    public void Stop()
    {
        stopped = true;
    }

    public void Reset()
    {
        minutes = 0.0f;
        seconds = 0.0f;
        timeElapsed = TimeSpan.Zero;
    }

    public void Display()
    {
        textbox.Visible = true;
    }

}
