using Godot;
using System;

public class MapObject : Node2D
{
    [Export] public Boolean crystalSwitch;
    public override void _Ready()
    {
        
    }

    public async void BreakCrystals()
    {
        var data = GetNode<Data>("/root/Data");
        var player = GetNode<Player>("/root/Player");
        player.stateMachine.TransitionTo("PlayerStates/Disabled");

        var breakableCrystalNode = GetNode<BreakableCrystals>("/root/Level/BreakableCrystals");
        var breakableCrystalChildren = breakableCrystalNode.GetChildren();

        var camera = GetNode<Camera2D>("/root/Level/Camera2D");
        camera.target = breakableCrystalNode.mainSwitch;

        var audioManager = GetNode<AudioManager>("/root/AudioManager");
        audioManager.PlaySFX(data.sfxTree.crystalBreak);
        // Hide the sprite of the main crystal first
        GetNode<Sprite>("Sprite").Visible = false;

        await ToSignal(GetTree().CreateTimer(0.5f),"timeout");
        // Destroy each of the crystals associated with it
        foreach (Node item in breakableCrystalChildren)
        {
            GD.Print(item.Name);
            if (item.Name.BeginsWith("MapObject"))
            {
                var crystal = (Node2D)item;
                camera.target = crystal;
                await ToSignal(GetTree().CreateTimer(0.5f),"timeout");
                audioManager.PlaySFX(data.sfxTree.crystalBreak);
                item.QueueFree();
            }
        }
        GetNode<Node2D>("/root/Level/BreakableCrystals").QueueFree();
        camera.ResetTarget();
        player.stateMachine.TransitionTo("PlayerStates/Idle");
        QueueFree();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
