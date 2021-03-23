using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AIWheels : Agent //This class controls the wheels which power/control the car (engine/steering)
{
    [SerializeField] private GameObject[] checkPoints;
    [SerializeField] private Rigidbody rb_car;

    private int currentCheckPoint = 0;
    private float resetIfCantMove = 5;
    private float timer;
    private MLViewController controller;

    public float WheelAngleX = 0f;
    public float WheelAngleY = 0f;
    public float WheelAngleZ = 0f;

    public float speed; //value to see speed of car. DO NOT EDIT IN VIEWER

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public float MotorTorque = 0;
    public float BrakeTorque = 0;

    public override void Initialize()
    {
        controller = transform.parent.Find("MLView").GetComponent<MLViewController>();
        wheelFL.steerAngle = 0f;
        wheelFR.steerAngle = 0f;

        wheelBL.motorTorque = 0f;
        wheelBR.motorTorque = 0f;
        wheelBL.brakeTorque = 0f;
        wheelBR.brakeTorque = 0f;
    }

    public override void Heuristic(float[] actionsOut)
    {
        float x = 0;
        float y = 0;

        if (Input.GetKey(KeyCode.W)) x = 1;
        if (Input.GetKey(KeyCode.S)) x = -1;
        if (Input.GetKey(KeyCode.D)) y = 1;
        if (Input.GetKey(KeyCode.A)) y = -1;

        actionsOut[0] = x;
        actionsOut[1] = y;
    }

    public override void OnEpisodeBegin()
    {
        Reset();
        base.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var mlViews = controller.GetMLViews();
        if (checkPoints[currentCheckPoint]) sensor.AddObservation((checkPoints[currentCheckPoint].transform.position - transform.position).normalized);
        sensor.AddObservation(1 - (timer / 60f));
        sensor.AddObservation(speed / 60f);
        sensor.AddObservation(transform.parent.up.y);
        
        sensor.AddObservation(mlViews[0].GetMLInput());
        sensor.AddObservation(mlViews[1].GetMLInput());
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        base.OnActionReceived(vectorAction);
        timer += Time.fixedDeltaTime;
        speed = rb_car.velocity.magnitude;

        //**Controls** (will change values to adjustable variables later)
        if(vectorAction[0] > 0){ //Go forward
            wheelBL.motorTorque = MotorTorque * vectorAction[0];
            wheelBR.motorTorque = MotorTorque * vectorAction[0];
            wheelBL.brakeTorque = 0f;
            wheelBR.brakeTorque = 0f;

            if(speed < 5) { //Car go fast when slow until fast enough, then normal speed
                wheelBL.motorTorque = (MotorTorque + (MotorTorque * 5)) * vectorAction[0];
                wheelBR.motorTorque = (MotorTorque + (MotorTorque * 5)) * vectorAction[0];
			}
		}
        if(vectorAction[0] < 0)
        { //Brake
            wheelBL.brakeTorque = BrakeTorque * Mathf.Abs(vectorAction[0]);
            wheelBR.brakeTorque = BrakeTorque * Mathf.Abs(vectorAction[0]);
            
            if(speed < 5) { //if slowed down enough, stop braking and go backwards
                wheelBL.brakeTorque = 0f;
                wheelBR.brakeTorque = 0f;
                wheelBL.motorTorque = (MotorTorque - (MotorTorque * 5)) * Mathf.Abs(vectorAction[0]);
                wheelBR.motorTorque = (MotorTorque - (MotorTorque * 5)) * Mathf.Abs(vectorAction[0]);
			}
            
            
            //wheelBL.brakeTorque = BrakeTorque;
            //wheelBR.brakeTorque = BrakeTorque;
		}
        if(vectorAction[0] == 0){ //If nothing then do nothing
			wheelBL.motorTorque = 0f;
            wheelBR.motorTorque = 0f;
            wheelBL.brakeTorque = 0f;
            wheelBR.brakeTorque = 0f;
		}
        if(vectorAction[1] < 0)
        { //Turn Left
            wheelFL.steerAngle = 20f * vectorAction[1];
            wheelFR.steerAngle = 20f * vectorAction[1];
		}
        if(vectorAction[1] > 0)
        { //Turn Right
            wheelFL.steerAngle = 20f * vectorAction[1];
            wheelFR.steerAngle = 20f * vectorAction[1];
		}
        if(vectorAction[1] == 0)
        { //If nothing make wheel centered
			wheelFL.steerAngle = 0f;
            wheelFR.steerAngle = 0f;
		}

        /*Debug.Log("FrontLeft: " + wheelFL.rpm);
        Debug.Log("FrontRight: " + wheelFR.rpm);
        Debug.Log("BackLeft: " + wheelBL.rpm);
        Debug.Log("BackRight: " + wheelBR.rpm);*/

        if (transform.parent.up.y < 0) AddReward(-0.01f);

        if (speed > .1f)
        {
            resetIfCantMove = 5;
        }
        else
        {
            resetIfCantMove -= Time.fixedDeltaTime;
        }

        if (resetIfCantMove <= 0 || timer > 120)
        {
            Reset();
        }

        AddReward(-.001f);
    }

    private void Reset()
    {
        wheelFL.steerAngle = 0f;
        wheelFR.steerAngle = 0f;

        wheelBL.motorTorque = 0f;
        wheelBR.motorTorque = 0f;
        wheelBL.brakeTorque = 0f;
        wheelBR.brakeTorque = 0f;
        rb_car.velocity = new Vector3();

        transform.parent.position = new Vector3(-3.05f, 2.06f, -31.85f);
        transform.parent.rotation = Quaternion.Euler(new Vector3());
        SetReward(0);
        currentCheckPoint = 0;
        resetIfCantMove = 5;
        timer = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-2f);
            EndEpisode();
        }

        if (other.gameObject == checkPoints[currentCheckPoint])
        {
            AddReward(.5f);
            currentCheckPoint = currentCheckPoint == checkPoints.Length - 1 ? 0 : currentCheckPoint + 1;
            Debug.Log("Reward");
        }
        else if (checkPoints.Contains(other.gameObject))
        {
            Debug.Log(other.name);
            AddReward(-2f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }
}
