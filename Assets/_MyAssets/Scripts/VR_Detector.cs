using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Detector : MonoBehaviour
{
    public InteractionType type;

    private void Update()
    {
        switch (Controller.Get(type).clickState)
        {
            case ClickState.CLICK:
                if (clickTarget != null && (clickTarget.type == InteractionType.ALL || clickTarget.type == type))
                {
                    clickTarget.detector = this;
                    clickTarget.Click();
                }
                break;
            case ClickState.STAY:
                if (holdTarget != null && (holdTarget.type == InteractionType.ALL || holdTarget.type == type))
                {
                    holdTarget.Hold();
                }
                break;
            case ClickState.OFF:
                if (holdTarget != null && (holdTarget.type == InteractionType.ALL || holdTarget.type == type))
                {
                    holdTarget.Off();
                    holdTarget = null;
                }
                break;
        }
    }

    public VR_Trigger clickTarget;
    public VR_Trigger holdTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VR_Trigger>() != null)
        {
            clickTarget = other.GetComponent<VR_Trigger>();
            holdTarget = other.GetComponent<VR_Trigger>();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<VR_Trigger>() != null)
        {
            holdTarget = other.GetComponent<VR_Trigger>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<VR_Trigger>() != null)
        {
            clickTarget = null;
        }
    }

    public void LoseTarget()
    {
        clickTarget = null;
        holdTarget = null;
    }
}
