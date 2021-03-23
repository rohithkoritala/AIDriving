using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    float top_speed = 30f;
    float acceleration = 8f;
    float speed = 0.0f;
    float drift = 50f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed);

        if (Input.GetKey(KeyCode.RightArrow)) {

            transform.Rotate(0, drift * Time.deltaTime, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            transform.Rotate(0, -drift * Time.deltaTime, 0, Space.Self);
        }

        if (Input.GetKey(KeyCode.X))
        {

            if (speed >= top_speed)
            {
                speed = top_speed;
            }
            else
            {
                speed += acceleration * Time.deltaTime;
            }      
        }

        else {

            if (speed <= 0.0f)
            {
                speed = 0.0f;
            }
            else {
                speed -= 6f * Time.deltaTime;
            }
        }
    }
}
