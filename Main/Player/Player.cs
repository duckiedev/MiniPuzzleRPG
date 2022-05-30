using Godot;
using System;

public class Player : KinematicBody2D
{
    public enum PlayerStates
    {
        IDLE,
        MOVE,
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