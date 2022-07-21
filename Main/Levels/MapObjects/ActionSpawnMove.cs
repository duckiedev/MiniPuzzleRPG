using Godot;
using System;

public class ActionSpawnMove : ActionSpawn
{
    Data data;
    Tween tween;
    Node2D startPoint;
    Player player;
    public override void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        startPoint = GetNode<Node2D>("StartPoint");
    }

    public override void Run()
    {
        if (CheckIfCanSpawn())
        {
            spawnInstance = spawnFile.Instance();
            Box boxInstance = spawnInstance as Box;
            boxInstance.Set("position",startPoint.GlobalPosition);
            GetTree().Root.GetNode<Level>("Level").Call("add_child",boxInstance);
            var args = new Godot.Collections.Dictionary();
            args.Add("endPosition",GlobalPosition);
            boxInstance.stateMachine.TransitionTo("BoxStates/BoxMoveSpawn",args);
        }
    }

    public Boolean CheckIfCanSpawn()
    {
        if (IsInstanceValid(spawnInstance))
        {
            Box boxInstance = spawnInstance as Box;
            if (GlobalPosition == boxInstance.Position || boxInstance.tween.IsActive())
            {
                return false;
            }
        }
        return true;
    }
}
