using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//ADDRESSABLE
public class TmpFonts : MonoBehaviour
{
    public static TmpFonts Loader { get; private set; }

    [SerializeField]
    private List<AssetReferenceTmpFont> _assetReferenceTmpFonts;

    /// <summary>
    /// 0: BlackOpsOne-Regular SDF <br/>
    /// 1: Filepile SDF <br/>
    /// 2: LiberationSans SDF - Fallback <br/>
    /// 3: LiberationSans SDF <br/>
    /// 4: NexaRustExtras-Free SDF <br/>
    /// 5: NexaRustHandmade-Extended SDF <br/>
    /// 6: NexaRustSans-Black SDF <br/>
    /// 7: NexaRustScriptL-0 SDF <br/>
    /// 8: NexaRustSlab-BlackShadow01 SDF <br/>
    /// 9: SparTakus SDF <br/>
    /// 10: Tokeely_Brookings SDF <br/>
    /// 11: Tokeely_Brookings_Schatten SDF <br/>
    /// 12: Electrolize-Regular_Extended ASCII SDF <br/>
    /// 13: NEXON Football Gothic B SDF <br/>
    /// 14: NEXON Football Gothic L SDF <br/>
    /// 15: Oxanium-ExtraBold_Extended ASCII SDF <br/>
    /// 16: Ubuntu-Bold_Extended ASCII SDF <br/>
    /// 17: Ubuntu-Medium_Extended ASCII SDF <br/>
    /// 18: Ubuntu-Regular_Extended ASCII SDF
    /// </summary>
    public List<AssetReferenceTmpFont> Asset => _assetReferenceTmpFonts;

    [SerializeField] [Space]
    private InitAddressablesValidationChecklist _validationChecklist;




    private void Awake()
    {
        if (Loader == null)
        {
            Loader = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() => StartCoroutine(RunIteration());

    private IEnumerator RunIteration()
    {
        for (int i = 0; i < _assetReferenceTmpFonts.Count; i++)
        {
            if (_assetReferenceTmpFonts[i].OperationHandle.IsValid())
                continue;

            yield return _assetReferenceTmpFonts[i].LoadAssetAsync().IsDone;
        }

        yield return null;

        _validationChecklist.CheckValidation(null, null, true);
    }
}
