using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public Define.WorldObject WorldObjectType { get; protected set; }
    public float MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Hp { get { return _hp; } set { _hp = value; } }
    public float Attack { get { return _attack; } set { _attack = value; } }
    public float AttackDelay { get { return _attackDelay; } set { _attackDelay = value; } }
    public virtual float AttackRange { get { return _attackRange * Define.TILE_SIZE; } set { _attackRange = value; } }
    public bool IsEnemy { get; protected set; }
    [SerializeField] protected float _maxHp;
    [SerializeField] protected float _hp;
    [SerializeField] protected float _attack;
    [SerializeField] protected float _currentAttackTime;
    [SerializeField] protected float _attackDelay;
    [SerializeField] protected float _attackRange;
    Coroutine _coroutine;
    bool _init;

    protected virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    public virtual void OnDamaged(BaseController bc) { }

    public virtual void OnDead()
    {
        Managers.Object.Despawn(gameObject);
    }

    protected void ForWait(float t, Action evt)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CoForWait(t, evt));
    }

    IEnumerator CoForWait(float t, Action evt)
    {
        yield return new WaitForSeconds(t);
        evt.Invoke();
    }
}