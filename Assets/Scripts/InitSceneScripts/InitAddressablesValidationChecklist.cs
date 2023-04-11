using UnityEngine;

public class InitAddressablesValidationChecklist : MonoBehaviour
{
    private bool _isSoundControllerLoaderValid;
    private bool _isAddressableTileValid;
    private bool _isTmpFontValid;




    private void Awake() => LoadSceneOnAwake();

    private void LoadSceneOnAwake()
    {
        if (SoundControllerLoader.Instance == null || AddressableTile.Loader == null || TmpFonts.Loader == null)
            return;

        LoadScene(SoundControllerLoader.Instance.IsValid && AddressableTile.Loader.IsValid && TmpFonts.Loader.IsValid);
    }

    public void CheckValidation(bool? isSoundControllerLoaderValid, bool? isAddressableTileValid, bool? isTmpFontValid)
    {
        if (isSoundControllerLoaderValid.HasValue)
            _isSoundControllerLoaderValid = isSoundControllerLoaderValid.Value;

        if (isAddressableTileValid.HasValue)
            _isAddressableTileValid = isAddressableTileValid.Value;

        if (isTmpFontValid.HasValue)
            _isTmpFontValid = isTmpFontValid.Value;

        LoadScene(_isSoundControllerLoaderValid && _isAddressableTileValid && _isTmpFontValid);
    }

    private void LoadScene(bool isValid)
    {
        if (isValid)
            MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}
