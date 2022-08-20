using Godot;
using System;

public class WarpTile : Node2D
{
    [Export] public NodePath warpToPath;
    private WarpTile warpTo;
    public Node2D boxPlace;
    public Boolean disabled;
    public override void _Ready()
    {
        warpTo = GetNode<WarpTile>(warpToPath);
        boxPlace = GetNode<Node2D>("BoxPlace");
    }

    public async void _on_Area2D_body_entered(Node body)
    {
        if (!disabled)
        {
            await ToSignal(body.GetNode<GridMoveTween>("GridMoveTween"),"tween_completed");
            warpTo.disabled = true;
            Godot.Collections.Dictionary args = new Godot.Collections.Dictionary();
            args.Add("warpTo",warpTo);

            if (body.IsInGroup("Player"))
            {
                Player player = body as Player;
                player.stateMachine.TransitionTo("PlayerStates/Warp",args);
            }
            else if (body.IsInGroup("Box"))
            {
                Box box = body as Box;
                box.stateMachine.TransitionTo("BoxStates/BoxWarp",args);
            }
        }
    }

    public void _on_Area2D_body_exited(Node2D body)
    {
        if (!disabled) return;
        if (body.IsInGroup("Player") || body.IsInGroup("Box"))
        {
            if (body.GlobalPosition != GlobalPosition)
            {
                disabled = false;
            }
        }
    }

}
