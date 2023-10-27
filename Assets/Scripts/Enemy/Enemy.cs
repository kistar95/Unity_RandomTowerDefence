using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float enemyHp;
    private int wayPointCount;
    private int currentIndex;
    private Movement movement;
    private EnemySpawner enemySpawner;
    
    public Movement Movement
    {
        get { return movement; }
    }

    public void SetUp(int _hp, Transform[] _wayPoints, EnemySpawner _enemySpawner)
    {
        enemyHp = _hp;
        wayPoints = _wayPoints;
        enemySpawner = _enemySpawner;
        wayPointCount = wayPoints.Length;
        currentIndex = 0;
        movement = GetComponent<Movement>();

        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextWayPoint();

        while (true)
        {
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < movement.MoveSpeed * 0.02)
            {
                NextWayPoint();
            }

            yield return null;
        }
    }

    private void NextWayPoint()
    {
        if (currentIndex < wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 dir = (wayPoints[currentIndex].position - transform.position).normalized;
            movement.SetDirection(dir);
        }
        else
        {
            OnDie();
            GameManager.Instance.LoseLife();
        }
    }

    public void OnDamaged(float _dmg)
    {
        enemyHp -= _dmg;

        if (enemyHp <= 0)
        {
            GameManager.Instance.KillCounting();
            OnDie();
        }
    }

    public void OnDie()
    {
        enemySpawner.CurEnemyList.Remove(this.transform);
        gameObject.SetActive(false);
    }
}
