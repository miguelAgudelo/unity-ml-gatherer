using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIController : MonoBehaviour
{
    public collectorPackageArea cpa;
    public Material materialT;
    private void Update()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Package Remaing: " + cpa.packages.Count.ToString();
        if(cpa.floor2.GetComponent<MeshRenderer>().material != materialT && cpa.packagesF2I.Count == 0)
        {
            cpa.floor2.GetComponent<MeshRenderer>().material = materialT;
        }
    }
}
