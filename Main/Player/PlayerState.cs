using Godot;
using System;


public class PlayerState : State
{
    public Data data;
    public Player player;
    public AudioManager audioManager;
    public AnimationTree animationTree;
    public AnimationNodeStateMachinePlayback animStateMachine;
    public RayCast2D ray;
    public GridMoveTween tween;

    public override void _Ready()
    {
        base._Ready();
        data = GetNode<Data>("/root/Data");
        player = Owner as Player;
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        animationTree = player.GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        ray = player.GetNode<RayCast2D>("RayCast2D");
        animStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        tween = player.GetNode<GridMoveTween>("GridMoveTween");
    }
}
