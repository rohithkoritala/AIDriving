using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{

    private List<CheckpointSingle> checkpointSingleList;
    private int nextCheckpointSingleIndex;

    private void Awake() {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointSingleList = new List<CheckpointSingle>();

        foreach(Transform checkPointSingleTransform in checkpointsTransform) {
            CheckpointSingle checkpointSingle = checkPointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
		}

        nextCheckpointSingleIndex = 0;
	}

    public void PlayerCrossedCheckpoint(CheckpointSingle checkpointSingle) {
        if(checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            //Debug.Log("Correct");
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
		}
		else {
            //Debug.Log("Wrong");
		}
        if(nextCheckpointSingleIndex == checkpointSingleList.Count - 1) {
            GlobalVariables.LapComplete = true;

		}
	}
}
