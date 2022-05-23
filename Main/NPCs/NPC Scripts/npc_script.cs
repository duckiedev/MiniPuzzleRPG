using Godot;
using System;

public class npc_script : Resource
{
    [Export] Sprite sprite;
    [Export] Vector2 facing;
    [Export] Script movementType;
    [Export] Script function;
    [Export] Resource dialog;
}
