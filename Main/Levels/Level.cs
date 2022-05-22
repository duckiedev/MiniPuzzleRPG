using Godot;
using System;

public class Level : Node2D
{
    [Export] public int levelWorld;
    [Export] public int levelNumber;
    [Export] public string levelNext = "0-0";
    [Export] public string levelMusic = "levelMusic";
    [Export] public Boolean playerCanMove = false;

    public Stopwatch stopwatch;

    private Player player;
    private PlayerStart playerStart;

    public override void _Ready()
    {
        stopwatch = GetNodeOrNull<Stopwatch>("/root/Stopwatch");
        player = GetNode<Player>("/root/Player");
        playerStart = GetNodeOrNull<PlayerStart>("PlayerStart");
        
        if (playerCanMove && playerStart != null) 
        {
            if (stopwatch is null)
            {
                var scene = ResourceLoader.Load<PackedScene>("res://Main/UI/Stopwatch.tscn");
                stopwatch = scene.Instance<Stopwatch>();
                AddChild(stopwatch);
            }
            stopwatch.Reset();
            stopwatch.Display();
            stopwatch.Start();

            player.stepsTaken = 0;
            player.Position = playerStart.GlobalPosition;
            GetTree().Root.MoveChild(player,GetTree().Root.GetChildCount());
            player.stateMachine.TransitionTo("PlayerStates/Idle");
        }
        else
        {
            player.stateMachine.TransitionTo("PlayerStates/Disabled");
        }

    }
}
