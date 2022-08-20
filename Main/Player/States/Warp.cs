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
            player.tween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                0.5,
                3,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            player.tween.InterpolateProperty(
                player,
                "position",
                player.Position,
                player.Position + new Vector2(0,-96),
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            player.tween.Start();

            await ToSignal(player.tween,"tween_all_completed");

            animationTree.Active = false;
            animationPlayer.Play("spin");
            WarpTile warpTo = msg["warpTo"] as WarpTile;
            player.tween.InterpolateProperty(
                animationPlayer,
                "playback_speed",
                3,
                0.5,
                0.5f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            player.tween.InterpolateProperty(
                player,
                "position",
                new Vector2(0,-96) + warpTo.GlobalPosition,
                warpTo.GlobalPosition,
                1.0f,
                Tween.TransitionType.Sine,
                Tween.EaseType.In
            );

            player.tween.Start();

            await ToSignal(player.tween,"tween_all_completed");

            animationTree.Active = true;
            animationPlayer.Stop();
            player.vectorPos = (warpTo.boxPlace.GlobalPosition-warpTo.GlobalPosition)/Data.gridSize;
            stateMachine.TransitionTo("PlayerStates/Move");
        }
    }

}
