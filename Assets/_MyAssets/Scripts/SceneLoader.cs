using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SCENE { MAIN, FACTORY, RECORD }

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader instance;

    public static SceneLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    static string[] scenes = { "MainScene", "FactoryScene", "RecordScene" };

    public static bool isLoad = false;

    public void SceneLoad(SCENE openScene)
    {
        if (!isLoad)
        {
            isLoad = true;
            Instance.StartCoroutine(Instance.SceneLoding(openScene));
        }
    }

    IEnumerator SceneLoding(SCENE openScene)
    {
        yield return SceneChangeEffectShowing(true);

        AsyncOperation open;
        open = SceneManager.LoadSceneAsync(scenes[(int)openScene], LoadSceneMode.Single);
        yield return new WaitUntil(() => open.isDone);

        yield return SceneChangeEffectShowing(false);

        isLoad = false;
    }


    public IEnumerator SceneChangeEffectShowing(bool isShowing)
    {
        if (GameObject.FindGameObjectWithTag("FadeEffect") != null)
        {
            Image fadeEffect = GameObject.FindGameObjectWithTag("FadeEffect").GetComponent<Image>();

            int alpha = isShowing ? 0 : 1;
            float time = 60;

            fadeEffect.color = new Color(0, 0, 0, alpha);

            //fadeEffect.gameObject.SetActive(true);


            for (int i = 0; i < time; i++)
            {
                float t = i / time;
                fadeEffect.color = new Color(0, 0, 0, isShowing ? t : 1 - t);
                yield return null;
            }

            fadeEffect.color = new Color(0, 0, 0, 1 - alpha);

            //fadeEffect.gameObject.SetActive(isShowing);
        }
    }
}