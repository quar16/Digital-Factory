using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_RecordDetector : VR_Detector
{
    public ClickState clickState;
    private void Update()
    {
        switch (clickState)
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
}
