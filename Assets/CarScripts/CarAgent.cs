using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;
using UnityEngine.Serialization;
using System.Collections;

public class CarAgent : Agent
{
    float top_speed;
    float acceleration;
    float speed;
    float drift;
    private Rigidbody rBody;
    private Vector3 startingPosition;

    public override void Initialize()
    {

        rBody = GetComponent<Rigidbody>();
        startingPosition = transform.position;

        speed = 0.0f;
        acceleration = 8f;
        top_speed = 30.0f;
        drift = 50.0f;
    }

    public override void OnActionReceived(float[] vectorAction)
    {

        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {

            Accelerate();
        }
        else
        {
            Deccelerate();
        }

        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            TurnRight();
        }
        else if (Mathf.FloorToInt(vectorAction[1]) == 2)
        {
            TurnLeft();
        }

    }

    public override void Heuristic(float[] actionsOut) {

        actionsOut[0] = 0;
        actionsOut[1] = 0;

        if (Input.GetKey(KeyCode.X)) actionsOut[0] = 1;
        if (Input.GetKey(KeyCode.RightArrow)) actionsOut[1] = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) actionsOut[1] = 2;
    }

    private void FixedUpdate() {

       RequestDecision();
    }

    public override void OnEpisodeBegin()
    {

        Reset();
    }

    public void TurnRight()
    {

        transform.Rotate(0, drift * Time.deltaTime, 0, Space.Self);
    }

    public void TurnLeft()
    {

        transform.Rotate(0, -drift * Time.deltaTime, 0, Space.Self);
    }

    public void Accelerate()
    {

        transform.Translate(transform.forward * Time.deltaTime * speed);

        if (speed >= top_speed)
        {
            speed = top_speed;
        }
        else
        {
            speed += acceleration * Time.deltaTime;
        }
    }

    public void Deccelerate()
    {

        transform.Translate(transform.forward * Time.deltaTime * speed);

        if (speed <= 0.0f)
        {
            speed = 0.0f;
        }
        else
        {
            speed -= 6f * Time.deltaTime;
        }
    }

    public void Reset() { }
}