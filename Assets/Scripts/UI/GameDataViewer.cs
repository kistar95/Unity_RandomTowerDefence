using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDataViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI StageText;
    [SerializeField] private TextMeshProUGUI GoldText;
    [SerializeField] private TextMeshProUGUI LifeText;
    [SerializeField] private WaveSystem waveSystem;
    float second;

    private void Start()
    {
        second = GameManager.Instance.StageDelayTime;
    }

    void Update()
    {
        GameTimer();
        GameRoundCount();
        GoldAndLife();
    }

    private void GameTimer()
    {
        if (waveSystem.WaveState == WaveState.StopAndReady)
        {
            timerText.gameObject.SetActive(true);
            second -= Time.deltaTime;
            timerText.text = second.ToString("F1");
            
        }
        else if (waveSystem.WaveState == WaveState.Running)
        {
            timerText.gameObject.SetActive(false);
            second = GameManager.Instance.StageDelayTime;
        }
    }

    public void GameRoundCount()
    {
        if (waveSystem.WaveState == WaveState.StopAndReady)
        {
            StageText.text = "NEXT STAGE...";
        }
        else if (waveSystem.WaveState == WaveState.Running)
        {
            StageText.text = "STAGE " + (waveSystem.CurStageLevel + 1).ToString();
        }
        
    }

    public void GoldAndLife()
    {
        GoldText.text = GameManager.Instance.CurrentGold.ToString();
        LifeText.text = GameManager.Instance.CurrentLife.ToString();
    }
}
