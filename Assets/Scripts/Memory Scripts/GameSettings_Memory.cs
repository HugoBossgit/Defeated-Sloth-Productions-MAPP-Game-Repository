using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings_Memory : MonoBehaviour
{
    private int settings;
    private const int settingsNumber = 2;


    public enum EpairNumber
    {
        NotSet = 0,
        E10Pairs = 10,
        E15Pairs = 15,
    }

    public enum EPuzzleCategories
    {
        NotSet,
        Animals,
        Weapons,
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
        gameSettings = new Settings();
        ResetGameSettings();
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

    public string GetMaterialDirectoryName()
    {
        return "Materials/";
    }

    
}
