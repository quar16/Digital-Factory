using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Camera : MonoSingleton<VR_Camera>
{
    public Transform cameraT;

    public IEnumerator SetCamera(Transform pivot, bool withEffect = true)
    {
        if (withEffect)
            yield return SceneLoader.Instance.SceneChangeEffectShowing(true);

        yield return new WaitWhile(() => cameraT.localPosition == Vector3.zero);

        transform.eulerAngles = new Vector3(0, pivot.eulerAngles.y - cameraT.localEulerAngles.y, 0);

        Vector3 delta = pivot.position + transform.position - cameraT.position;
        transform.position = new Vector3(delta.x, 0, delta.z);



        if (withEffect)
            yield return SceneLoader.Instance.SceneChangeEffectShowing(false);
    }

}
