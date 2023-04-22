#if UNITY_EDITOR
using UnityEngine;
using TMPro;
using System;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;


[CreateAssetMenu(menuName = "Scriptable objects/Prefabs resetter/Fonts resetter")]
public class PrefabsFontResetter : ScriptableObject
{
    public Transform[] t;

    [Space]
    public TMP_FontAsset _font;

    public int _iterationCount;


    public void ResetFonts()
    {
        _iterationCount = 0;

        foreach (var prefab in t)
        {
            TMP_Text prefabTmpText = prefab.GetComponent<TMP_Text>();
            TMP_Text[] childsTmpText = prefab.GetComponentsInChildren<TMP_Text>(true);

            if (prefabTmpText != null)
            {
                if (prefabTmpText.name != "")
                {
                    prefabTmpText.font = _font;

                    CountIteration();

                    EditorUtility.SetDirty(this);
                }
            }

            foreach (var childTmpText in childsTmpText)
            {
                if(prefab.name != "")
                {
                    childTmpText.name = childTmpText.name;
                }

                childTmpText.font = _font;

                CountIteration();

                EditorUtility.SetDirty(this);
            }
        }
    }

    public void CountIteration()
    {
        _iterationCount++;

        GlobalFunctions.DebugLog($"Iteration: {_iterationCount}");
    }
}
#endif
