using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControls : MonoBehaviour //This class is for the general car controls (flip mechanic)
{

    [SerializeField] private Rigidbody rb_car;
    [SerializeField] private Vector3 car_COM; //Car Center of Mass

    Vector3 CurrentAngle; //Euler angle

    public Vector3 Velocity;
    private float Speed;
    private double SpeedKPH;
    public Text SpeedText;
    public Text SpeedKPHText;

    public float FlipTime = 0f;
    bool Flipped = false;

    private float originalRotationX;
    private float originalRotationY;
    private float originalRotationZ;


    void Start()
    {
        
        originalRotationX = rb_car.transform.position.x;
        originalRotationY = rb_car.transform.position.y;
        originalRotationZ = rb_car.transform.position.z;

        rb_car.centerOfMass = car_COM; //Sets the center of mass to the assigned object in the inspector
    }

    void FixedUpdate()
    {
        //CurrentAngle = rb_car.transform.rotation;
        //Debug.Log("X: " + rb_car.transform.rotation.x);
        //Debug.Log(rb_car.transform.rotation);
        
        //get speed of car
        Velocity = rb_car.velocity;
        Speed = Velocity.magnitude * 2; //2x for bigger number :)
        //Debug.Log("Speed: " + Speed);

        SpeedKPH = Speed * 1.60934;

        if (SpeedText) SpeedText.text = "mph: " + Speed.ToString("0.0");
        if (SpeedKPHText) SpeedKPHText.text = "kph: " + SpeedKPH.ToString("0.0");


        //Flip stuff
        if((rb_car.transform.rotation.x >= 0.2) || (rb_car.transform.rotation.x <= -0.2)
        || (rb_car.transform.rotation.z <= -0.2) || (rb_car.transform.rotation.z <= -0.2)) { //If car is tilted enough, start flip timer.
            FlipTime += Time.fixedDeltaTime;
            Debug.Log("Fliptime: " + FlipTime);
            Flipped = true;

            if((FlipTime >= 5) && (Flipped == true)) { //if flipped for more than 5 seconds, reset to normal orientation (a bit above the ground)
                rb_car.transform.position = new Vector3(rb_car.transform.position.x, rb_car.transform.position.y + 1, rb_car.transform.position.z);
                rb_car.transform.rotation = Quaternion.Euler(new Vector3(rb_car.transform.rotation.eulerAngles.x, rb_car.transform.rotation.eulerAngles.y, 0));

                //rb_car.transform.rotation = Change(originalRotationX, rb_car.transform.rotation.y, originalRotationZ); //new Vector3(0, 0, 0 /*,rb_car.transform.rotation.w*/);

                //reset variables for flip stuff
                Flipped = false; 
                FlipTime = 0;
			}
		}
		else {
            Flipped = false;
            FlipTime = 0;
		}

    }

    //Method to convert Float values into Quaternion values
    private static Quaternion Change(float x, float y, float z) 
    {
        Quaternion newQuaternion = new Quaternion();
        newQuaternion.Set(x, y, z, 1);
        //Return the new Quaternion
        return newQuaternion;
    }


}
