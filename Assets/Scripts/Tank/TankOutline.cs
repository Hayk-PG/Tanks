using System.Collections.Generic;
using UnityEngine;


public class TankOutline : MonoBehaviour
{
    private Outline _outline;
    private List<Outline> _outlines = new List<Outline>();
    private PlayerTurn _playerTurn;


    private void Awake()
    {
        _playerTurn = Get<PlayerTurn>.From(gameObject);

        AddOutline();
    }

    private void OnEnable()
    {
        GameSceneObjectsReferences.TurnController.OnTurnChanged += turnState => 
        {
            if (turnState == _playerTurn.MyTurn)
                SetOutlineActive(true);
            else
                SetOutlineActive(false);
        };
    }

    private void OnDisable()
    {
        GameSceneObjectsReferences.TurnController.OnTurnChanged -= turnState =>
        {
            if (turnState == _playerTurn.MyTurn)
                SetOutlineActive(true);
            else
                SetOutlineActive(false);
        };
    }

    private void AddOutline()
    {
        GlobalFunctions.Loop<MeshRenderer>.Foreach(transform.parent.GetComponentsInChildren<MeshRenderer>(), meshRenderer =>
        {
            _outline = meshRenderer.gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineWidth = 1;
            _outline.OutlineColor = Color.white;

            _outlines.Add(_outline);
        });
    }

    private void SetOutlineActive(bool isActive)
    {
        foreach (var outline in _outlines)
            outline.enabled = isActive;
    }
}
