using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public TMP_Text TMP_Text_Start;
    public TMP_Text TMP_Text_Option;
    public TMP_Text TMP_Text_LoadSave;
    private float originalSize;
    public GameObject mainScene;
    public GameObject optionScene;
    public GameObject loadSaveScene;
    void Start()
    {
        originalSize = TMP_Text_Start.fontSize;
    }
    public void OnclickStartScene()
    {
        AudioManager.Instance.PlayOnClick(1f, 2f); // Set speed to 1.5x, volume to 80%
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnPointerEnter_Start()
    {
        TMP_Text_Start.fontSize = 100f;
    }
    public void OnPointerExit_Start()
    {
        TMP_Text_Start.fontSize = originalSize;
    }
    public void OnPointerEnter_Option()
    {
        TMP_Text_Option.fontSize = 100f;
    }
    public void OnPointerExit_Option()
    {
        TMP_Text_Option.fontSize = originalSize;
    }
    public void OnPointerEnter_Load()
    {
        TMP_Text_LoadSave.fontSize = 100f;
    }
    public void OnPointerExit_Load()
    {
        TMP_Text_LoadSave.fontSize = originalSize;
    }
    public void OnClickOptionScene()
    {
        AudioManager.Instance.PlayOnClick(1f, 2f);
        mainScene.SetActive(false);
        optionScene.SetActive(true);
        loadSaveScene.SetActive(false);
    }
    public void OnClickReturnSCene()
    {
        AudioManager.Instance.PlayOnClick(1f, 2f);
        mainScene.SetActive(true);
        optionScene.SetActive(false);
        loadSaveScene.SetActive(false);
    }
    public void OnClickLoadingSaveScene()
    {
        AudioManager.Instance.PlayOnClick(1f, 2f);
        mainScene.SetActive(false);
        optionScene.SetActive(false);
        loadSaveScene.SetActive(true);
    }

}
