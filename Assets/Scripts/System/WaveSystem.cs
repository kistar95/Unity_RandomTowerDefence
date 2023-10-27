using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveState
{
    Running,
    StopAndReady
}

[System.Serializable]
public struct Wave
{
    public GameObject enemyPrefab;
    public int waveEnemyHp;
    public int spawnEnemyCount;
    public float enemyMoveSpeed;
}

public class WaveSystem : MonoBehaviour
{
    [SerializeField] private WaveState waveState = WaveState.StopAndReady;
    [SerializeField] private Wave[] waves;
    [SerializeField] private EnemySpawner enemySpawner;
    //private int stageLevel;
    [SerializeField] private int curStageLevel = -1;
    //private int stageDelayTime = 10;
    private WaitForSeconds stageDelay;

    public int CurStageLevel
    {
        get { return curStageLevel; }
    }

    public WaveState WaveState
    {
        get { return waveState; }
    }

    private void Start()
    {
        stageDelay = new WaitForSeconds(GameManager.Instance.StageDelayTime);
        StartCoroutine("StopAndReady");
    }

    private void Update()
    {
        if (enemySpawner.CurEnemyList.Count <= 0 && waveState == WaveState.Running)
        {
            EndWave();
            StartCoroutine("StopAndReady");
        }
    }

    public void StartWave()
    {
        if (enemySpawner.SpawnedEnemyCount == 0 && curStageLevel < waves.Length - 1)
        {
            //ChangeWave(WaveState.Running);
            waveState = WaveState.Running;
            for (int i = 0; i < enemySpawner.StageEnemyList.Count; i++)
            {
                Destroy(enemySpawner.StageEnemyList[i].gameObject);
            }
            enemySpawner.StageEnemyList.Clear();
            curStageLevel++;
            enemySpawner.StartWave(waves[curStageLevel]);
        }
    }

    public void EndWave()
    {
        if (curStageLevel + 1 % 10 == 0) // 보스 스테이지 클리어 시
        {
            GameManager.Instance.GetGold(200);
        }
        else
        {
            GameManager.Instance.GetGold(100);
        }
    }

    private IEnumerator StopAndReady()
    {
        waveState = WaveState.StopAndReady;
        enemySpawner.SpawnedEnemyCount = 0;
        yield return stageDelay;
        StartWave();
    }
}
