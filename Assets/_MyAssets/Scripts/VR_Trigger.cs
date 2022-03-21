using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum InteractionType { LEFT, RIGHT, ALL }

public class VR_Trigger : MonoBehaviour
{
    public InteractionType type;
    public VR_Detector detector;

    public UnityEvent ClickEvent;
    public UnityEvent HoldEvent;
    public UnityEvent OffEvent;

    public void Click()
    {
        ClickEvent.Invoke();
    }

    public virtual void Hold()
    {
        HoldEvent.Invoke();
    }

    public virtual void Off()
    {
        OffEvent.Invoke();
    }

}
