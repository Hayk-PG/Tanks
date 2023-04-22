﻿using System.Collections;
using UnityEngine;

public class AiChangeTileToArmoredTile : MonoBehaviour
{
    private ScoreController _scoreController;
    private PlayerTurn _playerTurn;
    private TurnController _turnController;
    private TileModifyManager _tileModifyManager;
    private GlobalTileController _globalTileController;
    private ChangeTiles _changeTiles;
    private TilesData _tilesData;

    private TileProps _tileProps;
    private Tile _tile;
    private bool _canUseAgain = true;


    private void Awake()
    {
        _scoreController = Get<ScoreController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _tileModifyManager = FindObjectOfType<TileModifyManager>();
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
            if (_scoreController.Score >= _tileModifyManager.NewPrices[2].Price && _canUseAgain)
            {
                foreach (var tile in _tilesData.TilesDict)
                {
                    if (tile.Value != null)
                    {
                        if(tile.Key.x >= transform.position.x - 0.5f && tile.Key.x <= transform.position.x + 0.5f)
                        {
                            _tileProps = Get<TileProps>.From(tile.Value);
                            _tile = Get<Tile>.From(tile.Value);

                            if (_tileProps != null && !_tile.IsProtected && !_changeTiles.HasTile(tile.Key - Vector3.up))
                            {                               
                                StartCoroutine(ActivateArmoredTile(_tileProps));
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator ActivateArmoredTile(TileProps tileProps)
    {
        yield return new WaitForSeconds(0.3f);
        tileProps.ActiveProps(TileProps.PropsType.MetalGround, true, null);
        _scoreController.Score -= _tileModifyManager.NewPrices[2].Price;
        _canUseAgain = false;
        yield return new WaitForSeconds(60);
        _canUseAgain = true;
    }
}
