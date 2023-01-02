using System;
using UnityEngine;

public class TileModifyTabButtonsListener : MonoBehaviour, IReset
{
    [SerializeField] private Btn _btnBuildBasicTiles;
    [SerializeField] private Btn _btnExtendBasicTiles;
    [SerializeField] private Btn _btnBuildConcreteTiles;
    [SerializeField] private Btn _btnUpgradeToConcreteTiles;

    public event Action onBuildBasicTiles;
    public event Action onExtendBasicTiles;
    public event Action onBuildConcreteTiles;
    public event Action onUpgradeToConcreteTiles;


    private void OnEnable()
    {
        _btnBuildBasicTiles.onSelect += BuildBasicTiles;
        _btnExtendBasicTiles.onSelect += ExtendBasicTiles;
        _btnBuildConcreteTiles.onSelect += BuildConcreteTiles;
        _btnUpgradeToConcreteTiles.onSelect += UpgradeToConcreteTiles;
    }

    private void OnDisable()
    {
        _btnBuildBasicTiles.onSelect -= BuildBasicTiles;
        _btnExtendBasicTiles.onSelect -= ExtendBasicTiles;
        _btnBuildConcreteTiles.onSelect -= BuildConcreteTiles;
        _btnUpgradeToConcreteTiles.onSelect -= UpgradeToConcreteTiles;
    }

    private void BuildBasicTiles() => onBuildBasicTiles?.Invoke();

    private void ExtendBasicTiles() => onExtendBasicTiles?.Invoke();

    private void BuildConcreteTiles() => onBuildConcreteTiles?.Invoke();

    private void UpgradeToConcreteTiles() => onUpgradeToConcreteTiles?.Invoke();

    public void SetDefault()
    {
        _btnBuildBasicTiles.Select();
    }
}
