
public class PlayerDamageScreenText : BaseScreenText
{
    public override void Display(int value)
    {
        _text.text = value.ToString();
        base.Display(value);
    }
}
