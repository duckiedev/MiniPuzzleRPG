using Godot;
using System;

public class StateMachine : Node
{
    [Signal] delegate void Transitioned(String statePath);
    [Export] NodePath initialState;
    public State state;
    public String stateName = "";

    public StateMachine()
    {
        AddToGroup("stateMachine");
    }
    
    public async override void _Ready()
    {
        state = GetNode<State>(initialState);
        await ToSignal(Owner, "ready");
        state.Enter();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        state.UnhandledInput(@event);
    }

    public override void _Process(float delta)
    {
        state.Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        state.PhysicsProcess(delta);
    }


    public void TransitionTo(String targetStatePath, Godot.Collections.Dictionary msg)
    {
        if (!HasNode(targetStatePath)) return;

        var targetState = GetNode<State>(targetStatePath);
        state.Exit();
        state = targetState;
        state.Enter(msg);
        EmitSignal("Transitioned", targetStatePath);
    }

    public void TransitionTo(String targetStatePath)
    {
        this.TransitionTo(targetStatePath, new Godot.Collections.Dictionary());
    }

    public void SetState(State value)
    {
        state = value;
        stateName = state.Name;
    }
}
