using UnityEngine;

public class Loading : MonoBehaviour
{
    private static Loading _inst;

    [SerializeField]
    private GameObject _loadingGroup;


    private void Awake()
    {
        _inst = this;
    }

    public static void Activity(bool isActive)
    {
        _inst?._loadingGroup.SetActive(isActive);
    }
}
