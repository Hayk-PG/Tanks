using UnityEngine;

public class ExplosiveBarrels : MonoBehaviour
{
    [SerializeField] private Barrel[] _barrels;
    private GlobalExplosiveBarrels _globalExplosiveBarrels;



    private void Awake()
    {
        _globalExplosiveBarrels = FindObjectOfType<GlobalExplosiveBarrels>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, LaunchBarrel, ()=> _globalExplosiveBarrels.LaunchBarrel(transform.position));
    }

    public void LaunchBarrel()
    {
        int random = Random.Range(0, _barrels.Length);
        _barrels[random].LaunchBarrel();
    }
}
