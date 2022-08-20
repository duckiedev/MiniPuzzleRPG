using Godot;
using System;
using System.Globalization;

public class Data : Node
{
    public static int gridSize = 16;
    [Export] public SfxTree sfxTree;
    [Export] public MusicTree musicTree;
    [Export] public BitmapFont mainFont;
    public int currentLevel = 0;
    public int nextLevel = 1;
    public int currentSave = 0;
    public String[] saveFileProgress = {"","",""};
    public PackedScene pauseScreenScene;
    public PauseScene pauseScreen;
    public Boolean newGame = true;

    [Export] public String[] levelArr = {"1-1","0-2","0-3","0-4","0-5"}; //,"1-1";

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

        /*
        var bgMapRes = ResourceLoader.Load<PackedScene>("res://Main/TilePalette/BGMAP.tscn");
        var bgMap = bgMapRes.Instance();
        Viewport viewPort = bgMap.GetNode<Viewport>("Viewport");
        var tilesetTextureData = viewPort.GetTexture().GetData();
        //var tilesetTexture = new ImageTexture();
        //tilesetTexture.CreateFromImage(tilesetTextureData);
        tilesetTextureData.SavePng("res://GFX/tilesetrendered01.png");
        */

        pauseScreenScene = ResourceLoader.Load<PackedScene>("res://Main/UI/PauseScene.tscn");

        GetSaveGameData();
    }

    public Godot.Collections.Dictionary CreateSaveLevelData(String time, int moves)
    {
        var saveLevelData = new Godot.Collections.Dictionary();
        saveLevelData.Add("time",time);
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
        newGame = true;
        currentLevel = 0;
        nextLevel = 1;
        SaveGame(saveFile,currentLevel,null,-1);
        LoadGame(saveFile);
        newGame = false;
    }

    public void SaveGame(int saveFile, int curLevel, String time, int moves)
    {
        // Create the file if it doesn't exist
        var file = new File();
        if (!file.FileExists($"user://save{saveFile.ToString()}.dat"))
        {
            var newFileError = file.Open($"user://save{saveFile.ToString()}.dat",File.ModeFlags.Write);//,"keitaidenjuutelefang");
            if (newFileError == Error.Ok)
            {
                var saveFileData = new Godot.Collections.Dictionary();
                saveFileData.Add("lastLevel",curLevel);

                for (int i = 0; i < levelArr.Length; i++)
                {
                    saveFileData.Add(i, CreateSaveLevelData(time,moves));
                }
                file.StoreVar(saveFileData);
            }
        }
        file.Close();

        // Read the existing data
        file = new File();
        var error = file.Open($"user://save{saveFile.ToString()}.dat",File.ModeFlags.ReadWrite);//,"keitaidenjuutelefang");
        if (error == Error.Ok)
        {
            // get the save file data
            var saveFileData = file.GetVar() as Godot.Collections.Dictionary;
            // if it doesn't exist, add it
            if ((int)saveFileData["lastLevel"] != nextLevel && !newGame)
            {
                saveFileData["lastLevel"] = nextLevel;
            }

            var currentLevelData = saveFileData[curLevel] as Godot.Collections.Dictionary;
            var currentLevelTime = currentLevelData["time"] as String;
            var currentLevelMoves = (int)currentLevelData["moves"];

            if (currentLevelTime == null && currentLevelMoves == -1)
            {
                saveFileData[curLevel] = CreateSaveLevelData(time,moves);
            } else {
                if (StringToTime(currentLevelTime) >= StringToTime(time) && currentLevelMoves >= moves)
                {
                    saveFileData[curLevel] = CreateSaveLevelData(time,moves);
                }
            }

            file.Seek(0);
            file.StoreVar(saveFileData);

            GetSaveGameData();
            CheckData(saveFile);
        }
        else
        {
            GD.Print("ERROR WITH FILE SAVE");
        }
        file.Close();
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
                currentLevel = (int)saveFileData["lastLevel"];
            }
        }
        file.Close();
    }

    public void CheckData(int saveFile)
    {
        var file = new File();
        if (file.FileExists($"user://save{saveFile.ToString()}.dat"))
        {
            var error = file.Open($"user://save{saveFile.ToString()}.dat",File.ModeFlags.Read);//,"keitaidenjuutelefang");
            if (error == Error.Ok)
            {
                GD.Print("Checking saved data: " + file.GetVar());
            }
        }
        file.Close();
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
                    saveFileProgress[i] = levelArr[(int)saveFileData["lastLevel"]] as String;
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
