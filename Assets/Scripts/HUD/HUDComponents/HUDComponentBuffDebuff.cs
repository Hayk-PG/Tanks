using UnityEngine;

public class HUDComponentBuffDebuff : MonoBehaviour
{
    [SerializeField] [Space]
    protected TurnState _turnState;

    [SerializeField][Space]
    private BuffDebuffUIElement[] _elements;



    private void OnEnable() => BuffDebuffHandler.onBuffDebuffIndicatorActivity += OnActivationBuffDebuffIcon;

    private void OnDisable() => BuffDebuffHandler.onBuffDebuffIndicatorActivity -= OnActivationBuffDebuffIcon;

    private void OnActivationBuffDebuffIcon(BuffDebuffType buffDebuffType, TurnState turnState, IBuffDebuffUIElementController buffDebuffUIElement, object[] data)
    {
        if (turnState != _turnState)
            return;

        foreach (var element in _elements)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);
                element.gameObject.transform.SetAsFirstSibling();
                element.Set(buffDebuffType, buffDebuffUIElement, data);

                return;
            }
        }
    }
}
