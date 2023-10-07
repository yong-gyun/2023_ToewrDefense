using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolUnitController : UnitController
{
    public bool IsTimeLimitUnit { get; set; }
    UnitController _targetUnit;
    ProtectedTowerController _protectedTower;
    Vector3 _destPos;
    int _destIdx;
    int _destCount = 0;

    private void Start()
    {
        Init();
        State = Define.State.Move;
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        IsEnemy = false;
        return true;
    }

    public void SetTower(ProtectedTowerController ptc)
    {
        _protectedTower = ptc;

        float maxDitance = Mathf.Infinity;

        for (int i = 0; i < ptc.PatrolPoints.Count; i++)
        {
            Vector3 dir = (ptc.PatrolPoints[i].position - transform.position);
            dir.y = 0f;
            float distance = dir.magnitude;

            if (distance < maxDitance)
            {
                maxDitance = distance;
                _destPos = ptc.PatrolPoints[i].position;
                _destIdx = i;
            }
        }
    }

    protected override void Update()
    {
        if(Hp <= 0f)
        {
            Hp = 0f;
            OnDead();
            return;
        }

        Hp -= Time.deltaTime;
        UpdateSetTarget();

        switch (State)
        {
            case Define.State.Move:
                UpdateMove();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    protected override void UpdateSetTarget()
    {
        if (_targetUnit != null)
            return;

        _targetUnit = Util.GetShortestDistance(gameObject, Managers.Object.EnemyUnits);
    }

    protected override void UpdateMove()
    {
        if(_targetUnit != null)
        {
            State = Define.State.Attack;
            return;
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0f;

        if (dir.magnitude <= 0.25f)
        {
            if(_destCount == _protectedTower.PatrolPoints.Count)
            {
                if(Managers.Object.ProtectedTowers.Count == 0)
                {
                    if (Managers.Object.LastProtectedTower != null)
                        SetTower(Managers.Object.LastProtectedTower);
                }
                else
                {
                    int idx = Managers.Object.ProtectedTowers.FindIndex((pt) => _protectedTower);
                    SetTower(Managers.Object.ProtectedTowers[idx]);
                }
            }

            if(_protectedTower != null)
            {
                _destCount++;
                _destPos = _protectedTower.PatrolPoints[_destIdx++ % _protectedTower.PatrolPoints.Count].position;
            }
            else
            {
                float maxDistance = Mathf.Infinity;

                foreach (ProtectedTowerController ptc in Managers.Object.ProtectedTowers)
                {
                    Vector3 interval = ptc.transform.position - transform.position;
                    float distance = interval.magnitude;

                    if(distance < maxDistance)
                    {
                        maxDistance = distance;
                        _protectedTower = ptc;
                    }
                }

                ProtectedTowerController protectedTower = Util.GetShortestDistance(gameObject, Managers.Object.ProtectedTowers);
                SetTower(protectedTower);
            }
        }

        transform.position += dir.normalized * MoveSpeed * Time.deltaTime;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }

    protected override void UpdateAttack()
    {
        if(_targetUnit == null)
        {
            State = Define.State.Move;
            return;
        }

        Vector3 dir = _targetUnit.transform.position - transform.position;
        dir.y = 0f;

        if (Util.GetDistance(dir.magnitude) > AttackRange)
        {
            _targetUnit = null;
            return;
        }

        _currentAttackTime -= Time.deltaTime;

        if (_currentAttackTime <= 0f)
        {
            _currentAttackTime = AttackDelay;

            if (_isAnimating == false)
                OnAttacked();
        }

        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }

    protected override void OnAttacked()
    {
        Managers.Sound.Play("Unit/PatrolUnitAttack");
        GameObject go = Managers.Resource.Instantiate($"Bullet/{Type}Bullet");
        go.transform.position = _firePos.position;
        Bullet bullet = go.GetOrAddComponent<Bullet>();
        bullet.SetTarget(this, _targetUnit);
    }
}