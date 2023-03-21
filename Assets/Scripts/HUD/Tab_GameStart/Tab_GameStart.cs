using System.Collections;
using UnityEngine;

public class Tab_GameStart : MonoBehaviour
{
    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStarted;

    private void OnGameStarted() => StartCoroutine(Hide());

    private IEnumerator Hide()
    {
        yield return new WaitForSeconds(1);

        gameObject.SetActive(false);
    }
}
