using System.Collections;
using UnityEngine;

public class AiChangeTileToArmoredTile : PlayerChangeTileToMetalGround
{
    private ScoreController _scoreController;
    private ChangeTiles _changeTiles;
    private bool _canUseAgain = true;


    protected override void Awake()
    {
        base.Awake();
        _scoreController = Get<ScoreController>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
    }

    protected override void Start()
    {
        _relatedPropsTypeButton = GlobalFunctions.ObjectsOfType<AmmoTypeButton>.Find(props => props._properties.SupportOrPropsType == Names.MetalGround);
    }

    protected override void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    protected override void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    protected virtual void OnTurnChanged(TurnState turnState)
    {
        if (turnState == _playerTurn.MyTurn)
        {
            if (_scoreController.Score >= _relatedPropsTypeButton._properties.RequiredScoreAmmount && _canUseAgain)
            {
                InstantiateHelper(out bool isPlayer1, out Vector3 transformPosition);

                foreach (var tile in _tilesData.TilesDict)
                {
                    if (tile.Value != null)
                    {
                        if (IsTileFound(tile.Key.x, isPlayer1, transformPosition) && tile.Value.GetComponent<TileProps>() != null)
                        {
                            if (!tile.Value.GetComponent<Tile>().IsProtected && !_changeTiles.HasTile(tile.Key - Vector3.up))
                            {
                                Result(isPlayer1, transformPosition, tile.Key);
                                _scoreController.Score -= _relatedPropsTypeButton._properties.RequiredScoreAmmount;
                                StartCoroutine(RunPropsTimerCoroutine());
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    private IEnumerator RunPropsTimerCoroutine()
    {
        _canUseAgain = false;
        float t = (_relatedPropsTypeButton._properties.Minutes * 60) + _relatedPropsTypeButton._properties.Seconds;
        yield return new WaitForSeconds(t);
        _canUseAgain = true;
    }
}
