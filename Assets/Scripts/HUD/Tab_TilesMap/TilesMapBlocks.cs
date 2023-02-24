using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TilesMapBlocks : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    [SerializeField] [Space]
    private GridLayoutGroup _gridLayoutGroup;




    private void OnEnable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        float horDistance = Mathf.Abs(GameSceneObjectsReferences.MapPoints.HorizontalMin - GameSceneObjectsReferences.MapPoints.HorizontalMax);

        print(horDistance);
    }
}
