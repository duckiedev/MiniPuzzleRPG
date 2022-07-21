using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public int zFloor = 0;
    public int zCurrent = 0;

    public enum PlayerStates
    {
        IDLE,
        MOVE,
        DROP,
        DISABLED,
        WARP
    }

    public StateMachine stateMachine;
    public PlayerStates state;
    public int stepsTaken;

    public override void _Ready()
    {
        stateMachine = GetNode<StateMachine>("StateMachine");
        AddToGroup("Player",true);
    }

}