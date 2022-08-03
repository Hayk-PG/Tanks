using UnityEngine;

public enum SingleGameDifficultyLevel { Easy, Normal, Hard}

public partial class Data : MonoBehaviour
{
    [Header("Difficulty Level")]
    [SerializeField] private SingleGameDifficultyLevel _singleGameDifficultyLevel;

    public SingleGameDifficultyLevel SingleGameDifficultyLevel
    {
        get => _singleGameDifficultyLevel;
    }




    public void SetDifficultyLevel(int difficultyLevel)
    {
        _singleGameDifficultyLevel = (SingleGameDifficultyLevel)difficultyLevel;
        SetData(new NewData { DifficultyLevel = difficultyLevel });
    }
}
