using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AssemblePart : VR_Trigger
{
    Rigidbody rigid;
    Transform parent;
    Outlinable outline;

    public Transform combineTarget;

    public int assembleIndex;

    float posErrorRange = 0.1f;
    float rotErrorRange = 5f;

    float combineTimeSet = 3f;
    float accuracyTimeSet = 1.5f;


    [HideInInspector]
    public bool isCombine = false;
    bool isLose = true;

    public void Start()
    {
        ClickEvent.AddListener(() => { OnClick(); });
        rigid = GetComponent<Rigidbody>();
        parent = transform.parent;
        outline = GetComponent<Outlinable>();
    }

    public void OnClick()
    {
        if (isCombine)
            return;

        if (transform.parent == detector.transform)
        {
            isLose = true;
            transform.parent = parent;
            rigid.constraints = RigidbodyConstraints.None;
            detector.LoseTarget();
        }
        else
        {
            isLose = false;
            transform.parent = detector.transform;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    int resetCount = 0;
    public void Update()
    {
        if (isCombine || isLose)
            return;

        if (triggerStay != lastTriggerStay)
        {
            lastTriggerStay = triggerStay;
            resetCount = 0;

            combineTime += Time.deltaTime;
            Vector3 nowCombineEuler = transform.eulerAngles - combineTarget.eulerAngles;
            Vector3 nowCombinePos = transform.position - combineTarget.position;

            bool PX = Mathf.Abs(nowCombinePos.x) < posErrorRange;
            bool PY = Mathf.Abs(nowCombinePos.y) < posErrorRange;
            bool PZ = Mathf.Abs(nowCombinePos.z) < posErrorRange;
            bool EX = Mathf.Abs(nowCombineEuler.x) < rotErrorRange
                || Mathf.Abs(nowCombineEuler.x) > 360 - rotErrorRange;
            bool EY = Mathf.Abs(nowCombineEuler.y) < rotErrorRange
                || Mathf.Abs(nowCombineEuler.y) > 360 - rotErrorRange;
            bool EZ = Mathf.Abs(nowCombineEuler.z) < rotErrorRange
                || Mathf.Abs(nowCombineEuler.z) > 360 - rotErrorRange;

            if (PX && PY && PZ && EX && EY && EZ)
                combineAccuracy += Time.deltaTime;

            if (combineTime >= combineTimeSet)
            {
                transform.parent = combineTarget;

                if (combineAccuracy >= accuracyTimeSet)
                {
                    transform.localPosition = Vector3.zero;
                    transform.localEulerAngles = Vector3.zero;
                    DataManager.assemblySuccess[assembleIndex] = DataManager.assemblySuccess[assembleIndex] && true;
                }
                else
                {
                    DataManager.assemblySuccess[assembleIndex] = false;
                }

                isCombine = true;
                Destroy(rigid);
                detector.LoseTarget();
            }
        }
        else
        {
            resetCount++;
            if (resetCount >= 10)
            {
                resetCount = 0;
                combineTime = 0;
                combineAccuracy = 0;
            }
        }
        float t = combineTime / combineTimeSet;
        outline.FrontParameters.Color = new Color(t, 1 - t, 0);
    }

    float combineTime = 0;
    float combineAccuracy = 0;
    int triggerStay = 0;
    int lastTriggerStay = 0;

    void OnTriggerStay(Collider other)
    {
        if (isCombine || isLose)
            return;

        if (other.gameObject.transform == combineTarget)
        {
            triggerStay = (triggerStay + 1) % 10;
        }
    }
}