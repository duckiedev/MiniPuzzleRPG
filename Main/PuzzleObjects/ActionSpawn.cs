using Godot;
using System;

public class ActionSpawn : Action
{

    [Export(PropertyHint.File)] public PackedScene spawnFile;
    public Node spawnInstance;

    public override void _Ready()
    {

    }

    public override void Run()
    {
        spawnInstance = spawnFile.Instance();
        GetTree().Root.GetNode<Level>("Level").CallDeferred("add_child",spawnInstance);

        spawnInstance.Set("position",Position);
    }
}
