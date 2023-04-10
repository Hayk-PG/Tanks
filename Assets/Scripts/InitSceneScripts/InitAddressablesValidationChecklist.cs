using UnityEngine;

public class InitAddressablesValidationChecklist : MonoBehaviour
{
    private bool _isSoundControllerLoaderValid;
    private bool _isAddressableTileValid;
    private bool _isTmpFontValid;



    public void CheckValidation(bool? isSoundControllerLoaderValid, bool? isAddressableTileValid, bool? isTmpFontValid)
    {
        if (isSoundControllerLoaderValid.HasValue)
            _isSoundControllerLoaderValid = isSoundControllerLoaderValid.Value;

        if (isAddressableTileValid.HasValue)
            _isAddressableTileValid = isAddressableTileValid.Value;

        if (isTmpFontValid.HasValue)
            _isTmpFontValid = isTmpFontValid.Value;

        print($"isSoundControllerLoaderValid: {_isSoundControllerLoaderValid}, isAddressableTileValid: {_isAddressableTileValid}, isTmpFontValid: {_isTmpFontValid}");

        if (_isSoundControllerLoaderValid && _isAddressableTileValid && _isTmpFontValid)
            MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}
