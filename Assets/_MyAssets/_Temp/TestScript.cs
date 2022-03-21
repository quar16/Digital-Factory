using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public Transform parent;
    public Transform child;

    public Transform a1;
    public Transform a2;
    public Transform a3;

    public void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            parent.position += child.forward * 0.01f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            parent.position -= child.forward * 0.01f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            parent.position -= child.right * 0.01f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            parent.position += child.right * 0.01f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            parent.eulerAngles += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            parent.eulerAngles -= new Vector3(0, 1, 0);
        }

        //Vector3 v3 = a2.transform.position - a1.transform.position;
        //float angle = Vector3.Angle(v3, Vector3.right);
        //if (v3.z < 0)
        //    angle = 360 - angle;
        //Debug.Log(angle);
        //a3.eulerAngles = new Vector3(0, 180-angle, 0);
    }

    //public GameObject cube;
    //int count = 0;
    //public void FixedUpdate()
    //{
    //    count++;
    //    if (count < 500)
    //    {
    //        Instantiate(cube, new Vector3(0, 4, 0), Quaternion.identity, null);
    //    }
    //}
}
