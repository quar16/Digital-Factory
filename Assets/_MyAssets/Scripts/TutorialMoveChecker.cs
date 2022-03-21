using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMoveChecker : MonoBehaviour
{
    public Transform innerArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Camera>() != null)
        {
            FlowManager.FlowMessage = "WalkTutorialDone";
        }
    }

    public void Update()
    {
        transform.eulerAngles += Vector3.up * -0.27f;
        innerArea.eulerAngles += Vector3.up;
    }

    //time check
}
