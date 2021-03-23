using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLCarControls : MonoBehaviour //This class is for the general car controls (flip mechanic)
{

    [SerializeField] private Rigidbody rb_car;

    Vector3 CurrentAngle; //Euler angle
    public float FlipTime = 0f;
    bool Flipped = false;


    void Start()
    {
        
    }

    void FixedUpdate()
    {
        //CurrentAngle = rb_car.transform.rotation;
        Debug.Log("X: " + rb_car.transform.rotation.x);
        Debug.Log(rb_car.transform.rotation);

        if((rb_car.transform.rotation.x >= 0.1) || (rb_car.transform.rotation.x <= -0.1)) { //If car is tilted enough, start timer.
            FlipTime += Time.deltaTime;
            Debug.Log("Fliptime: " + FlipTime);
            Flipped = true;

            if((FlipTime >= 5) && (Flipped == true)) { //if flipped for more than 5 seconds, reset to normal orientation (a bit above the ground)
                rb_car.transform.position = new Vector3(rb_car.transform.position.x, rb_car.transform.position.y + 1, rb_car.transform.position.z);
                rb_car.transform.rotation = Change(0,0,0); //new Vector3(0, 0, 0 /*,rb_car.transform.rotation.w*/);

                //reset variables for flip stuff
                Flipped = false; 
                FlipTime = 0;
			}
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
