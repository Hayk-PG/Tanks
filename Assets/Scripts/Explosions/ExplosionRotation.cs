using UnityEngine;

public class ExplosionRotation : MonoBehaviour
{
    private void OnEnable ()=> transform.eulerAngles = Vector3.zero;
}
