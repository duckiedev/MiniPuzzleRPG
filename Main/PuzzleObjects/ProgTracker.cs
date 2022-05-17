using Godot;
using System;

public class ProgTracker : Node
{

    [Export(PropertyHint.Range)] private int trackerMax;
    
    public int trackerCurrent = 0;

    public override void _Ready()
    {
        
    }

    public void checkProgress()
    {
        trackerCurrent++;
        if (trackerCurrent == trackerMax) {
            for (int i = 0; i < GetChildCount(); i++)
            {
                var currentChild = GetChild(i);
                if (currentChild.HasMethod("Run")) {
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
