using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pause : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TMP_Text playTimeText;

    private void Awake()
    {
        GameManager.Instance.PauseGameEvent.AddListener(PauseGame);
    }

    private void PauseGame()
    {
        playTimeText.text = TimeSpan.FromSeconds(GameManager.Instance.PlayTime).ToString("mm\\:ss");
    }

    public void OnPanel()
    {
        uiManager.OnPanel(this.gameObject);
        GameManager.Instance.PauseGame();
    }

    public void OffPanel()
    {
        uiManager.OffPanel();
        GameManager.Instance.ResumeGame();
    }
}
