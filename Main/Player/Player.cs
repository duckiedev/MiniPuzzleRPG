using Godot;
using System;

public class Player : NodeGBC
{
    public enum PlayerStates
    {
        IDLE,
        MOVE,
        DROP,
        DISABLED,
        WARP
    }
    public PlayerStates state;
    public int stepsTaken;
    
    public override void _Ready()
    {
        base._Ready();
        stateMachine = GetNode<StateMachine>("StateMachine");
        ray = GetNode<RayCast2D>("RayCast2D");
        AddToGroup("Player",true);
    }

}