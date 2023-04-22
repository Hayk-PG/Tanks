using UnityEngine;
using System;
using TMPro;

public class TmpFontToTmpTextAssigner : MonoBehaviour
{
    [Serializable]
    private struct TmpText
    {
        public TMP_Text[] _txts;
        public int _fontIndex;
    }

    #region Tooltip
    [Tooltip(
        "0: BlackOpsOne-Regular SDF \n" +
        "1: Filepile SDF \n" +
        "2: LiberationSans SDF \n" +
        "3: LiberationSans SDF \n" +
        "4: NexaRustExtras-Free SDF \n" +
        "5: NexaRustHandmade-Extended \n" +
        "6: NexaRustSans-Black SDF \n" +
        "7: NexaRustScriptL-0 SDF \n" +
        "8: NexaRustSlab-BlackShadow01 SDF \n" +
        "9: SparTakus SDF \n" +
        "10: Tokeely_Brookings SDF \n" +
        "11: Tokeely_Brookings_Schatten SDF" )]
    #endregion

    [SerializeField]
    private TmpText[] _tmpTexts;


    private void Awake()
    {
        foreach (var tmpText in _tmpTexts)
            foreach (var txts in tmpText._txts)
                txts.font = TmpFonts.Loader.Asset[tmpText._fontIndex].OperationHandle.Result as TMP_FontAsset;
    }
}
