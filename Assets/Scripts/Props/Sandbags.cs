using UnityEngine;

public class Sandbags : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        print(other.name);
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    public void SandbagsDirection(bool isPlayer1)
    {
        transform.localEulerAngles = new Vector3(0, isPlayer1 ? 90 : -90, 0);
    }
}
