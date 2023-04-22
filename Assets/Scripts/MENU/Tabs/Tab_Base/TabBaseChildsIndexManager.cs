using UnityEngine;

public class TabBaseChildsIndexManager : MonoBehaviour
{
    [SerializeField]
    private Transform _group_BackgroundFXs;

    [SerializeField] [Space]
    private Transform _subTab_Upper, _subTab_Bottom;

    [SerializeField] [Space]
    private Transform _tabLoading, _tabTransition;


    private void Awake()
    {
        _group_BackgroundFXs.SetAsFirstSibling();

        _subTab_Upper.SetSiblingIndex(1);

        _subTab_Bottom.SetSiblingIndex(2);

        _tabLoading.SetAsLastSibling();

        _tabTransition.SetAsLastSibling();
    }
}
