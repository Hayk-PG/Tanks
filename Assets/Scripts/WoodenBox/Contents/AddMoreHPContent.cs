public class AddMoreHPContent : IWoodBoxContent
{
    public void Use(TankController tankController, Tab_WoodboxContent tab_WoodboxContent)
    {
        HealthController healthController = Get<HealthController>.From(tankController.gameObject);

        if (healthController != null)
        {
            healthController.GetHealthFromWoodBox(out bool isDone, out string text);

            if (isDone)
                tab_WoodboxContent.OnContent(Tab_WoodboxContent.Content.Health, text);
        }
    }
}
