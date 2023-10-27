using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : Skill
{
    private float slow = 0.5f;

    private void Start()
    {
        StartCoroutine("ColliderEnableAndDisable");
    }

    public override IEnumerator ColliderEnableAndDisable()
    {
        yield return new WaitForSeconds(1f);
        skillCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        skillCollider.enabled = false;
    }

    private IEnumerator ResetEnemy(Transform _target)
    {
        yield return new WaitForSeconds(1.5f);
        Enemy enemy = _target.GetComponent<Enemy>();
        enemy.Movement.ResetSpeed();
    }

    private void SlowEnemy(Transform _target)
    {
        Enemy enemy = _target.GetComponent<Enemy>();
        enemy.Movement.MoveSpeed -= enemy.Movement.MoveSpeed * slow;
        StartCoroutine(ResetEnemy(_target));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SlowEnemy(other.transform);
        }
    }
}
