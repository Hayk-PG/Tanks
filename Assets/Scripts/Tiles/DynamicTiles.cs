using System.Collections;
using UnityEngine;

public class DynamicTiles : MonoBehaviour
{
    [SerializeField] [Space]
    private Tile _tile;

    private MeshRenderer _meshRenderer;

    private Color32 _clrNormal = new Color32(123, 26, 0, 255);
    private Color32 _clrRain = new Color32(0, 123, 119, 255);
    private Color32 _clrSnow = new Color32(154, 237, 238, 255);

    private bool _isChangingColor;

    



    private void OnEnable()
    {
        _tile.onMeshInstantiated += OnMeshInstantiated;
        _tile.onDestroyingMesh += OnDestroyingMesh;

        GameSceneObjectsReferences.RainManager.onRainActivity += OnRainActivity;
    }

    private void OnDisable()
    {
        _tile.onMeshInstantiated -= OnMeshInstantiated;
        _tile.onDestroyingMesh -= OnDestroyingMesh;

        GameSceneObjectsReferences.RainManager.onRainActivity -= OnRainActivity;
    }

    private void OnMeshInstantiated(GameObject gameObject)
    {
        _meshRenderer = Get<MeshRenderer>.From(gameObject);

        OnRainActivity(GameSceneObjectsReferences.RainManager.IsRaining);
    }

    private void OnDestroyingMesh()
    {
        _isChangingColor = false;

        GameSceneObjectsReferences.RainManager.onRainActivity -= OnRainActivity;
    }

    private void OnRainActivity(bool isRaining)
    {
        _isChangingColor = false;

        StartCoroutine(ControlMaterialColor(isRaining ? _clrRain: _clrNormal));
    }

    private IEnumerator ControlMaterialColor(Color32 color)
    {
        yield return StartCoroutine(StopChangingColor());

        while (_isChangingColor)
        {
            yield return StartCoroutine(StartChangingColor(color));
        }
    }

    private IEnumerator StopChangingColor()
    {
        yield return null;

        _isChangingColor = true;
    }

    private IEnumerator StartChangingColor(Color32 color)
    {
        if (_meshRenderer == null)
            yield break;

        _meshRenderer.materials[0].color = Color.LerpUnclamped(_meshRenderer.materials[0].color, color, 2 * Time.deltaTime);

        Color32 materialColor = _meshRenderer.materials[0].color;
        Color32 newColor = color;

        if (materialColor.r == newColor.r && materialColor.g == newColor.g && materialColor.b == newColor.b)
            _isChangingColor = false;

        print("Changing the material color...");

        yield return null;
    }
}
