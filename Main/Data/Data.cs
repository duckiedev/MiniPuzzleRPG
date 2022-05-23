using Godot;
using System;

public class Data : Node
{
    public static int gridSize = 16;
    [Export] public SfxTree sfxTree;
    [Export] public MusicTree musicTree;
    [Export] public BitmapFont mainFont;

    public string currentLevel;
    public int currentSave = 0;
    public string[] saveFileProgress = {"","",""};

    public PackedScene pauseScreenScene;
    public PauseScene pauseScreen;

    public override void _Ready()
    {
        if (ResourceLoader.Load("res://GFX/UI/font.tres") is null)
        {
            mainFont = new BitmapFont();
            var fontTexture = ResourceLoader.Load("res://GFX/UI/font.png") as Godot.Texture;
            var fontChars = " !\"#$%&'()*+,./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            mainFont.AddTexture(fontTexture);
            for (int i = 0; i < fontChars.Length(); i++)
            {
                mainFont.AddChar(fontChars.OrdAt(i), 0, new Rect2(8 * i, 0, 8, 8), new Vector2(0, 0), 8);
            }
            ResourceSaver.Save("res://GFX/UI/font.tres",mainFont);
        }

        pauseScreenScene = ResourceLoader.Load<PackedScene>("res://Main/UI/PauseScene.tscn");

        GetSaveGameData();
    }

    public void NewGame(int saveFile)
    {
        var saveData = "0-1";
        SaveGame(saveFile,saveData);
        LoadGame(saveFile);
    }
    public void SaveGame(int saveFile, string currentLevel, TimeSpan time = new TimeSpan(), int moves = 0)
    {
        var file = new File();
        var error = file.OpenEncryptedWithPass($"user://save{saveFile.ToString()}.dat",File.ModeFlags.Write,"keitaidenjuutelefang");
        if (error == Error.Ok)
        {
            file.StoreVar(currentLevel);
            file.Close();
            GetSaveGameData();
        }
        else
        {
            GD.Print("ERROR WITH FILE SAVE");
        }
    }

    public void LoadGame(int saveFile)
    {
        var file = new File();
        if (file.FileExists($"user://save{saveFile.ToString()}.dat"))
        {
            var error = file.OpenEncryptedWithPass($"user://save{saveFile.ToString()}.dat",File.ModeFlags.Read,"keitaidenjuutelefang");
            if (error == Error.Ok)
            {
                currentSave = saveFile;
                currentLevel = file.GetVar() as string;
                file.Close();
            }
        }
    }

    public void GetSaveGameData()
    {
        int files = 3;
        for (int i = 0; i < files; i++)
        {
            var file = new File();
            if (file.FileExists($"user://save{i.ToString()}.dat"))
            {
                var error = file.OpenEncryptedWithPass($"user://save{i.ToString()}.dat",File.ModeFlags.Read,"keitaidenjuutelefang");
                if (error == Error.Ok)
                {
                    saveFileProgress[i] = file.GetVar() as string;
                }
                else
                {
                    saveFileProgress[i] = "ERROR";
                }
            }
            else
            {
                saveFileProgress[i] = "";
            }
            file.Close();
        }
    }

    public void PauseGame()
    {
        GetNode<AudioManager>("/root/AudioManager").PauseMusic();

        pauseScreen = pauseScreenScene.Instance() as PauseScene;
        GetTree().Root.AddChild(pauseScreen);
        GetTree().Root.MoveChild(pauseScreen,GetTree().Root.GetChildCount());
        pauseScreen.Owner = GetTree().Root;
        GetTree().Paused = true;
    }
    public void UnpauseGame()
    {
        GetTree().Root.RemoveChild(pauseScreen);
        pauseScreen.QueueFree();
        GetNode<Player>("/root/Player").stateMachine.TransitionTo("PlayerStates/Idle");
        GetNode<AudioManager>("/root/AudioManager").UnpauseMusic();
        GetTree().Paused = false;
    }


}
