using UnityEngine;

public enum SingleGameDifficultyLevel { Easy, Normal, Hard}
public enum GameWind { On, Off}

public partial class Data : MonoBehaviour
{
    [Header("Difficulty Level")]
    [SerializeField]  private SingleGameDifficultyLevel _singleGameDifficultyLevel;
    [Header("Game Wind")]
    [SerializeField] private GameWind _gameWind;
    [SerializeField] private bool _isWindOn;
    [Header("Game Time")]
    [SerializeField] private int _gameTime;
    [Header("Map Index")]
    [SerializeField] private int _mapIndex;

    public SingleGameDifficultyLevel SingleGameDifficultyLevel
    {
        get => _singleGameDifficultyLevel;
        private set => _singleGameDifficultyLevel = value;
    }
    public GameWind GameWind
    {
        get => _gameWind;
        private set => _gameWind = value;
    }
    public bool IsWindOn
    {
        get => _isWindOn;
        set => _isWindOn = value;
    }
    public int[] GameTimeVariations { get; private set; } = new int[] { 10, 25, 30, 35, 45 };
    public int GameTime 
    {
        get => _gameTime;
        set => _gameTime = value;
    }
    public int MapIndex
    {
        get => _mapIndex;
        set => _mapIndex = value;
    }


    public void SetDifficultyLevel(int difficultyLevel)
    {
        SingleGameDifficultyLevel = (SingleGameDifficultyLevel)difficultyLevel;
        SetData(new NewData { DifficultyLevel = difficultyLevel });
    }

    public void SetWind(int wind)
    {
        GameWind = (GameWind)wind;
        SetData(new NewData { IsWindToggleOn = wind });
        IsWindOn = GameWind == GameWind.On ? true : false;
    }

    public void SetGameTime(int gameTime)
    {
        if(gameTime < GameTimeVariations.Length)
            GameTime = GameTimeVariations[gameTime];

        SetData(new NewData { GameTime = gameTime });
    }
}
