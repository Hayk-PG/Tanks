using UnityEngine;

public class AirBomberCameraAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;



    public void PlayAnimation(string stateName = "", int layer = 0) => _animator.Play(stateName, layer, 0);

    // Hides the game object by deactivating it.
    // This method is intended to be called at the end of the "Blur" animation.

    public void Hide() => gameObject.SetActive(false);
}
