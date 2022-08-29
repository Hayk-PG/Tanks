using UnityEngine;

public class MetalTile : MonoBehaviour
{
    private void Awake()
    {
        SecondarySoundController.PlaySound(1, 2);
    }
}
