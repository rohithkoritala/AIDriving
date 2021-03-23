using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AIControl : Agent
{
    public float InputX { get; private set; }

    public float InputY { get; private set; }

    [SerializeField] private float offset;
    private Rigidbody rb;
    private Vector3 startingPosition;
    private Quaternion startingRotation;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rb.velocity.magnitude);
        sensor.AddObservation(transform.rotation.eulerAngles.y);
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode.");
        Reset();
    }

    public override void Heuristic(float[] actionsOut)
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.W)) x = 1;
        if (Input.GetKey(KeyCode.S)) x = 2;
        if (Input.GetKey(KeyCode.D)) y = 1;
        if (Input.GetKey(KeyCode.A)) y = 2;

        actionsOut[0] = x;
        actionsOut[1] = y;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        InputX = 0;
        InputY = 0;
        if (vectorAction[0] == 1) InputY = 1;
        if (vectorAction[0] == 2) InputY = -1;
        if (vectorAction[1] == 1) InputX = 1;
        if (vectorAction[1] == 2) InputX = -1;

        float maxSpeed = InputY >= 0 ? 15 : 8;
        float speed = InputY >= 0 ? 25 : 20;

        if (rb.velocity.magnitude < maxSpeed) rb.AddRelativeForce(new Vector3(0,0,InputY) * speed);

        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, InputX * Time.fixedDeltaTime * 150, 0));

        if (rb.velocity.magnitude < .1f)
        {
            Debug.Log("Slow poke.");
            AddReward(-.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            float targetAngle = other.transform.rotation.eulerAngles.y;
            float angle = transform.rotation.eulerAngles.y - offset;
            
            var localVel = transform.InverseTransformDirection(rb.velocity).normalized;

            float direction = localVel.z > 0 ? 0 : 180;

            if (Mathf.Abs(Mathf.DeltaAngle(angle + direction, targetAngle)) <= 80)
            {
                Debug.Log("Rewarded.");
                AddReward(.5f);
            }
            else
            {
                Debug.Log("Punished.");
                AddReward(-.8f);
            }
        }

        if (other.gameObject.layer == 7)
        {
            AddReward(-1);
            EndEpisode();
        } 
    }

    private void OnTriggerExit(Collider other)
    {
       if (other.gameObject.layer == 6)
       {
            var localVel = transform.InverseTransformDirection(rb.velocity).normalized;

            bool direction = localVel.z > 0;

            if (!direction)
            {
                Debug.Log("Punished2.");
                AddReward(-.4f);
            }
       }
    }

    private void Reset()
    {
        SetReward(0);
        InputX = 0;
        InputY = 0;

        GetComponent<Rigidbody>().velocity = Vector3.zero;

        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }
}
