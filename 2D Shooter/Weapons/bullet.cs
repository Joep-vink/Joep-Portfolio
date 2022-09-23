using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class bullet : MonoBehaviour
{
    [field: SerializeField]
    public virtual BulletDataSo BulletData { get; set; }
}
