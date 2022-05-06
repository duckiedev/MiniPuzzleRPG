using Godot;
using System;

public class Player : KinematicBody2D
{
    private Data data;
    private enum playerState {
        MOVE,
        PUSH
    }
    private playerState state = playerState.MOVE;
    private float gridSize = 16;
    private AudioManager audioManager;
    private AnimationTree animationTree;
    private AnimationNodeStateMachinePlayback stateMachine;
    private RayCast2D ray;
    private SceneChanger sceneChanger;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        data = GetTree().Root.GetNode<Data>("Data");
        sceneChanger = GetTree().Root.GetNode<SceneChanger>("SceneChanger");
        audioManager = GetTree().Root.GetNode<AudioManager>("AudioManager");
        ray = this.GetNode<RayCast2D>("RayCast2D");
        animationTree = (AnimationTree)GetNode("AnimationTree");
        animationTree.Active = true;

        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("next_map"))
        {
            sceneChanger.ChangeScene("res://Main/Levels/Level0-2.tscn");
        }
        if (Input.IsActionJustPressed("reset"))
        {
            GetTree().ReloadCurrentScene();
        }
        switch (state)
        {
            case playerState.MOVE:
                moveState(getPlayerMovementInput());
            break;
        }
    }
    public Vector2 getPlayerMovementInput()
    {
        Vector2 inputVector = new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"), // x
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")     // y
        );
        return inputVector;
    }
    public void moveState(Vector2 dir){
        if (Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left") || Input.IsActionJustPressed("ui_up")|| Input.IsActionJustPressed("ui_down"))
        {
            animationTree.Set("parameters/idle/blend_position", dir);
            animationTree.Set("parameters/push/blend_position", dir);
            Boolean canMove = false;
            Vector2 vectorPos = dir * gridSize;
            ray.CastTo = vectorPos;
            ray.ForceRaycastUpdate();

            if (ray.IsColliding()) 
            {
                Node collider = (Node)ray.GetCollider();
                if (collider.IsInGroup("Box")) 
                {
                    Box collidedBox = (Box)collider;
                    audioManager.PlaySFX(data.sfxTree.boxMoveSFX);
                    stateMachine.Travel("push");
                        if (collidedBox.move(dir)) 
                        {
                            canMove = true;
                        } else {
                            canMove = false;
                        }
                } else {
                     canMove = false;
                }
            } else {
                stateMachine.Travel("idle");
                audioManager.PlaySFX(data.sfxTree.playerMoveSFX);
                canMove = true;
            }
            
            if (canMove) {
                Position += vectorPos;
            }
        }
    }
}