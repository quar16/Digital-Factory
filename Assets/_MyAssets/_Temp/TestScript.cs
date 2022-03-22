using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void FixedUpdate()
    {
        Debug.Log("a");
    }

    private void Start()
    {
        StartCoroutine(FixUpdate());
    }

    IEnumerator FixUpdate()
    {
        while (true)
        {
            Debug.Log("b");
            yield return new WaitForFixedUpdate();
        }
    }

    private void Update()
    {
            Debug.Log("c");
    }
}
