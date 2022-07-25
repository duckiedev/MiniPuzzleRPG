using Godot;
using System;

public class BoxDisabled : BoxState
{
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        box.state = Box.boxState.DISABLED;
        parent.Enter();
    }

}
