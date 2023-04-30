using System;
using UnityEngine;

public class InitAddressablesValidationChecklist : MonoBehaviour
{
    public event Action onVerifyLoaders;
    public event Action<float> onCheckValidation;




    private void Start() => VerifyLoadersAndNotify();

    // This method raises the onVerifyLoaders event only when CheckValidation has completed successfully and the game has returned to the Init scene to reload the Menu scene.

    private void VerifyLoadersAndNotify()
    {
        bool isValidCheck = SoundControllerLoader.Instance == null || AddressableTile.Loader == null || TmpFonts.Loader == null;

        if (isValidCheck)
            return;

        bool areDependenciesValid = SoundControllerLoader.Instance.IsValid && AddressableTile.Loader.IsValid && TmpFonts.Loader.IsValid;

        if (areDependenciesValid)
            onVerifyLoaders?.Invoke();
    }

    //When the game is first loaded, this method checks if all the Addressables have loaded successfully in order to confirm the validity.

    public void CheckValidation(bool? isSoundControllerLoaderValid, bool? isAddressableTileValid, bool? isTmpFontValid)
    {
        if (isSoundControllerLoaderValid.HasValue)
            RaiseOnCheckValidationEvent(0.34f);

        if (isAddressableTileValid.HasValue)
            RaiseOnCheckValidationEvent(0.34f);

        if (isTmpFontValid.HasValue)
            RaiseOnCheckValidationEvent(0.34f);
    }

    private void RaiseOnCheckValidationEvent(float value) => onCheckValidation?.Invoke(value);
}
