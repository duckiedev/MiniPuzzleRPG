using Godot;
using System;
[Tool]
public class ActionDestroyObstacle : Action
{
    [Export] private NodePath obstaclePath;
    private Obstacle obstacle;
    public override void Run()
    {
        obstacle = GetNode<Obstacle>(obstaclePath);
        obstacle.CallDeferred("queue_free");
    }

}
