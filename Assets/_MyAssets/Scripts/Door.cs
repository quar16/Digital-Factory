using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public VR_Trigger trigger;
    public Transform door;

    Vector3 firstVector;

    bool open = false;
    public bool isOpen
    {
        get
        {
            return open;
        }
    }
    float startAngle = 0;

    public bool isAxisX = false;
    public int setDoorMax = 0;
    int doorMax = 110;

    public void Start()
    {
        trigger.ClickEvent.AddListener(() => { DoorClick(); });
        trigger.HoldEvent.AddListener(() => { DoorHold(); });
        trigger.OffEvent.AddListener(() => { DoorOff(); });

        if (setDoorMax != 0)
            doorMax = setDoorMax;
    }

    public void DoorClick()
    {
        firstVector = trigger.detector.transform.position - door.position;
        if (!isAxisX)
            firstVector = new Vector3(firstVector.x, 0, firstVector.z);
        else
            firstVector = new Vector3(0, firstVector.y, firstVector.z);
        startAngle = door.localEulerAngles.y;
    }
    public void DoorHold()
    {
        Vector3 doorNowVector = trigger.detector.transform.position - door.position;
        if (!isAxisX)
            doorNowVector = new Vector3(doorNowVector.x, 0, doorNowVector.z);
        else
            doorNowVector = new Vector3(0, doorNowVector.y, doorNowVector.z);

        float angle = startAngle + Vector3.SignedAngle(firstVector, doorNowVector, door.up);
        door.localEulerAngles = new Vector3(0, Mathf.Clamp(angle, 0, doorMax), 0);

        if (door.localEulerAngles.y > 90)
            open = true;
    }
    public void DoorOff()
    {
        if (door.localEulerAngles.y > doorMax - 10)
        {
            open = true;
            door.localEulerAngles = new Vector3(0, doorMax, 0);
        }
        else if (door.localEulerAngles.y < 5)
        {
            open = false;
            door.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}
