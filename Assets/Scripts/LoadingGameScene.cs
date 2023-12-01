using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingGameScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI loadingFinishText;
    private AsyncOperation op;
    private string nextScene;
    private bool isLoad = false;

    void Start()
    {
        nextScene = "GameScene";
        StartCoroutine(LoadSceneProcess());
    }

    private void Update()
    {
        if (loadingFinishText.gameObject.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isLoad = true;
            }
        }
    }

    private IEnumerator LoadSceneProcess()
    {
        op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return new WaitForSeconds(0.5f);

            if (op.progress >= 0.9f)
            {
                loadingText.gameObject.SetActive(false);
                loadingFinishText.gameObject.SetActive(true);

                if (isLoad)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
