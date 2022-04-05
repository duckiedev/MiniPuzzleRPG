using Godot;
using System;

public class ProgTracker : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.Range)] private int trackerMax;
    [Export(PropertyHint.Enum,"Enable Node,Unlock Door")] private int action;
    [Export] private String nodeName;
    
    public int trackerCurrent = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void checkProgress()
    {
        trackerCurrent++;
        if (trackerCurrent == trackerMax) {
            Node world = this.GetParent<Node>();
            var targetNode = world.GetNode(nodeName);
            switch (action){
                case 0:
                    targetNode.SetDeferred("enabled",true);
                    targetNode.CallDeferred("_Ready");
                break;
                case 1:
                    targetNode.QueueFree();
                break;
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
