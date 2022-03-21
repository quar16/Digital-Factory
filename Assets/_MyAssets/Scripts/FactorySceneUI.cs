using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactorySceneUI : MonoBehaviour
{
    public void ToMain()
    {
        SceneLoader.Instance.SceneLoad(SCENE.MAIN);
    }
}
