using UnityEngine.EventSystems;

public class Slider_Sound : Toggle_Sound
{
    protected bool _isPointerDown;


    protected override void Awake()
    {
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        _isPointerDown = false;
    }

    public void OnValueChanged()
    {
        if (_isPointerDown)
            UISoundController.PlaySound(_listIndex, _clipsIndexes[0]);
    }
}
