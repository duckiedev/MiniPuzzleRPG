using Godot;
using System;

public class Level : Node2D
{
    [Export] public int levelWorld;
    [Export] public int levelNumber;
    [Export] public string levelNext = "0-0";
    [Export] public string levelMusic = "levelMusic";
    [Export] public Boolean playerCanMove = false;

    public override void _Ready()
    {
        if (playerCanMove) {
            GetNode<Player>("/root/Player").stateMachine.TransitionTo("PlayerStates/Idle");
        }
        else
        {
            GetNode<Player>("/root/Player").stateMachine.TransitionTo("PlayerStates/Disabled");
        }

    }
}
