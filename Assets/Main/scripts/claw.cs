using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class claw : MonoBehaviour
{
    private GathererAgent gatherer;
    private void Start()
    {
        gatherer = GetComponentInParent<GathererAgent>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Package"))
        {
            gatherer.clawCollision(other.gameObject);
        }
    }
}
