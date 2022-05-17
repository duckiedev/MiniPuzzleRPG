using Godot;
using System;

public enum PlayerStates
{
    IDLE,
    MOVE,
    PUSH,
    DISABLED
}
public class PlayerState : State
{
    public Data data;
    public Player player;
    //public CollisionShape2D playerCollision;
    public AudioManager audioManager;
    public AnimationTree animationTree;
    public AnimationNodeStateMachinePlayback animStateMachine;
    public RayCast2D ray;
    public GridMoveTween tween;

    public override void _Ready()
    {
        base._Ready();
        data = GetTree().Root.GetNode<Data>("Data");
        player = Owner as Player;
        //playerCollision = player.GetNode<CollisionShape2D>("CollisionShape2D");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        animationTree = (AnimationTree)player.GetNode("AnimationTree");
        animationTree.Active = true;
        ray = player.GetNode<RayCast2D>("RayCast2D");
        animStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        tween = player.GetNode<GridMoveTween>("GridMoveTween");
    }
}
