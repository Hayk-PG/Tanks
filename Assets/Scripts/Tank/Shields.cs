using UnityEngine;


public class Shields : MonoBehaviour
{
    private GameObject[] _shields = new GameObject[2];   


    private void Awake()
    {
        _shields[0] = transform.GetChild(0).gameObject;
        _shields[1] = transform.GetChild(1).gameObject;
    }

    private void Start() => SetTankShield();

    private void SetTankShield() => Get<PlayerShields>.From(gameObject).SetShield(this);

    public void Activity(int index, bool isActive)
    {
        _shields[index].SetActive(isActive);

        SecondarySoundController.PlaySound(1, isActive ? 0 : 1);
    }
}
