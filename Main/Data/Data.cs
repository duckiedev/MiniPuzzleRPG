using Godot;
using System;
using System.Globalization;

public class Data : Node
{
    public static int gridSize = 16;
    [Export] public SfxTree sfxTree;
    [Export] public MusicTree musicTree;
    [Export] public BitmapFont mainFont;
    public string currentLevel;
    public int currentSave = 0;
    public String[] saveFileProgress = {"","",""};
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

    public Godot.Collections.Dictionary CreateSaveLevelData(TimeSpan time, int moves)
    {
        var saveLevelData = new Godot.Collections.Dictionary();
        saveLevelData.Add("time",TimeToString(time));
        saveLevelData.Add("moves",moves);
        return saveLevelData;
    }

    public String TimeToString(TimeSpan time)
    {
        return time.ToString("mm':'ss");
    }

    public TimeSpan StringToTime(String time)
    {
        return TimeSpan.ParseExact(time,"mm':'ss",CultureInfo.CurrentCulture);
    }

    public void NewGame(int saveFile)
    {
        currentLevel = "0-1";
        SaveGame(saveFile,currentLevel,TimeSpan.Zero,0);
        LoadGame(saveFile);
    }

    public void SaveGame(int saveFile, String currentLevel, TimeSpan time, int moves)
    {
        var file = new File();
        var error = file.Open($"user://save{saveFile.ToString()}.dat",File.ModeFlags.WriteRead);//,"keitaidenjuutelefang");
        GD.Print(error);
        if (error == Error.Ok)
        {
            // get the save file data
            var saveFileData = file.GetVar() as Godot.Collections.Dictionary;
            if (saveFileData is null)
            {
                saveFileData = new Godot.Collections.Dictionary();
            }
            // check the last level
            var saveFileDataLastLevel = saveFileData["lastLevel"] as String;
            // if it doesn't exist, add it
            if (saveFileDataLastLevel is null)
            {
                GD.Print("no lastLevel");
                saveFileData.Add("lastLevel",currentLevel);
            } // if it isn't the same, update it
            else if (saveFileDataLastLevel != currentLevel)
            {
                saveFileData["lastLevel"] = currentLevel;
            }
            // check specific level data, if it doesnt exist, add it
            if (saveFileData[currentLevel] is null) {
                saveFileData.Add(currentLevel, CreateSaveLevelData(time,moves));
            }
            else // if it does, see if it's a better score and save
            {
                var currentLevelData = saveFileData[currentLevel] as Godot.Collections.Dictionary;
                var currentLevelTime = currentLevelData["time"] as String;
                var currentLevelMoves = (int)currentLevelData["moves"];
                if (StringToTime(currentLevelTime) >= time && currentLevelMoves >= moves)
                {
                    saveFileData[currentLevel] = CreateSaveLevelData(time,moves);
                }
            }
            file.StoreVar(saveFileData);
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
            var error = file.Open($"user://save{saveFile.ToString()}.dat",File.ModeFlags.Read);//,"keitaidenjuutelefang");
            if (error == Error.Ok)
            {
                currentSave = saveFile;
                var saveFileData = file.GetVar() as Godot.Collections.Dictionary;
                // check the last level
                currentLevel = saveFileData["lastLevel"] as String;
                //GD.Print(currentLevel);
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
                var error = file.Open($"user://save{i.ToString()}.dat",File.ModeFlags.Read);//,"keitaidenjuutelefang");
                if (error == Error.Ok)
                {
                    var saveFileData = file.GetVar() as Godot.Collections.Dictionary;
                    saveFileProgress[i] = saveFileData["lastLevel"] as String;
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
