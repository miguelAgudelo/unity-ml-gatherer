using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class plataform : MonoBehaviour
{
    private collectorPackageArea packageArea;
    private void Start()
    {
        packageArea = GetComponentInParent<collectorPackageArea>();
    }
    //Reward for package collision with platform
    //Extra reward if it is the last package
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PackageL"))
        {
            if (packageArea.packagesF2I.Contains(other.gameObject))
                packageArea.packagesF2I.Remove(other.gameObject);
            packageArea.packages.Remove(other.gameObject);
            Destroy(other.gameObject);
            packageArea.GathererAgent.GetComponent<GathererAgent>().isLoaded = false;
            packageArea.GathererAgent.GetComponent<GathererAgent>().AddReward(1f);
            if (packageArea.packages.Count == 0)
            {
                packageArea.GathererAgent.GetComponent<GathererAgent>().AddReward(5f);
                packageArea.GathererAgent.GetComponent<GathererAgent>().EndEpisode();
            }
        }
    }
}
