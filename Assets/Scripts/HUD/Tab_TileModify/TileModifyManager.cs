using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class TileModifyManager : MonoBehaviour, IRequiredPointsManager
{
    public enum TileModifyType { BuildBasicTiles, ExtendBasicTiles, BuildConcreteTiles, UpgradeToConcreteTiles }

    private TileModifyType _tileModifyType;

    [SerializeField]
    private Tab_Modify _tabModify;

    [SerializeField] [Space]
    private TileModifyTabButtonsListener _tileModifyTabButtonsListener;

    private List<GameObject> foundTiles;

    [SerializeField] [Space]
    private TMP_Text _txtScorePrice;



    private int RequiredPoints { get; set; }

    public bool CanModifyTiles
    {
        get => _tabModify.LocalPlayerScoreController.Score >= RequiredPoints;
    }

    public class Points
    {
        public string Name { get; set; }
        public int RequiredPoints { get; set; }
    }

    // Default values
    // Will be used if the SetRequiredCost method has not been called.

    public Points[] NewPoints = new Points[4]
    {
        new Points{Name = Names.ModifyGround, RequiredPoints = 250},
        new Points{Name = Names.MetalCube, RequiredPoints = 1000},
        new Points{Name = Names.MetalGround, RequiredPoints = 1000},
        new Points{Name = Names.Bridge, RequiredPoints = 250}
    };







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

    public void SetRquiredCost(int tileModiferCostPercentage, int armoredCubeCostPercentage, int armoredTileCostPercentage, int tileExtenderCostPercentage)
    {
        NewPoints = new Points[]
        {
            new Points{Name = Names.ModifyGround, RequiredPoints = 250 / 100 * tileModiferCostPercentage},
            new Points{Name = Names.MetalCube, RequiredPoints = 1000 / 100 * armoredCubeCostPercentage},
            new Points{Name = Names.MetalGround, RequiredPoints = 1000 / 100 * armoredTileCostPercentage},
            new Points{Name = Names.Bridge, RequiredPoints = 250 / 100 * tileExtenderCostPercentage}
        };
    }

    private void UpdateScoreUI(int playerPoints, int requiredPoints)
    {
        _txtScorePrice.text = playerPoints + "/" + requiredPoints;
    }

    private void BuildBasicTiles()
    {
        SetRequiredPoints(NewPoints[0].RequiredPoints);

        SetTileModifyType(TileModifyType.BuildBasicTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void ExtendBasicTiles()
    {
        SetRequiredPoints(NewPoints[3].RequiredPoints);

        SetTileModifyType(TileModifyType.ExtendBasicTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void BuildConcreteTiles()
    {
        SetRequiredPoints(NewPoints[1].RequiredPoints);

        SetTileModifyType(TileModifyType.BuildConcreteTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void UpgradeToConcreteTiles()
    {
        SetRequiredPoints(NewPoints[2].RequiredPoints);

        SetTileModifyType(TileModifyType.UpgradeToConcreteTiles);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void SetRequiredPoints(int requiredPoints) => RequiredPoints = requiredPoints;

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
        UpdateScoreUI(_tabModify.LocalPlayerScoreController.Score, RequiredPoints);

        foundTiles = new List<GameObject>();

        foreach (var tile in GameSceneObjectsReferences.TilesData.TilesDict)
        {
            bool haveLeftTilesBeenFound = tile.Key.x <= _tabModify.LocalPlayerTransform.position.x - GameSceneObjectsReferences.TilesData.Size && tile.Key.x >= _tabModify.LocalPlayerTransform.position.x - (GameSceneObjectsReferences.TilesData.Size * 6);
            bool haveRIghtTilesBeenFound = tile.Key.x >= _tabModify.LocalPlayerTransform.position.x + GameSceneObjectsReferences.TilesData.Size && tile.Key.x <= _tabModify.LocalPlayerTransform.position.x + (GameSceneObjectsReferences.TilesData.Size * 6);
            bool foundNearTiles = tile.Key.x >= _tabModify.LocalPlayerTransform.position.x - (GameSceneObjectsReferences.TilesData.Size * 6) && tile.Key.x <= _tabModify.LocalPlayerTransform.position.x + (GameSceneObjectsReferences.TilesData.Size * 6);

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

    public void SubtractScore(Vector3? position = null)
    {
        IncrementRequiredPoints(50);

        UpdateScoreUI(_tabModify.LocalPlayerScoreController.Score - RequiredPoints, RequiredPoints);

        DeductPointsFromPlayer(position);

        StartCoroutine(StartFindTilesAroundPlayer());
    }

    private void DeductPointsFromPlayer(Vector3? position = null) => _tabModify.LocalPlayerScoreController.GetScore(-RequiredPoints, null, null, position);

    public void IncrementRequiredPoints(int amount = 0)
    {
        switch (_tileModifyType)
        {
            case TileModifyType.BuildBasicTiles:

                NewPoints[0].RequiredPoints += amount;

                SetRequiredPoints(NewPoints[0].RequiredPoints);

                break;

            case TileModifyType.BuildConcreteTiles:

                NewPoints[1].RequiredPoints += amount;

                SetRequiredPoints(NewPoints[1].RequiredPoints);

                break;

            case TileModifyType.UpgradeToConcreteTiles:

                NewPoints[2].RequiredPoints += amount;

                SetRequiredPoints(NewPoints[2].RequiredPoints);

                break;

            case TileModifyType.ExtendBasicTiles:

                NewPoints[3].RequiredPoints += amount;

                SetRequiredPoints(NewPoints[3].RequiredPoints);

                break;
        }
    }
}
