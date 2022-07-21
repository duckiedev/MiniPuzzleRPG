using Godot;
using System;

public class ActionDropDown : Action
{
    [Export] public String objectGroup;
    [Export(PropertyHint.Enum,"Up,Down,Left,Right")] public String ledgeDirection;
    private Node obj;
    private Area2D area2d;
    private Boolean objEntered = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //area2d = GetNode<Area2D>("Area2D");
    }

    public override void _Process(float delta)
    {

    }

    public void _on_Area2D_body_entered(Node body)
    {
        objEntered = body.IsInGroup(objectGroup);  
    }

    public void _on_Area2D_body_exited(Node body)
    {
        if (objEntered)
        {
            Box box = body as Box;
            
        }
    }
    public override void Run()
    {
        base.Run();
    }

}
