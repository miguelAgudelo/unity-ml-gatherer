using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class collectorPackageArea : MonoBehaviour
{
    [Tooltip("The agent inside the area")]
    public GameObject GathererAgent;
    [Tooltip("The plataform inside the area")]
    public GameObject plataform;
    [Tooltip("Prefab of a package")]
    public GameObject package;
    [Tooltip("List of a package instantiate")]
    public List<GameObject> packages;
    [Tooltip("List of a walls")]
    public List<GameObject> walls;
    [Tooltip("Wall of firts floor")]
    public GameObject wallF2;
    [Tooltip("Possible positions for the gatherer")]
    public List<Transform> gathererPositions;
    [Tooltip("Possible positions for the gatherer on second floor")]
    public List<Transform> gathererPositionsF2;
    [Tooltip("Possible positions for the gatherer on second floor, only for lesson 3")]
    public List<Transform> gathererPositionsF2Back;
    [Tooltip("list of packeages on first floor")]
    public List<Transform> packagesF1;
    [Tooltip("list of packeages on second floor")]
    public List<Transform> packagesF2;
    //it is only for demo
    [HideInInspector]
    public List<GameObject> packagesF2I;
    //transform Y walls
    public float wallsUp = 0.344f;
    public float wallsDown = -9.73f;
    public GameObject floor1;
    public GameObject floor2;
    public Material material;
    List<Transform> packagesF1Register = new List<Transform>();
    List<Transform> packagesF2Register = new List<Transform>();
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    //Restart training environment with the parameter "Gatherer" which represents the lesson in the curriculum file to generate the necessary changes in the environment
    public void ResetArea(int l = 0)
    {
        floor2.GetComponent<MeshRenderer>().material = material;
        packagesF2Register.Clear();
        floor1.SetActive(false);
        floor2.SetActive(false);
        wallF2.SetActive(false);
        RemoveAllPackage();
        foreach(GameObject w in walls)
            w.transform.localPosition = new Vector3(w.transform.localPosition.x, wallsDown, w.transform.localPosition.z);
        switch(l) {
            //Lesion uno solo 15 paquetes en el primer piso
            case 0:
                SpawnPackage(15, l);
                floor1.SetActive(true);
                break;
            //Lesson one only 15 packs on the first floor
            case 1:
                SpawnPackage(15, l);
                floor2.SetActive(true);
                break;
            //lesson three with 20 packages on the first floor with walls
            case 2:
                SpawnPackage(20, l);
                floor2.SetActive(true);
                foreach (GameObject w in walls)
                    w.transform.localPosition = new Vector3(w.transform.localPosition.x, wallsUp, w.transform.localPosition.z);
                break;
            //Lesson four with 25 packages on the first floor with walls and second floor with fewer packages
            //Wall on floor two to force the gathere down to the first floor at the far end of the platform
            case 3:
                SpawnPackage(25, l);
                floor2.SetActive(true);
                wallF2.SetActive(true);
                foreach (GameObject w in walls)
                    w.transform.localPosition = new Vector3(w.transform.localPosition.x, wallsUp, w.transform.localPosition.z);
                break;
            //lesson four with 25 packages on the first floor with walls and second floor
            case 4:
                SpawnPackage(25, l);
                floor2.SetActive(true);
                foreach (GameObject w in walls)
                    w.transform.localPosition = new Vector3(w.transform.localPosition.x, wallsUp, w.transform.localPosition.z);
                break;
            default:
                break;
        }
        PlaceGatherer(l);
    }
    //Clean the packages area
    private void RemoveAllPackage()
    {
        if (packages != null)
        {
            for (int i = 0; i < packages.Count; i++)
            {
                if (packages[i] != null)
                    Destroy(packages[i]);
            }
        }
        packages = new List<GameObject>();
    }
    //Randomly instantiate the gatherer in the possible positions already defined
    private void PlaceGatherer(int l)
    {
        if (l == 0)
        {
            GathererAgent.transform.position = gathererPositions[Random.Range(0, gathererPositions.Count - 1)].position;
        }
        else if (l == 2)
        {
            GathererAgent.transform.position = gathererPositionsF2[Random.Range(0, gathererPositionsF2.Count - 1)].position;
        }
        else if (l == 3)
        {
            GathererAgent.transform.position = gathererPositionsF2Back[Random.Range(0, gathererPositionsF2Back.Count - 1)].position;
        }
        else
        {
            List<Transform> alldronePosition = new List<Transform>();
            foreach (Transform t in gathererPositions)
                alldronePosition.Add(t);
            foreach (Transform t in gathererPositionsF2)
                alldronePosition.Add(t);
            GathererAgent.transform.position = alldronePosition[Random.Range(0, alldronePosition.Count - 1)].position;
        }
        Rigidbody rigidbody = GathererAgent.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        GathererAgent.transform.rotation = Quaternion.identity;
        GathererAgent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }
    //Random packages positions
    private void SpawnPackage(int count,int l)
    {
        packagesF1Register.Clear();
        foreach (var p1 in packagesF1)
            packagesF1Register.Add(p1);
        for (int i = 0; i < count; i++)
        {
            GameObject pacakageObject = Instantiate(package);
            pacakageObject.transform.position = ChooseRandomPackageF1();
            pacakageObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            pacakageObject.transform.SetParent(transform);
            packages.Add(pacakageObject);
        }
        if (l == 0 || l == 2)
            return;
        packagesF2Register.Clear();
        foreach (var p2 in packagesF2)
            packagesF2Register.Add(p2);
        for (int i = 0; i < 15; i++)
        {
            GameObject pacakageObject = Instantiate(package);
            pacakageObject.transform.position = ChooseRandomPackageF2();
            pacakageObject.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            pacakageObject.transform.SetParent(transform);
            packages.Add(pacakageObject);
            packagesF2I.Add(pacakageObject);
        }
    }
    //Random position floor one
    public Vector3 ChooseRandomPackageF1()
    {
        Transform p = null;
        p = packagesF1Register[Random.Range(0, packagesF1Register.Count-1)];
        packagesF1Register.Remove(p);
        return p.position;
    }
    //Random position floor two
    public Vector3 ChooseRandomPackageF2()
    {
        Transform p = null;
        p = packagesF2Register[Random.Range(0, packagesF2Register.Count - 1)];
        packagesF2Register.Remove(p);
        return p.position;
    }
}
