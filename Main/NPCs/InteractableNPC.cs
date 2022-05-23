using Godot;
using System;

public class InteractableNPC : Node2D
{
    [Export] Resource npcScript;
    public override void _Ready()
    {
        var dialogScene = ResourceLoader.Load<PackedScene>("res://Main/UI/TextBox.tscn");
        var dialog = dialogScene.Instance<TextBox>();
        GetNode<Level>("/root/Level").CallDeferred("add_child",dialog);
        string[] dialogPages = {"man, thats a lot of text to parse. not sure what to do with it all yeet.", "this is the second page of testing many pages."};
        dialog.dialogPages = dialogPages;
    }

}
