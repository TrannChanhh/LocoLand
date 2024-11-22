using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public float loadingScene = 3f;
    void Start()
    {
        StartCoroutine(LoadingScene());
    }

    // Update is called once per frame
    IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(loadingScene);
        SceneManager.LoadScene("Main");
    }
}
