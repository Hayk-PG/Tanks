using UnityEngine;

public class WoodBox : MonoBehaviour
{
    private enum Content { AddScore, AddHealth}
    [SerializeField] private Content _content;
    private int _contentIndex;



    public void OnContent(int contentIndex, TankController tankController)
    {
        _contentIndex = contentIndex < System.Enum.GetValues(typeof(Content)).Length ? contentIndex : System.Enum.GetValues(typeof(Content)).Length - 1;
        _content = (Content)_contentIndex;

        switch (_content)
        {
            case Content.AddScore: AddPlayerScore(tankController); break;
            case Content.AddHealth: AddPlayerHealth(tankController); break;
        }
    }

    private void AddPlayerScore(TankController tankController)
    {
        ScoreController scoreController = Get<ScoreController>.From(tankController.gameObject);
        scoreController.GetScore(500, null);
    }

    private void AddPlayerHealth(TankController tankController)
    {
        HealthController healthController = Get<HealthController>.From(tankController.gameObject);

        if(healthController.Health + 20 <= 100)
        {
            healthController.Health += 20;
        }
        else
        {
            healthController.Health = 100;
        }
    }
}
