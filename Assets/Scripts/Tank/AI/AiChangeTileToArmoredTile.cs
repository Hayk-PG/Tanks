using System.Collections;
using UnityEngine;

public class AiChangeTileToArmoredTile : MonoBehaviour
{
    private ScoreController _scoreController;
    private PlayerTurn _playerTurn;
    private TurnController _turnController;
    private Tab_TileModify _tab_TileModify;
    private GlobalTileController _globalTileController;
    private ChangeTiles _changeTiles;
    private TilesData _tilesData;
    private bool _canUseAgain = true;


    private void Awake()
    {
        _scoreController = Get<ScoreController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _tab_TileModify = FindObjectOfType<Tab_TileModify>();
        _globalTileController = FindObjectOfType<GlobalTileController>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tilesData = FindObjectOfType<TilesData>();
    }


    private void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if (turnState == _playerTurn.MyTurn)
        {
            if (_scoreController.Score >= _tab_TileModify.NewPrices[2].Price && _canUseAgain)
            {
                foreach (var tile in _tilesData.TilesDict)
                {
                    if (tile.Value != null)
                    {
                        if(tile.Key.x >= transform.position.x - 0.5f && tile.Key.x <= transform.position.x + 0.5f)
                        {
                            TileProps tp = Get<TileProps>.From(tile.Value);
                            Tile t = Get<Tile>.From(tile.Value);

                            if (tp != null && !t.IsProtected && !_changeTiles.HasTile(tile.Key - Vector3.up))
                            {                               
                                StartCoroutine(ActivateArmoredTile());
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ActivateArmoredTile()
    {
        _scoreController.Score -= _tab_TileModify.NewPrices[2].Price;
        _canUseAgain = false;
        yield return new WaitForSeconds(60);
        _canUseAgain = true;
    }
}
