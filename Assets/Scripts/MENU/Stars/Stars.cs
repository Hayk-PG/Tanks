using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] _stars;


    public void Display(int visibleStarsLength)
    {
        HideAll();

        for (int i = 0; i < visibleStarsLength; i++)
            _stars[i].SetActive(true);
    }

    private void HideAll()
    {
        foreach (var star in _stars)
            star.SetActive(false);
    }
}
