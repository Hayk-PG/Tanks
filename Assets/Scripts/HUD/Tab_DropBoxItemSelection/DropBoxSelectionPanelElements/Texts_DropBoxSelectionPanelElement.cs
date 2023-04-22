using UnityEngine;
using TMPro;

public class Texts_DropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField] [Space]
    private TMP_Text _txtTitle, _txtUsageFrequency, _txtAbility;

    public TMP_Text Title => _txtTitle;
    public TMP_Text UsageFrequency => _txtUsageFrequency;
    public TMP_Text Ability => _txtAbility;
}
