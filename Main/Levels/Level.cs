using Godot;
using System;

public class Level : Node2D
{
    [Export] public int levelWorld;
    [Export] public int levelNumber;
    [Export] public string levelNext = "0-0";
    [Export] public string levelMusic = "levelMusic";
    [Export] public Boolean playerCanMove = false;

    private Player player;
    private PlayerStart playerStart;

    public override void _Ready()
    {
        player = GetNode<Player>("/root/Player");
        playerStart = GetNodeOrNull<PlayerStart>("PlayerStart");
        
        if (playerCanMove && playerStart != null) 
        {
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
