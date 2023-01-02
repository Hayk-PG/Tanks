using System.Collections.Generic;
using UnityEngine;

public class AAProjectileArsenal : MonoBehaviour
{
    [SerializeField] private List<BulletController> _missiles;

    public List<BulletController> Missiles => _missiles;
}
