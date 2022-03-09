using UnityEngine;

public class HUDBounds : MonoBehaviour
{
    private Canvas _canvas;

    public Vector2 _canvasPixelRectMin;
    public Vector2 _canvasPixelRectMax;



    private void Awake()
    {
        _canvas = Get<Canvas>.From(gameObject);

        GetCanvasPixelRectCoordinates();
    }

    private void GetCanvasPixelRectCoordinates()
    {
        if(_canvas != null)
        {
            _canvasPixelRectMin = new Vector3(_canvas.pixelRect.xMin, _canvas.pixelRect.yMin);
            _canvasPixelRectMax = new Vector3(_canvas.pixelRect.xMax, _canvas.pixelRect.yMax);
        }
    }   
}
