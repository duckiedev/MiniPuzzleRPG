using Godot;
using System;

public class Warp : PlayerState
{
    public override async void Enter(Godot.Collections.Dictionary msg)
    {
        player.state = Player.PlayerStates.WARP;
        if (msg.Count > 0)
        {
            animationTree.Active = false;
            animationPlayer.Play("spin");
            tween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                0.5,
                3,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            tween.InterpolateProperty(
                player,
                "position",
                player.Position,
                player.Position + new Vector2(0,-96),
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            tween.Start();

            await ToSignal(tween,"tween_all_completed");

            animationTree.Active = false;
            animationPlayer.Play("spin");
            WarpTile warpTo = msg["warpTo"] as WarpTile;
            tween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                3,
                0.5,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            tween.InterpolateProperty(
                player,
                "position",
                new Vector2(0,-96) + warpTo.GlobalPosition,
                warpTo.GlobalPosition,
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            tween.Start();

            await ToSignal(tween,"tween_all_completed");

            animationTree.Active = true;
            animationPlayer.Stop();
            var args = new Godot.Collections.Dictionary();
            GD.Print("boxplaceGP : " + warpTo.boxPlace.GlobalPosition + " warptoGP : " + warpTo.GlobalPosition);
            GD.Print("MoveTo: " + (warpTo.boxPlace.GlobalPosition-warpTo.GlobalPosition)/16);
            args.Add("dir",(warpTo.boxPlace.GlobalPosition-warpTo.GlobalPosition)/16);
            stateMachine.TransitionTo("PlayerStates/Move",args);
        }
    }

}
