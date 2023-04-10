using UnityEngine;
using TMPro;
using UnityEngine.UI;


[ExecuteInEditMode]
public class FontResetter : MonoBehaviour
{
    [SerializeField]
    private TMP_FontAsset _font;

    [SerializeField] [Space]
    private bool _executeTmpFonts, _checkTmpFonts;

    [SerializeField] [Space]
    private bool _executeBasicFonts, _checkBasicFonts;

    [SerializeField] [Space]
    private int _iterationCount;




    private void Update()
    {
        if (_executeTmpFonts)
        {
            _iterationCount = 0;

            GlobalFunctions.Loop<TMP_Text>.Foreach(FindObjectsOfType<TMP_Text>(true), tmpText => 
            {
                if (tmpText.name != "")
                {
                    tmpText.font = _font;

                    _iterationCount++;
                }
            });

            _executeTmpFonts = false;
        }

        if (_checkTmpFonts)
        {
            _iterationCount = 0;

            GlobalFunctions.Loop<TMP_Text>.Foreach(FindObjectsOfType<TMP_Text>(true), tmpText =>
            {
                if (tmpText.font != _font)
                    _iterationCount++;
            });

            _executeTmpFonts = false;

            _checkTmpFonts = false;
        }

        if(_checkBasicFonts)
        {
            GlobalFunctions.Loop<Text>.Foreach(FindObjectsOfType<Text>(true), text =>
            {
                print($"Name: {text.name} / {text.font}");

                _iterationCount++;
            });

            _checkBasicFonts = false;
        }
    }
}
