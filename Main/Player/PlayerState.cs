using Godot;
using System;


public class PlayerState : State
{

    public Player player;
    public CollisionShape2D collisionShape;
    public AudioManager audioManager;
    public AnimationTree animationTree;
    public AnimationPlayer animationPlayer;
    public AnimationNodeStateMachinePlayback animStateMachine;
    public RayCast2D ray;

    public override void _Ready()
    {
        base._Ready();
        
        player = Owner as Player;
        collisionShape = player.GetNode<CollisionShape2D>("CollisionShape2D");
        audioManager = GetNode<AudioManager>("/root/AudioManager");
        animationPlayer = player.GetNode<AnimationPlayer>("AnimationPlayer");
        animationTree = player.GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        ray = player.GetNode<RayCast2D>("RayCast2D");
        animStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

    }
}
