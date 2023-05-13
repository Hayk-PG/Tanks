using UnityEngine;


public class PlayerFeedback : MonoBehaviour
{
    [SerializeField] [Space]
    private Tab_HitText _tabHitText;





    public HitTextManager CurrentHitTextManager(string tankName)
    {
        return tankName == Names.Tank_FirstPlayer ? _tabHitText.HitTexManagerForPlayer1 : _tabHitText.HitTextManagerForPlayer2;
    }

    public void DisplayDropBoxItemText(string tankname, string text) => DisplayHitText(CurrentHitTextManager(tankname), HitTextManager.TextType.Hint, text);

    public void DisplayDamageText(string tankName, int damage)
    {
        HitTextManager.TextType textType = damage <= 15 ? HitTextManager.TextType.Damage : damage > 15 && damage < 30 ? HitTextManager.TextType.CriticalDamage :
                                               damage >= 30 && damage < 40 ? HitTextManager.TextType.CriticalStun : damage >= 40 ? HitTextManager.TextType.FatalStun : HitTextManager.TextType.None;

        string colorCode = textType == HitTextManager.TextType.Damage ? "#D45719" : textType == HitTextManager.TextType.CriticalDamage ? "#EB4C28" :
                           textType == HitTextManager.TextType.CriticalStun ? "#D42019" : textType == HitTextManager.TextType.FatalStun ? "#F61E74" : "#F6861E";

        DisplayHitText(CurrentHitTextManager(tankName), textType, GlobalFunctions.TextWithColorCode(colorCode, (-damage).ToString()));
    }

    public void OnHitEnemy(string tankName, int playerHitsIndex, int[] scores)
    {
        int total = 0;

        GlobalFunctions.Loop<int>.Foreach(scores, value => { total += value; });

        Conditions<bool>.Compare(playerHitsIndex < 3, () => OnSingleHit(tankName, total), () => OnBackToBackHit(tankName, playerHitsIndex, total));
    }

    private void OnSingleHit(string tankName, int total)
    {
        for (int i = 0; i < SoundController.Instance.SoundsList[1]._clips.Length; i++)
        {
            if (SoundController.Instance.SoundsList[1]._clips[i]._score >= total)
            {
                DisplayHitText(CurrentHitTextManager(tankName), HitTextManager.TextType.Hit, "");

                break;
            }
        }
    }

    private void OnBackToBackHit(string tankName, int playerHitsIndex, int total)
    {
        for (int i = 0; i < SoundController.Instance.SoundsList[2]._clips.Length; i++)
        {
            if (SoundController.Instance.SoundsList[2]._clips[i]._score >= playerHitsIndex)
            {
                GetComboScore(tankName, total, i);

                DisplayHitText(CurrentHitTextManager(tankName), HitTextManager.TextType.HitCombo, GlobalFunctions.BlueColorText("+" + (playerHitsIndex - 2)));

                break;
            }
        }
    }

    private void GetComboScore(string tankName, int total, int index)
    {
        float a = SoundController.Instance.SoundsList[2]._clips[index]._score;
        float b = a * 0.1f;
        float c = (total * b);

        ScoreController sc = GlobalFunctions.ObjectsOfType<ScoreController>.Find(s => s.gameObject.name == tankName);
        sc?.GetScore(Mathf.RoundToInt(c), null, Vector3.zero);
    }

    public void DisplayWeaponChangeText(string tankName, string weaponType)
    {
        DisplayHitText(CurrentHitTextManager(tankName), HitTextManager.TextType.Hint, weaponType);
    }

    private void DisplayHitText(HitTextManager hitTextManager, HitTextManager.TextType textType, string text)
    {
        hitTextManager.Display(textType, text);
    }
}
