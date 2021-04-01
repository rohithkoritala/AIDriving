using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheels : MonoBehaviour //This class controls the wheels which power/control the car (engine/steering)
{

    [SerializeField] private Rigidbody rb_car;

    public float WheelAngleX = 0f;
    public float WheelAngleY = 0f;
    public float WheelAngleZ = 0f;

    public float speed; //value to see speed of car. DO NOT EDIT IN INSPECTOR

    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelBL;
    public WheelCollider wheelBR;

    public float TorqueMultiplier = 0; //for when low speeds to help accelerate
    public float MotorTorque = 0;
    public float BrakeTorque = 0;

    void Start()
    {
        wheelFL.steerAngle = 0f;
        wheelFR.steerAngle = 0f;

        wheelBL.motorTorque = 0f;
        wheelBR.motorTorque = 0f;
        wheelBL.brakeTorque = 0f;
        wheelBR.brakeTorque = 0f;
    }

    void FixedUpdate()
    {
        speed = rb_car.velocity.magnitude;

        //**Controls** (will change values to adjustable variables later)
        if(Input.GetKey("w")){ //Go forward
            wheelBL.motorTorque = MotorTorque;
            wheelBR.motorTorque = MotorTorque;
            wheelBL.brakeTorque = 0f;
            wheelBR.brakeTorque = 0f;

            if(speed < 5) { //Car go fast when slow, until fast enough, then normal speed
                wheelBL.motorTorque = MotorTorque + (MotorTorque * TorqueMultiplier);
                wheelBR.motorTorque = MotorTorque + (MotorTorque * TorqueMultiplier);
			}
		}
        if(Input.GetKey("s")){ //Brake
            wheelBL.brakeTorque = BrakeTorque;
            wheelBR.brakeTorque = BrakeTorque;
            
            if(speed < 5) { //if slowed down enough, stop braking and go backwards
                wheelBL.brakeTorque = 0f;
                wheelBR.brakeTorque = 0f;
                wheelBL.motorTorque = MotorTorque - (MotorTorque * TorqueMultiplier);
                wheelBR.motorTorque = MotorTorque - (MotorTorque * TorqueMultiplier);
			}
            
            
            //wheelBL.brakeTorque = BrakeTorque;
            //wheelBR.brakeTorque = BrakeTorque;
		}
        if(!(Input.GetKey("w") || Input.GetKey("s"))){ //If nothing then do nothing
			wheelBL.motorTorque = 0f;
            wheelBR.motorTorque = 0f;
            wheelBL.brakeTorque = 0f;
            wheelBR.brakeTorque = 0f;
		}
        if(Input.GetKey("a")){ //Turn Left
            wheelFL.steerAngle = -20f;
            wheelFR.steerAngle = -20f;
		}
        if(Input.GetKey("d")){ //Turn Right
            wheelFL.steerAngle = 20f;
            wheelFR.steerAngle = 20f;
		}
        if(!(Input.GetKey("a") || Input.GetKey("d"))){ //If nothing make wheel centered
			wheelFL.steerAngle = 0f;
            wheelFR.steerAngle = 0f;
		}

        /*Debug.Log("FrontLeft: " + wheelFL.rpm);
        Debug.Log("FrontRight: " + wheelFR.rpm);
        Debug.Log("BackLeft: " + wheelBL.rpm);
        Debug.Log("BackRight: " + wheelBR.rpm);*/

    }
}
