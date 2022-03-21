using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VR_HoldTrigger : VR_Trigger
{
    public Transform gaugeBody;
    public Image gauge;

    public int maxHoldCount;
    int holdCount = 0;
    public UnityEvent holdEndEvent;

    Transform player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void Update()
    {
        gaugeBody.LookAt(player);
    }


    public override void Hold()
    {
        holdCount++;

        gauge.fillAmount = (float)holdCount / maxHoldCount;

        if (holdCount >= maxHoldCount)
        {
            holdEndEvent.Invoke();
        }
        base.Hold();
    }

    public override void Off()
    {
        holdCount = 0;
        gauge.fillAmount = 0;
        base.Off();
    }

}
