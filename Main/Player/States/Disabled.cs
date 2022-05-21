using Godot;
using System;

public class Disabled : PlayerState
{
    public override void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.DISABLED;
    }
}
