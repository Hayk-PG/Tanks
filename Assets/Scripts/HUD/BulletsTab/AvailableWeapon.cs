using UnityEngine;

public class AvailableWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform _layoutGroup;


    public void OnAnimationEnd()
    {
        transform.SetParent(_layoutGroup);        
    }

    public void PlayImpactSoundFX()
    {
        UISoundController.PlaySound(6, 0);
    }
}
