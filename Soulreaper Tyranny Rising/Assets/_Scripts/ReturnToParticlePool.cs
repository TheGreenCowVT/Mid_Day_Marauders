using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReturnToParticlePool : MonoBehaviour
{
    bool returned;

    private void OnEnable()
    {
        returned = false;
    }
    private void OnParticleSystemStopped()
    {
        transform.SetParent(null);
        returned = true;
        ObjectPool.instance.ReturnToPool(gameObject);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (!returned)
        {
            returned = true;
            ObjectPool.instance.ReturnToPool(gameObject);
        }
    }
}
