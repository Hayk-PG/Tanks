public class AddScoreContent : IWoodBoxContent
{
    public void Use(TankController tankController, Tab_WoodboxContent tab_WoodboxContent)
    {
        ScoreController scoreController = Get<ScoreController>.From(tankController.gameObject);

        if(scoreController != null)
        {
            scoreController.GetScoreFromWoodBox(out bool isDone, out string text);

            if (isDone)
                tab_WoodboxContent.OnContent(Tab_WoodboxContent.Content.Score, text);
        }
    }
}
