using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordSceneCamera : MonoBehaviour
{
    public Transform cameraT;
    public Transform target;

    Vector3 nowPos;
    Vector3 lastPos;

    float targetX = -0.5f;
    float targetY = 0.5f;
    float speed = 0.02f;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            lastPos = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            nowPos = Input.mousePosition;

            float x = nowPos.x - lastPos.x;
            float y = nowPos.y - lastPos.y;
            targetX -= x * 0.001f;
            targetY = Mathf.Clamp(targetY + y * 0.001f, 0.01f, 0.99f);

            float posY = -Mathf.Cos(targetY * Mathf.PI);
            float xzValue = Mathf.Abs(Mathf.Sin(targetY * Mathf.PI));
            float posX = Mathf.Cos(targetX * Mathf.PI) * xzValue;
            float posZ = Mathf.Sin(targetX * Mathf.PI) * xzValue;

            target.position = cameraT.position + new Vector3(posX, posY, posZ);

            cameraT.LookAt(target, Vector3.up);

            lastPos = Input.mousePosition;

            /////////////////


            if (Input.GetKey(KeyCode.E))
                speed = Mathf.Clamp(speed + 0.001f, 0.02f, 0.2f);
            if (Input.GetKey(KeyCode.Q))
                speed = Mathf.Clamp(speed - 0.001f, 0.02f, 0.2f);

            Vector3 forward = Vector3.Normalize(new Vector3(cameraT.forward.x, 0, cameraT.forward.z));
            Vector3 left = Vector3.Normalize(-new Vector3(cameraT.right.x, 0, cameraT.right.z));
            Vector3 up = Vector3.up;

            if (Input.GetKey(KeyCode.W))
                cameraT.position += forward * speed;
            if (Input.GetKey(KeyCode.S))
                cameraT.position -= forward * speed;
            if (Input.GetKey(KeyCode.A))
                cameraT.position += left * speed;
            if (Input.GetKey(KeyCode.D))
                cameraT.position -= left * speed;
            if (Input.GetKey(KeyCode.R))
                cameraT.position += up * speed;
            if (Input.GetKey(KeyCode.F))
                cameraT.position -= up * speed;

            if (Input.mouseScrollDelta.y > 0)
                cameraT.position += cameraT.forward * speed;
            else if (Input.mouseScrollDelta.y < 0)
                cameraT.position -= cameraT.forward * speed;

        }
    }
}
