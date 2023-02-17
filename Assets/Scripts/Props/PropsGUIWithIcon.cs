
public class PropsGUIWithIcon : PropsGUI
{
    protected override void OnTileHealth(float health)
    {
        _animator.Play("PropsGUIIconAnim", 0, 0);
    }
}
