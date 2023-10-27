using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TowerType
{
    FIGHTER,
    MARKSMAN,
    MAGE
}

public enum TowerState
{
    SearchEnemy,
    AttackEnemy
}
[System.Serializable]
public struct TowerStatus
{
    public string towerName;
    public int towerLevel;
    public TowerType towerType;
    public float attackDamage;
    public float skillDamage;
    public float upgradeDamage;
    public float attackRange;
    public float attackRate;
    public int multipleTargetCount;
}

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected TowerStatus towerStatus; // Ÿ�� �ɷ�ġ
    [SerializeField] protected GameObject skillPrefab;
    [SerializeField] protected AudioClip attackSound;

    protected Transform attackTarget = null; // ���� ���� Ÿ��
    protected Transform[] attackTargets; // ���� ���� Ÿ��
    protected List<Transform> sortTargets; // Ÿ���� ������ �Ÿ��� ���� ����Ʈ

    protected TowerState towerState = TowerState.SearchEnemy;

    protected TowerManager towerSpawner;
    protected EnemySpawner enemySpawner;
    protected TowerField thisTowerField; // ���� Ÿ���� ��ġ�� �ʵ�
    protected Animator towerAnim;
    protected AudioSource audioSource;

    public TowerField ThisTowerField
    {
        get { return thisTowerField; }
    }

    public TowerStatus TowerStatus
    {
        get { return towerStatus; }
    }

    public Skill TowerSkill
    {
        get 
        {
            if (skillPrefab != null)
            {
                return skillPrefab.GetComponent<Skill>();
            }
            else
            {
                return null;
            }
        }
    }

    private void Update()
    {
        if (attackTarget != null)
        {
            Vector3 dir = (attackTarget.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        if (towerStatus.multipleTargetCount > 0 && towerState == TowerState.AttackEnemy)
        {
            Vector3 dir = (attackTargets[0].position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void Setup(TowerManager _towerSpawner, EnemySpawner _enemySpawner, TowerField _towerField)
    {
        towerSpawner = _towerSpawner;
        enemySpawner = _enemySpawner;
        thisTowerField = _towerField;
        towerAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        // Ÿ�� ���ݼӵ��� ���� ���� �ִϸ��̼� �ӵ� ����
        towerAnim.SetFloat("AttackSpeed", 1 / towerStatus.attackRate);
        attackTargets = new Transform[towerStatus.multipleTargetCount];

        ChangeState(TowerState.SearchEnemy);
    }

    public void ChangeState(TowerState _towerState)
    {
        StopCoroutine(towerState.ToString());
        towerState = _towerState;
        StartCoroutine(towerState.ToString());
    }

    public abstract IEnumerator SearchEnemy();

    public abstract IEnumerator AttackEnemy();

    public abstract void Attack();

    protected Transform FindTarget()
    {
        float closetDistance = 1000.0f;
        for (int i = 0; i < enemySpawner.CurEnemyList.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, enemySpawner.CurEnemyList[i].position);
            if (distance <= towerStatus.attackRange && distance <= closetDistance)
            {
                closetDistance = distance;
                attackTarget = enemySpawner.CurEnemyList[i].transform;
            }
        }

        return attackTarget;
    }

    protected Transform[] FindTargets()
    {
        // ���� Ÿ���� �Ÿ��� ����� ������ ����Ʈ ����
        sortTargets = enemySpawner.StageEnemyList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();
        int count = 0;

        for (int i = 0; i < attackTargets.Length; i++)
        {
            if (attackTargets[i]?.gameObject.activeSelf == false)
            {
                attackTargets[i] = null;
            }
        }

        for (int i = 0; i < sortTargets.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, sortTargets[i].position);
            
            if (distance <= towerStatus.attackRange 
                && sortTargets[i].gameObject.activeSelf == true 
                && count <= towerStatus.multipleTargetCount - 1)
            {
                attackTargets[count] = sortTargets[i];
                count++;
            }
        }
        return attackTargets;
    }

    protected bool isPossibleAttack()
    {
        if (attackTarget.gameObject.activeSelf == false)
        {
            attackTarget = null;
            return false;
        }

        float distance = Vector3.Distance(transform.position, attackTarget.position);
        if (distance > towerStatus.attackRange)
        {
            attackTarget = null;
            return false;
        }

        return true;
    }
}
