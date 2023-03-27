using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDComponentBuffDebuff : MonoBehaviour
{
    [SerializeField] [Space]
    protected TurnState _turnState;

    [SerializeField][Space]
    private BuffDebuffUIElement[] _elements;



    private void OnEnable()
    {
        BuffDebuffHandler.onBuffDebuffIndicatorActivity += OnActivationBuffDebuffIcon;
    }

    private void OnDisable()
    {
        BuffDebuffHandler.onBuffDebuffIndicatorActivity -= OnActivationBuffDebuffIcon;
    }

    private void OnActivationBuffDebuffIcon(BuffDebuffType buffDebuffType, int hudComponentIndex, object[] data)
    {
        foreach (var element in _elements)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);

                return;
            }
        }
    }
}
