using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class TileModifyManager : MonoBehaviour
{
    public enum TileModifyType { BuildBasicTiles, ExtendBasicTiles, BuildConcreteTiles, UpgradeToConcreteTiles }
    private TileModifyType _tileModifyType;

    private Tab_Modify _tabModify;

    private TileModifyTabButtonsListener _tileModifyTabButtonsListener;

    private TilesData _tilesData;

    private List<GameObject> foundTiles;

    [SerializeField]
    private TMP_Text _txtScorePrice;

    private int Price { get; set; }

    public bool CanModifyTiles
    {
        get => _tabModify.LocalPlayerScoreController.Score >= Price;
    }

    public class Prices
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public Prices[] NewPrices = new Prices[4]
    {
        new Prices{Name = Names.ModifyGround, Price = 250},
        new Prices{Name = Names.MetalCube, Price = 1000},
        new Prices{Name = Names.MetalGround, Price = 1000},
        new Prices{Name = Names.Bridge, Price = 250}
    };




    private void Awake()
    {
        _tabModify = Get<Tab_Modify>.From(gameObject);

        _tileModifyTabButtonsListener = Get<TileModifyTabButtonsListener>.From(gameObject);

        _tilesData = FindObjectOfType<TilesData>();
    }

    private void OnEnable()
    {
        _tileModifyTabButtonsListener.onBuildBasicTiles += BuildBasicTiles;
        _tileModifyTabButtonsListener.onExtendBasicTiles += ExtendBasicTiles;
        _tileModifyTabButtonsListener.onBuildConcreteTiles += BuildConcreteTiles;
        _tileModifyTabButtonsListener.onUpgradeToConcreteTiles += UpgradeToConcreteTiles;
    }

    private void OnDisable()
    {
        _tileModifyTabButtonsListener.onBuildBasicTiles -= BuildBasicTiles;
        _tileModifyTabButtonsListener.onExtendBasicTiles -= ExtendBasicTiles;
        _tileModifyTabButtonsListener.onBuildConcreteTiles -= BuildConcreteTiles;
        _tileModifyTabButtonsListener.onUpgradeToConcreteTiles -= UpgradeToConcreteTiles;
    }

    private void SetScorePriceText(int score, int price)
    {
        _txtScorePrice.text = score + "/" + price;
    }

    private void BuildBasicTiles()
    {
        SetPrice(NewPrices[0].Price);

        SetTileModifyType(TileModifyType.BuildBasicTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void ExtendBasicTiles()
    {
        SetPrice(NewPrices[3].Price);

        SetTileModifyType(TileModifyType.ExtendBasicTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void BuildConcreteTiles()
    {
        SetPrice(NewPrices[1].Price);

        SetTileModifyType(TileModifyType.BuildConcreteTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void UpgradeToConcreteTiles()
    {
        SetPrice(NewPrices[2].Price);

        SetTileModifyType(TileModifyType.UpgradeToConcreteTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void SetPrice(int price) => Price = price;

    private void SetTileModifyType(TileModifyType tileModifyType) => _tileModifyType = tileModifyType;

    private void StoreFoundTiles(bool canStore, GameObject tile)
    {
        if (canStore && tile != null && Get<TileModifyGUI>.FromChild(tile) != null)
            foundTiles.Add(tile);
    }

    private IEnumerator StartFindTilesAroundPlayer()
    {
        yield return new WaitForSeconds(0.1f);

        FindTilesAroundPlayer();
    }

    public void FindTilesAroundPlayer()
    {
        SetScorePriceText(_tabModify.LocalPlayerScoreController.Score, Price);

        foundTiles = new List<GameObject>();

        foreach (var tile in _tilesData.TilesDict)
        {
            bool haveLeftTilesBeenFound = tile.Key.x <= _tabModify.LocalPlayerTransform.position.x - _tilesData.Size && tile.Key.x >= _tabModify.LocalPlayerTransform.position.x - (_tilesData.Size * 6);
            bool haveRIghtTilesBeenFound = tile.Key.x >= _tabModify.LocalPlayerTransform.position.x + _tilesData.Size && tile.Key.x <= _tabModify.LocalPlayerTransform.position.x + (_tilesData.Size * 6);
            bool foundNearTiles = tile.Key.x >= _tabModify.LocalPlayerTransform.position.x - (_tilesData.Size * 6) && tile.Key.x <= _tabModify.LocalPlayerTransform.position.x + (_tilesData.Size * 6);

            if (_tileModifyType == TileModifyType.BuildConcreteTiles || _tileModifyType == TileModifyType.BuildBasicTiles)
            {
                StoreFoundTiles(haveLeftTilesBeenFound, tile.Value);
                StoreFoundTiles(haveRIghtTilesBeenFound, tile.Value);
            }

            if (_tileModifyType == TileModifyType.UpgradeToConcreteTiles || _tileModifyType == TileModifyType.ExtendBasicTiles)
            {
                StoreFoundTiles(foundNearTiles, tile.Value);
            }
        }

        foreach (var tile in foundTiles)
        {
            if (!Get<Tile>.From(tile).IsProtected)
                Get<TileModifyGUI>.FromChild(tile).EnableGUI(_tileModifyType);
        }
    }

    public void SubtractScore()
    {
        SetScorePriceText(_tabModify.LocalPlayerScoreController.Score - Price, Price);

        _tabModify.LocalPlayerScoreController.GetScore(-Price, null);

        StartCoroutine(StartFindTilesAroundPlayer());
    }
}
