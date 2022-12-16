using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ExplosiveBarrels : MonoBehaviour, IDestruct
{
    [SerializeField] private Barrel[] _barrels;
    [SerializeField] private GameObject _explosion;
    private BoxCollider _boxCollider;
    private GlobalExplosiveBarrels _globalExplosiveBarrels;
    private Collider[] _colliders;
       
    private bool _isOverlapped;
    private bool _isLaunched;

    public float Health { get; set; }



    private void Awake()
    {
        _boxCollider = Get<BoxCollider>.From(gameObject);
        _globalExplosiveBarrels = FindObjectOfType<GlobalExplosiveBarrels>();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += LaunchBarrel;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= LaunchBarrel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, LaunchBarrel, () => _globalExplosiveBarrels.LaunchBarrelRaiseEvent(transform.position));
    }

    public void Destruct(int damage, int tileParticleIndex)
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, LaunchBarrel, () => _globalExplosiveBarrels.LaunchBarrelRaiseEvent(transform.position));
    }

    public void LaunchBarrel()
    {
        if (!_isLaunched)
        {
            _boxCollider.enabled = false;
            _isLaunched = true;
            StartCoroutine(LaunchBarrelCoroutine());
        }
    }

    private IEnumerator LaunchBarrelCoroutine()
    {
        while (true)
        {
            foreach (var barrel in _barrels)
            {
                if (barrel != null)
                    barrel.LaunchBarrel();

                yield return new WaitForSeconds(0.1f);
            }

            Explode();
        }
    }

    private void LaunchBarrel(EventData eventData)
    {
        if(eventData.Code == EventInfo.Code_LaunchBarrel)
        {
            object[] data = (object[])eventData.CustomData;

            if ((Vector3)data[0] == transform.position)
                LaunchBarrel();
        }
    }

    private void Explode()
    {
        OverlapSphere();
        int l = _colliders.Length >= 2 ? 2 : _colliders.Length;
        for (int i = 0; i < l; i++)
        {
            Get<IDestruct>.From(_colliders[i].gameObject)?.Destruct(200, 0);
            Get<IDamage>.From(_colliders[i].gameObject)?.Damage(46);
        }

        DestroyGameObject();
    }

    private void OverlapSphere()
    {
        if (!_isOverlapped)
        {
            _colliders = Physics.OverlapSphere(transform.position, 0.15f);
            _isOverlapped = true;
        }
    }

    public void DestroyGameObject()
    {
        _explosion.SetActive(true);
        _explosion.transform.SetParent(null);
        ExplosionsSoundController.PlaySound(2, 1);
        Destroy(gameObject);
    }
}
