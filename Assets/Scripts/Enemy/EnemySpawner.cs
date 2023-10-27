using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private int enemyCount;
    [SerializeField] private int enemyHp;
    [SerializeField] private float enemySpawnDelay;
    [SerializeField] private List<Transform> stageEnemyList; // 현재 스테이지 적 리스트
    [SerializeField] private List<Transform> curEnemyList; // 살아있는 적 리스트
    [SerializeField] private WaveSystem waveSystem;
    [SerializeField] private MissionSystem missionSystem;
    private float enemyMoveSpeed;

    int count = 0;

    public int SpawnedEnemyCount { get; set; } = 0;

    public int EnemyCount
    {
        get { return enemyCount; }
    }

    public List<Transform> StageEnemyList
    {
        get { return stageEnemyList; }
    }

    public List<Transform> CurEnemyList
    {
        get { return curEnemyList; }
    }

    private void Awake()
    {
        GameManager.Instance.GameOverEvent.AddListener(GameOverEnemy);
    }

    private void Update()
    {
        missionSystem.CheckKillMission();
    }

    public void StartWave(Wave _wave)
    {
        enemyPrefab = _wave.enemyPrefab;
        enemyCount = _wave.spawnEnemyCount;
        enemyMoveSpeed = _wave.enemyMoveSpeed;
        enemyHp = _wave.waveEnemyHp;
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (SpawnedEnemyCount <= enemyCount - 1)
        {
            GameObject clone = Instantiate(enemyPrefab);
            clone.GetComponent<Movement>().MoveSpeed = enemyMoveSpeed;
            clone.name = "Enemy" + count++;
            SpawnedEnemyCount++;
            Enemy enemy = clone.GetComponent<Enemy>();
            enemy.SetUp(enemyHp, wayPoints, this);
            stageEnemyList.Add(clone.transform);
            curEnemyList.Add(clone.transform);
            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    private void GameOverEnemy()
    {
        StopCoroutine("SpawnEnemy");
        for (int i = 0; i < stageEnemyList.Count; i++)
        {
            Destroy(stageEnemyList[i].gameObject);
        }
        stageEnemyList.Clear();
    }
}
