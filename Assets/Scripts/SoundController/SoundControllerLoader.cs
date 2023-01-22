using UnityEngine;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class SoundControllerLoader : MonoBehaviour
{
    [SerializeField] private AssetReference _assetReferenceSoundController;


    private void Awake()
    {
        _assetReferenceSoundController.InstantiateAsync().Completed += (asset) => 
        {
            MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
        };
    }
}
