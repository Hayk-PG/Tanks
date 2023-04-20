using UnityEngine;

public class BomberPropeller : MonoBehaviour
{
    private void FixedUpdate() => transform.Rotate(Vector3.right, -1000 * Time.deltaTime);
}
