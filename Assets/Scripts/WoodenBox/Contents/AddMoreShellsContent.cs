public class AddMoreShellsContent : IWoodBoxContent
{
    public void Use(TankController tankController, Tab_WoodboxContent tab_WoodboxContent)
    {
        PlayerAmmoType playerAmmoType = Get<PlayerAmmoType>.From(tankController.gameObject);

        if (playerAmmoType != null)
        {
            playerAmmoType.GetMoreBulletsFromWoodBox(out bool isDone, out string text);

            if (isDone)
                tab_WoodboxContent.OnContent(Tab_WoodboxContent.Content.Bullet, text);
        }
    }
}
