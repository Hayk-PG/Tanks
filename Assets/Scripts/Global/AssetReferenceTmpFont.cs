using UnityEngine.AddressableAssets;
using TMPro;
using System;

//ADDRESSABLE
[Serializable]
public class AssetReferenceTmpFont : AssetReferenceT<TMP_FontAsset>
{
    public AssetReferenceTmpFont(string guid) : base(guid)
    {

    }
}
