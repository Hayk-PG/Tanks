using System;
using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    private TankMovement _tankMovement;
    private Slider _slder;
    public Action<bool> OnFuelValue { get; set; }


    private void Awake()
    {
        _slder = Get<Slider>.FromChild(gameObject);
        _slder.value = 1000;
    }

    private void OnDisable()
    {
        if (_tankMovement != null)
            _tankMovement.OnFuel -= OnFuel;
    }

    public void CallPlayerEvents(TankMovement tankMovement)
    {
        _tankMovement = tankMovement;
        _tankMovement.OnFuel += OnFuel;
        _tankMovement.SubscribeToFuelEvents(this);
    }

    private void OnFuel(float rpm, bool isMyTurn)
    {
        Conditions<bool>.Compare(isMyTurn, ()=> DecreaseFuelAmmount(rpm), ResetFuelSlider);
    }

    private void DecreaseFuelAmmount(float rpm)
    {
        _slder.value -= Mathf.Abs(rpm / 2) * Time.deltaTime;
        OnFuelValue?.Invoke(_slder.value > 0);
    }

    private void ResetFuelSlider()
    {
        if (_slder.value != 1000)
        {
            _slder.value += 5000 * Time.deltaTime;
        }
    }
}

