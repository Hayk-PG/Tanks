using UnityEngine;

public class AnnouncerUI : MonoBehaviour
{
    public void OnAnimationEnd() => gameObject.SetActive(false);
}
