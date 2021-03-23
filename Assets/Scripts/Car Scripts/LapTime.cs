using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LapTime : MonoBehaviour
{
    //[SerializeField] private Rigidbody rb_car;

    public float lapNumber = 1;
    public float lapTime = 0;
    public float BestLapTime = 0;
    private bool FirstLap = true;
    private bool Flag = true;

    public Text LapText;
    public Text BestLapText;
    public Text LapNumberText;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(FirstLap == false) {
            LapText.text = "Lap Time: " + lapTime.ToString("0.0");
            BestLapText.text = "Best Lap: " + BestLapTime.ToString("0.0");

            lapTime += Time.fixedDeltaTime;
		}
        LapNumberText.text = "Lap " + lapNumber.ToString("0") + "/3";
        
        
    }

    private void OnTriggerEnter(Collider other) {
        //lapNumber starts at 1
        if((other.gameObject.tag == "Player") && (GlobalVariables.LapComplete == true)) {
            //Debug.Log(GlobalVariables.LapComplete);
            //Debug.Log("test");
            //Debug.Log(FirstLap);
            //Debug.Log("Lap Time: " + lapTime);
            
            
            if((lapTime < BestLapTime) && (FirstLap == false) && (GlobalVariables.LapComplete == true)){
                BestLapTime = lapTime;
                //Debug.Log("speedest");
                GlobalVariables.LapComplete = false;
                //Debug.Log(GlobalVariables.LapComplete);
                lapNumber++;
			}
            
            if((FirstLap == false) && (Flag == false) && (GlobalVariables.LapComplete == true)){
                //BestLapTime = lapTime;
                //Debug.Log("test");
                GlobalVariables.LapComplete = false;
                //Debug.Log(GlobalVariables.LapComplete);
                lapNumber++;
			}
            if((FirstLap == false) && (Flag == true) && (GlobalVariables.LapComplete == true)){
                BestLapTime = lapTime;
                Flag = false;
                //Debug.Log("test 2");
                //Debug.Log("BestLap Time: " + BestLapTime);
                GlobalVariables.LapComplete = false;
                
                lapNumber++;
                //Debug.Log(lapNumber);
			}
            if(FirstLap == true){
                FirstLap = false;
                GlobalVariables.LapComplete = false;
                lapNumber++;
                
			}
            if(lapNumber == 4) {
                lapNumber = 3;
                //Debug.Log("YOU DID IT HOLY SHIT");
                SceneManager.LoadScene("VictoryScene");
                
			}
            Debug.Log(lapNumber);
            lapTime = 0;

            
		}

	}
}
