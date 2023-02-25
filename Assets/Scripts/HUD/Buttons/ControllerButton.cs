using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControllerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image _img;

    [SerializeField]
    private Color[] _clrs;


    private void Awake() => Get<Image>.From(gameObject).alphaHitTestMinimumThreshold = 1;

    public void OnPointerDown(PointerEventData eventData) => _img.color = _clrs[1];

    public void OnPointerUp(PointerEventData eventData) => _img.color = _clrs[0];
}
