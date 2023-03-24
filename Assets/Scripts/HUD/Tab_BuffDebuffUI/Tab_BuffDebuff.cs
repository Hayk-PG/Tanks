using UnityEngine;

public class Tab_BuffDebuff : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [Space]

    [SerializeField]
    private BuffDebuffUIElement[] _elements;




    private void OnEnable()
    {
        BuffDebuffHandler.onActivateBuffDebuffIcon += OnActivationBuffDebuffIcon;
    }

    private void OnDisable()
    {
        BuffDebuffHandler.onActivateBuffDebuffIcon -= OnActivationBuffDebuffIcon;
    }

    private void OnActivationBuffDebuffIcon(IBuffDebuffHandler iBuffDebuffHandler, BuffDebuffHandler.BuffDebuffType buffDebuffType, object[] data)
    {
        foreach (var element in _elements)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);
                element.Set();

                return;
            }
        }
    }
}
