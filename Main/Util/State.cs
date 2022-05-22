using Godot;
using System;

public class State : Node
{

    public State parent = null;
    public StateMachine stateMachine;

    [Export] NodePath stateMachinePath;

    public async override void _Ready()
    {
        stateMachine = GetNode<StateMachine>(stateMachinePath);
        await ToSignal(Owner, "ready");
        if (!GetParent().IsInGroup("stateMachine"))
        {
            parent = GetParent<State>();
        }
    }

    public virtual void UnhandledInput(InputEvent @event)
    {
        
    }

    public virtual void Process(float delta)
    {
        
    }

    public virtual void PhysicsProcess(float delta)
    {
        
    }

    public virtual void Enter(Godot.Collections.Dictionary msg)
    {
        
    }
    public virtual void Enter()
    {
        this.Enter(new Godot.Collections.Dictionary());
    }

    public virtual void Exit()
    {

    }

    public Node GetStateMachine(Node node)
    {
        if (node != null && !node.IsInGroup("stateMachine"))
        {
            GetStateMachine(node.GetParent());
        }
        return node;
    }

}
