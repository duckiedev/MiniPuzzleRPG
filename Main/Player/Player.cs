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
    public Godot.Collections.Dictionary inputs;
    public int stepsTaken;
    
    public override void _Ready()
    {
        base._Ready();
        CreateInputs();
        stateMachine = GetNode<StateMachine>("StateMachine");
        animationTree = (AnimationTree)GetNode("AnimationTree");
        animationTree.Active = true;
        animStateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
        AddToGroup("Player",true);
    }

    private void CreateInputs()
    {
        inputs = new Godot.Collections.Dictionary();
        inputs.Add("up",Vector2.Up);
        inputs.Add("down",Vector2.Down);
        inputs.Add("left",Vector2.Left);
        inputs.Add("right",Vector2.Right);
    }

}