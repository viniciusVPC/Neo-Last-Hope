using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorTrigger : MonoBehaviour
{
    [SerializeField]GameObject target;

    private void OnTriggerStay2D(Collider2D col)
    {
        target = col.gameObject;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
