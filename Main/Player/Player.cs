using Godot;
using System;

public class Player : KinematicBody2D
{
    public StateMachine stateMachine;
    public enum playerState {
        IDLE,
        MOVE,
        DISABLED
    }
    public playerState state = playerState.MOVE;
    private SceneChanger sceneChanger;

    public override void _Ready()
    {
        stateMachine = GetNode<StateMachine>("StateMachine");
        sceneChanger = GetTree().Root.GetNode<SceneChanger>("SceneChanger");
        AddToGroup("Player",true);
    }
}