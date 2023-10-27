using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TMP_Text playTimeText;
    [SerializeField] private GameObject gameOverPanel;

    private void Awake()
    {
        GameManager.Instance.GameOverEvent.AddListener(OnGameOverPanel);
    }

    private void OnGameOverPanel()
    {
        uiManager.OnPanel(gameOverPanel);
        playTimeText.text = TimeSpan.FromSeconds(GameManager.Instance.PlayTime).ToString("mm\\:ss");
    }
}
