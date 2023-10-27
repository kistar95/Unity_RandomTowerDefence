using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected float skillCooltime;
    [SerializeField] protected Sprite skillSprite;
    protected float skillDamage;
    protected Transform target;

    protected ParticleSystem particleSystems;
    protected Collider skillCollider;

    public float SkillCooltime
    {
        get { return skillCooltime; }
    }
    public Sprite SkillSprite
    {
        get { return skillSprite; }
    }

    public abstract IEnumerator ColliderEnableAndDisable();

    public void Setup(Transform _target, float _damage)
    {
        particleSystems = GetComponent<ParticleSystem>();
        skillCollider = GetComponent<Collider>();
        target = _target;
        skillDamage = _damage;
    }

    private void Update()
    {
        if (particleSystems != null)
        {
            Destroy(this.gameObject, particleSystems.main.duration);
        }
        else
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
