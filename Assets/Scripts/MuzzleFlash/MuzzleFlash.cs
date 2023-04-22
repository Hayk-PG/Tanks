using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] private int _clipIndex;


    private void Awake() => SecondarySoundController.PlaySound(4, _clipIndex);
}
