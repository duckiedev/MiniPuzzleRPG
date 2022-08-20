using Godot;
using System;

public class BoxWarp : BoxState
{
    public override async void Enter(Godot.Collections.Dictionary msg)
    {
        GetNode<Player>("/root/Player").stateMachine.TransitionTo("PlayerStates/Disabled");
        if (msg.Count > 0)
        {
            //animationTree.Active = false;
            //animationPlayer.Play("spin");
            /*tween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                0.5,
                3,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );*/

            tween.InterpolateProperty(
                box,
                "position",
                box.Position,
                box.Position + new Vector2(0,-96),
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            tween.Start();

            await ToSignal(tween,"tween_all_completed");

            //animationTree.Active = false;
            //animationPlayer.Play("spin");
            WarpTile warpTo = msg["warpTo"] as WarpTile;
            /*
            tween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                3,
                0.5,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );*/

            tween.InterpolateProperty(
                box,
                "global_position",
                new Vector2(0,-96) + warpTo.GlobalPosition,
                warpTo.GlobalPosition,
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            tween.Start();

            await ToSignal(tween,"tween_all_completed");

            //animationTree.Active = true;
            //animationPlayer.Stop();
            //var args = new Godot.Collections.Dictionary();
            box.SetNextPosition(warpTo.boxPlace.GlobalPosition-warpTo.GlobalPosition);
            stateMachine.TransitionTo("BoxStates/BoxPushed");
        }
    }
}
