using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings_Memory : MonoBehaviour
{
    private readonly Dictionary<EPuzzleCategories, string> puzzleCatDirectory = new Dictionary<EPuzzleCategories, string>();
    private int settings;
    private const int settingsNumber = 2;


    public enum EpairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
    }

    public enum EPuzzleCategories
    {
        NotSet,
        Animals,
    }

    public struct Settings
    {
        public EpairNumber PairsNumber;
        public EPuzzleCategories PuzzleCategory;
    }

    private Settings gameSettings;

    public static GameSettings_Memory Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetPuzzleCatDirectory();
        gameSettings = new Settings();
        ResetGameSettings();
    }

    private void SetPuzzleCatDirectory()
    {
        puzzleCatDirectory.Add(EPuzzleCategories.Animals, "Animals");
    }

    public void SetPairNumber(EpairNumber number)
    {
        if(gameSettings.PairsNumber == EpairNumber.NotSet)
        {
            settings++;
        }
        gameSettings.PairsNumber = number;
    }

    public void SetPuzzleCategories(EPuzzleCategories cat)
    {
        if(gameSettings.PuzzleCategory == EPuzzleCategories.NotSet)
        {
            settings++;
        }
        gameSettings.PuzzleCategory = cat;
    }

    public EpairNumber GetPairNumber()
    {
        return gameSettings.PairsNumber;
    }

    public EPuzzleCategories GetPuzzleCategories()
    {
        return gameSettings.PuzzleCategory;
    }

    public void ResetGameSettings()
    {
        settings = 0;
        gameSettings.PuzzleCategory = EPuzzleCategories.NotSet;
        gameSettings.PairsNumber = EpairNumber.NotSet;
    }

    public bool AllSettingsReady()
    {
        return settings == settingsNumber;
    }

    public string GetMaterialDirectoryName()
    {
        return "Materials_Memory/";
    }

    public string GetPuzzleCategoryTextureDirectory()
    {
        if (puzzleCatDirectory.ContainsKey(gameSettings.PuzzleCategory))
        {
            return "Memory Game Graphics/PuzzleCat/" + puzzleCatDirectory[gameSettings.PuzzleCategory] + "/";
        }
        else
        {
            Debug.LogError("Error: cannot get directory name");
            return "";
        }
    }

    
}
