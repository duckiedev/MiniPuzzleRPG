using Godot;
using System;

public class ProgTracker : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.Range)] private int trackerMax;
    
    public int trackerCurrent = 0;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void checkProgress()
    {
        trackerCurrent++;
        GD.Print("Incrementing progress...");
        if (trackerCurrent == trackerMax) {
            for (int i = 0; i < GetChildCount(); i++)
            {
                var currentChild = GetChild(i);
                if (currentChild.HasMethod("Run")) {
                    GD.Print("it work?");
                    currentChild.Call("Run");
                }
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
