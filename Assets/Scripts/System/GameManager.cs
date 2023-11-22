using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    [SerializeField] private int playerLife;
    [SerializeField] private int playerGold;
    [SerializeField] private float stageDelayTime;
    [SerializeField] private int enemyKillCount = 0;
    private float playTime = 0;
    private bool isGameOver = false;
    private int originPlayerGold;
    private int originPlayerLife;

    public int FighterTowerUpgradeLevel { get; set; } = 0;
    public int MageTowerUpgradeLevel { get; set; } = 0;
    public int MarksmanTowerUpgradeLevel { get; set; } = 0;

    [HideInInspector] public UnityEvent GameOverEvent;
    [HideInInspector] public UnityEvent PauseGameEvent;
    [HideInInspector] public UnityEvent ResumGameEvent;
    [HideInInspector] public UnityEvent RestartGameEvent;

    public int CurrentLife
    {
        get { return playerLife; }
    }

    public int CurrentGold
    {
        get { return playerGold; }
    }

    public float StageDelayTime
    {
        get { return stageDelayTime; }
    }

    public int EnemyKillCount
    {
        get { return enemyKillCount; }
    }

    public float PlayTime
    {
        get { return playTime; }
    }

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        originPlayerGold = playerGold;
        originPlayerLife = playerLife;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        GameOver();
    }

    public void LoseLife()
    {
        playerLife -= 1;
    }

    public void UseGold(int _gold)
    {
        playerGold -= _gold;
    }

    public void GetGold(int _gold)
    {
        playerGold += _gold;
    }

    public void KillCounting()
    {
        enemyKillCount++;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("일시정지");
        PauseGameEvent.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        ResumGameEvent.Invoke();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        ResetGameData();
        RestartGameEvent.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        if (playerLife > 0)
        {
            return;
        }
        else if (playerLife <= 0 && !isGameOver)
        {
            Time.timeScale = 0;
            GameOverEvent.Invoke();
            isGameOver = true;
        }
    }

    private void ResetGameData()
    {
        playerGold = originPlayerGold;
        playerLife = originPlayerLife;
        FighterTowerUpgradeLevel = 0;
        MarksmanTowerUpgradeLevel = 0;
        MageTowerUpgradeLevel = 0;
    }
}
