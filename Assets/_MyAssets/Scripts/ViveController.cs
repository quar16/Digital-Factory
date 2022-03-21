using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum ClickState { NONE, OFF, CLICK, STAY }

public class ViveController : MonoBehaviour
{
    [Header("VR input ¿¬°á")]
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean triggerClick;

    public Vector3 pos
    {
        get
        {
            return transform.position;
        }
    }
    public Vector3 rot
    {
        get
        {
            return transform.eulerAngles;
        }
    }

    bool lastClickState = false;
    bool nowClickState = false;
    public ClickState clickState;
    void Update()
    {
        nowClickState = triggerClick.GetState(handType);

        clickState = 0;

        if (lastClickState)
            clickState += 1;
        if (nowClickState)
            clickState += 2;

        lastClickState = triggerClick.GetState(handType);
    }

}
public class Controller
{
    public static void Init()
    {
        var array = Object.FindObjectsOfType<ViveController>();

        foreach (var controller in array)
        {
            if (controller.handType == SteamVR_Input_Sources.LeftHand)
                left = controller;
            if (controller.handType == SteamVR_Input_Sources.RightHand)
                right = controller;
        }
    }

    public static ViveController left;
    public static ViveController right;

    public static ViveController Get(InteractionType interactionType)
    {
        if (interactionType == InteractionType.LEFT)
            return left;
        else 
            return right;
    }
}