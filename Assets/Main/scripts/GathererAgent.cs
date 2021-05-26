using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
public class GathererAgent : Agent
{
    [Tooltip("How fast the agent moves forward")]
    public float moveSpeed = 8f;
    [Tooltip("How fast the agent turns")]
    public float turnSpeed = 260f;
    [Tooltip("child that collision and catch pachage object")]
    public GameObject claw;
    [Tooltip("destination object to deliver packages")]
    private GameObject plataform;
    [Tooltip("environment object")]
    private collectorPackageArea packageArea;
    new private Rigidbody rigidbody;
    public bool isLoaded;
    public bool isTraining = true;
    public override void Initialize()
    {
        base.Initialize();
        packageArea = GetComponentInParent<collectorPackageArea>();
        plataform = packageArea.plataform;
        rigidbody = GetComponent<Rigidbody>();
    }
    //Gatherer can rotate on its own axis, go forward, sideways, up and down
    public override void OnActionReceived(float[] vectorAction)
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        float forwardAmount = vectorAction[0];
        float upAmount = 0f;
        float rightAmount = 0f;
        float turnAmount = 0f;
        if (vectorAction[1] == 1f)
        {
            upAmount = 1f;
        }
        else if (vectorAction[1] == 2f)
        {
            upAmount = -1f;
        }
        if (vectorAction[2] == 1f)
        {
            rightAmount = 1f;
        }
        else if (vectorAction[2] == 2f)
        {
            rightAmount = -1f;
        }
        if (vectorAction[3] == 1f)
        {
            turnAmount = 1f;
        }
        else if (vectorAction[3] == 2f)
        {
            turnAmount = -1f;
        }
        Vector3 move = new Vector3(rightAmount, upAmount , forwardAmount);
        rigidbody.AddRelativeForce(move * moveSpeed);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y + turnAmount * Time.fixedDeltaTime * turnSpeed, 0f);
        if (MaxStep > 0) AddReward(-1f / MaxStep);
    }
    public override void Heuristic(float[] actionsOut)
    {
        int forwardAction = 0;
        int turnAction = 0;
        int rightAction = 0;
        int upAction = 0;
        if (Input.GetKey(KeyCode.W))
            forwardAction = 1;
        if (Input.GetKey(KeyCode.E))
        {
            upAction = 2;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            upAction = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rightAction = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rightAction = 2;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnAction = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            turnAction = 2;
        }
        actionsOut[0] = forwardAction;
        actionsOut[1] = upAction;
        actionsOut[2] = rightAction;
        actionsOut[3] = turnAction;
    }
    //Restart the environment passing the value of the lesson number (see.yaml file)
    public override void OnEpisodeBegin()
    {
        isLoaded = false;
        float levelNumber = (isTraining) ? Academy.Instance.EnvironmentParameters.GetWithDefault("Gatherer", 0.0f) : 4f;
        packageArea.ResetArea((int)levelNumber);
    }
    //6 observed values ​​Vector Observation = 6
    public override void CollectObservations(VectorSensor sensor)
    {
        //Whether the Gatherer is loaded (1 float = 1 value)
        sensor.AddObservation(isLoaded);
        //Distance to the plataform (1 float = 1 value)
        sensor.AddObservation(Vector3.Distance(plataform.transform.position, transform.position));
        //Direction to plataform (1 Vector3 = 3 values)
        sensor.AddObservation((plataform.transform.position - transform.position).normalized);
        //cant of packages (1 int = 1 value)
        float f = 0;
        if (GetComponentInParent<collectorPackageArea>() != null){
            if (GetComponentInParent<collectorPackageArea>().packages != null)
                f = (float)GetComponentInParent<collectorPackageArea>().packages.Count / 85.00f;
        }
        sensor.AddObservation(f);
    }
    //Reward for collision between the gatherer's claw and a package
    public void clawCollision(GameObject package)
    {
        if (isLoaded) return;
        isLoaded = true;
        package.transform.parent = transform;
        package.tag = "PackageL";
        Destroy(package.GetComponent<Rigidbody>());
        AddReward(1f);
    }
    //Punishment for collision with the roof
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("roof"))
            AddReward(-1f);
    }
    //Punishment for continuous collision with the roof and walls
    private void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Untagged"))
            AddReward(-.0005f);
        if (collider.CompareTag("roof"))
            AddReward(-.05f);
    }
}
