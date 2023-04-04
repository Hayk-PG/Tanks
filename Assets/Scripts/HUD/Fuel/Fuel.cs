using System;
using UnityEngine;
using UnityEngine.UI;

public class Fuel : MonoBehaviour
{
    [SerializeField] 
    private Image _oilFilled;

    private TankMovement _tankMovement;

    private float _oilAmmount;
    private float OilFilledAmmount
    {
        get => _oilAmmount / 1000;
    }



    public Action<bool> OnFuelValue { get; set; }


    private void Awake()
    {
        _oilAmmount = 1000;

        _oilFilled.fillAmount = _oilAmmount / 1000;      
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
        _oilAmmount -= Mathf.Abs(rpm / 2) * Time.deltaTime;

        _oilFilled.fillAmount = OilFilledAmmount;

        OnFuelValue?.Invoke(_oilAmmount > 0);
    }

    private void ResetFuelSlider()
    {
        if (_oilAmmount != 1000)
            _oilAmmount += 5000 * Time.deltaTime;

        if (_oilAmmount > 1000)
            _oilAmmount = 1000;

        _oilFilled.fillAmount = OilFilledAmmount;
    }
}

