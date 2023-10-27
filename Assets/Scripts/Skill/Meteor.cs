using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Skill
{
    private void Start()
    {
        StartCoroutine("ColliderEnableAndDisable");
    }

    public override IEnumerator ColliderEnableAndDisable()
    {
        yield return new WaitForSeconds(0.5f);
        skillCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);
        skillCollider.enabled = false;
    }

    public void MeteorAttackEnemy(Transform _target)
    {
        Enemy enemy = _target.GetComponent<Enemy>();
        enemy.OnDamaged(skillDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            MeteorAttackEnemy(other.transform);
        }
    }
}
