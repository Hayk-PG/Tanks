using UnityEngine;

public class HitTextManager : MonoBehaviour
{
    public enum TextType { None, Hint, Warning, Hit, CriticalHit, HitCombo, Damage, CriticalDamage, CriticalStun, FatalStun }

    [SerializeField] 
    private HitText[] _hitTexts;

    [SerializeField] [Space]
    private Sprite _sprtHint;

    [SerializeField] 
    private Sprite _sprtWarning;

    [SerializeField] 
    private Sprite _sprtHit;

    [SerializeField] 
    private Sprite _sprtCriticalHit;

    [SerializeField] 
    private Sprite _sprtHitCombo;

    [SerializeField] 
    private Sprite _sprtDamage;

    [SerializeField] 
    private Sprite _sprtCriticalDamage;




    public void Display(TextType textType, string other)
    {
        foreach (var hitText in _hitTexts)
        {
            if (!hitText.gameObject.activeInHierarchy)
            {
                UnhideHitText(hitText);
                SetComboHitTextAsFirstSibling(textType, hitText);
                DisplayHitText(textType, hitText, other);

                break;
            }
        }
    }

    private void UnhideHitText(HitText hitText) => hitText.gameObject.SetActive(true);

    private void SetComboHitTextAsFirstSibling(TextType textType, HitText hitText)
    {
        if (textType == TextType.HitCombo)
            hitText.transform.SetAsFirstSibling();
    }

    private void DisplayHitText(TextType textType, HitText hitText, string other) => hitText.Display(Icon(textType), Text(textType, other));

    private Sprite Icon(TextType textType)
    {
        return textType == TextType.Hint ? _sprtHint : textType == TextType.Warning ? _sprtWarning : textType == TextType.Hit ? _sprtHit : textType == TextType.CriticalHit ? _sprtCriticalHit :
               textType == TextType.HitCombo? _sprtHitCombo: textType == TextType.Damage ? _sprtDamage : textType == TextType.CriticalDamage || textType == TextType.CriticalStun || 
               textType == TextType.FatalStun? _sprtCriticalDamage : null;
    }

    private string Text(TextType textType, string other)
    {
        string txt = textType == TextType.Hint ? "" : textType == TextType.Warning ? "" : textType == TextType.Hit ? "hit" : textType == TextType.CriticalHit ? "critical hit" :
                     textType == TextType.HitCombo ? "hit combo" : textType == TextType.Damage ? "damage" : textType == TextType.CriticalDamage ? "critical damage" :
                     textType == TextType.CriticalStun ? "critical stun": textType == TextType.FatalStun? "fatal stun": null;

        return txt + " " + other;
    }
}
