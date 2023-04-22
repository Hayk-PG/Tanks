using UnityEngine;

public class ParticlesSound : MonoBehaviour
{
    private enum SoundType { SecondarySoundController, ExplosionsSoundController }

    [SerializeField]
    private SoundType _soundType;

    [SerializeField]
    private int _listIndex, _clipIndex;


    private void Awake()
    {
        switch (_soundType)
        {
            case SoundType.ExplosionsSoundController: ExplosionsSoundController.PlaySound(_listIndex, _clipIndex); break;
            case SoundType.SecondarySoundController: SecondarySoundController.PlaySound(_listIndex, _clipIndex); break; 
        }
    }
}
