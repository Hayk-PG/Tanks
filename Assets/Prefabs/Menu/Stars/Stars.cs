using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] private GameObject[] _stars;


    public void Display(int visibleStarsLength)
    {
        for (int i = 0; i < visibleStarsLength; i++)
        {
            _stars[i].SetActive(true);
        }
    }
}
