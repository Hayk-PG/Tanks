using UnityEngine;

public class WoodBox : MonoBehaviour
{
    private enum Content { AddScore, AddHealth, AddWeapon}
    private Tab_WoodboxContent _tabWoodboxContent;
    [SerializeField] private Content _content;
    private int _contentIndex;



    private void Awake()
    {
        _tabWoodboxContent = FindObjectOfType<Tab_WoodboxContent>();
    }

    public void OnContent(int contentIndex, TankController tankController)
    {
        _contentIndex = contentIndex < System.Enum.GetValues(typeof(Content)).Length ? contentIndex : System.Enum.GetValues(typeof(Content)).Length - 1;
        _content = (Content)_contentIndex;

        switch (_content)
        {
            case Content.AddScore: AddPlayerScore(tankController); break;
            case Content.AddHealth: AddPlayerHealth(tankController); break;
            case Content.AddWeapon: AddWeapon(tankController); break;
        }
    }

    private void AddPlayerScore(TankController tankController)
    {
        ScoreController scoreController = Get<ScoreController>.From(tankController.gameObject);

        if(scoreController != null)
        {
            scoreController.GetScoreFromWoodBox(out bool isDone, out string text);

            if (isDone)
                _tabWoodboxContent.OnContent(Tab_WoodboxContent.Content.Score, text);
        }
    }

    private void AddPlayerHealth(TankController tankController)
    {
        HealthController healthController = Get<HealthController>.From(tankController.gameObject);

        if(healthController != null)
        {
            healthController.GetHealthFromWoodBox(out bool isDone, out string text);

            if (isDone)
                _tabWoodboxContent.OnContent(Tab_WoodboxContent.Content.Health, text);
        }
    }

    private void AddWeapon(TankController tankController)
    {
        PlayerAmmoType playerAmmoType = Get<PlayerAmmoType>.From(tankController.gameObject);

        if(playerAmmoType != null)
        {
            playerAmmoType.GetMoreBulletsFromWoodBox(out bool isDone, out string text);

            if (isDone)
                _tabWoodboxContent.OnContent(Tab_WoodboxContent.Content.Bullet, text);
        }
    }
}
