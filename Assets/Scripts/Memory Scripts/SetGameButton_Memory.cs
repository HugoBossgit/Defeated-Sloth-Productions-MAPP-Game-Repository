using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameButton_Memory : MonoBehaviour
{

    public enum EButtonType
    {
        NotSet,
        PairNumberBtn,
        PuzzleCategoryBtn,
    }

    [SerializeField] public EButtonType buttonType = EButtonType.NotSet;
    [HideInInspector] public GameSettings_Memory.EpairNumber PairNumber = GameSettings_Memory.EpairNumber.NotSet;
    [HideInInspector] public GameSettings_Memory.EPuzzleCategories PuzzleCategories = GameSettings_Memory.EPuzzleCategories.NotSet;

    void Start()
    {
        
    }

    public void SetGameOption(string GameSceneName)
    {
        var comp = gameObject.GetComponent<SetGameButton_Memory>();

        switch (comp.buttonType)
        {
            case SetGameButton_Memory.EButtonType.PairNumberBtn:
                GameSettings_Memory.Instance.SetPairNumber(comp.PairNumber);
                break;
            case EButtonType.PuzzleCategoryBtn:
                GameSettings_Memory.Instance.SetPuzzleCategories(comp.PuzzleCategories);
                break;
        }

        if (GameSettings_Memory.Instance.AllSettingsReady())
        {
            SceneManager.LoadScene(GameSceneName);
        }
    }
}
