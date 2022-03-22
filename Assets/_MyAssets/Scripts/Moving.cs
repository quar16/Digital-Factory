using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Transform parent;
    public Transform child;

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
    }
}
