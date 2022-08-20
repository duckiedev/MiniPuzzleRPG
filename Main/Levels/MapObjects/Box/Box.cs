using Godot;
using System;

public class Box : NodeGBC
{
    public enum boxState {
        IDLE,
        PUSH,
        WARP,
        DISABLED
    }
    public boxState state = boxState.IDLE;
    private Vector2 originalPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        stateMachine = GetNode<StateMachine>("StateMachine");
        tween = GetNode<GridMoveTween>("GridMoveTween");

        originalPosition = Position;
        AddToGroup("Box",true);
    }
    public void ResetBox()
    {
        Position = originalPosition;
        reset = false;
    }

    public void Destroy()
    {
        var player = GetNode<Player>("/root/Player");
        player.stateMachine.TransitionTo("PlayerStates/Idle");
        CallDeferred("queue_free");
    }

}
