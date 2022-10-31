using System;
using UnityEngine;

public class RateButton : MonoBehaviour
{
    private enum FunctionType { Increase, Decrease}

    [SerializeField] private FunctionType _functionType;
    private bool _isPointerDown;
    private float _pointerDownTime = 0;
    private int _pointerDownTimeMultiplier = 10;
    private int _number = 1;

    public event Action<int> _onSendNumber;


    private void Update()
    {
        if (_isPointerDown)
            OnHoldButton();
    }

    private void SendNumber(int number) => _onSendNumber?.Invoke(_functionType == FunctionType.Decrease ? -number : number);
    private void OnHoldButton()
    {
        _pointerDownTime += _pointerDownTimeMultiplier * Time.deltaTime;

        if (_pointerDownTime >= 1)
        {
            _pointerDownTime = 0;
            _pointerDownTimeMultiplier++;
            SendNumber(_number);
        }
    }

    private void OnReleaseButton()
    {
        _pointerDownTime = 0;
        _pointerDownTimeMultiplier = 10;
    }

    public virtual void ClickButton() => SendNumber(_number);
    public virtual void HoldButton() => _isPointerDown = true;

    public virtual void ReleaseButton()
    {
        _isPointerDown = false;
        OnReleaseButton();
    }
}
