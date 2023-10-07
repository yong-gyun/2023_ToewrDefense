using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBossController : UnitController
{
    public override Define.State State
    {
        get => base.State;
        set
        {
            _state = value;

            switch (_state)
            {
                case Define.State.Idle:
                    _anim.CrossFade("Idle", 0.1f);
                    break;
                case Define.State.Attack_To_Idle:
                    _anim.CrossFade("Idle", 0.1f);
                    break;
                case Define.State.Move:
                    _anim.CrossFade("Move", 0.1f);
                    break;
                case Define.State.Attack:
                    _anim.CrossFade("Attack", 0.1f);
                    break;
                case Define.State.RangeAttack:
                    _anim.CrossFade("RangeAttack", 0.1f);
                    break;
                case Define.State.Die:
                    _anim.CrossFade("Die", 0.1f);
                    break;
            }
        }
    }

    float _scanRange = 6f;
    float _currentRangeAttackCooltime = 0f;
    float _rangeAttackCooltime = 5f;
    List<Transform> _summonPoints = new List<Transform>(); 

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Transform summonPoints = gameObject.FindChild("SummonPoints", true).transform;

        for (int i = 0; i < summonPoints.childCount; i++)
            _summonPoints.Add(summonPoints.GetChild(i));

        if(Managers.Game.CurrentStage == 1)
        {
            Managers.Object.SpawnUnit(Define.UnitType.MeleeUnit).transform.position = _summonPoints[0].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.MeleeUnit).transform.position = _summonPoints[1].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.QuickMoveUnit).transform.position = _summonPoints[2].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.QuickMoveUnit).transform.position = _summonPoints[3].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.RangedAttackUnit).transform.position = _summonPoints[4].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.RangedAttackUnit).transform.position = _summonPoints[5].transform.position;
        }
        else if(Managers.Game.CurrentStage == 2)
        {
            Managers.Object.SpawnUnit(Define.UnitType.MiddleBossUnit).transform.position = _summonPoints[0].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.MiddleBossUnit).transform.position = _summonPoints[1].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.MiddleBossUnit).transform.position = _summonPoints[2].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.MiddleBossUnit).transform.position = _summonPoints[3].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.MiddleBossUnit).transform.position = _summonPoints[4].transform.position;
            Managers.Object.SpawnUnit(Define.UnitType.MiddleBossUnit).transform.position = _summonPoints[5].transform.position;
        }

        State = Define.State.Move;
        return true;
    }

    protected override void Update()
    {
        if (State == Define.State.Die)
            return;

        UpdateSetTarget();
        
        switch (State)
        {
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Attack_To_Idle:
                UpdateAttackToIdle();
                break;
            case Define.State.Move:
                UpdateMove();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
        }
    }

    void UpdateIdle()
    {
        if(_lockTarget != null)
        {
            State = Define.State.Move;
        }
    }

    protected override void UpdateMove()
    {
        if (_lockTarget == null)
            return;

        Vector3 dir = _lockTarget.transform.position - transform.position;

        if (dir.magnitude <= _attackInterval)
        {
            State = Define.State.Attack_To_Idle;
            _agent.SetDestination(transform.position);
            return;
        }

        _currentRangeAttackCooltime -= Time.deltaTime;

        if (dir.magnitude <= _scanRange * Define.TILE_SIZE + _lockTarget.Size)
        {
            if (_currentRangeAttackCooltime <= 0f)
            {
                State = Define.State.RangeAttack;
                _agent.SetDestination(transform.position);
                _currentRangeAttackCooltime = _rangeAttackCooltime;
                return;
            }
        }
        _agent.SetDestination(_lockTarget.transform.position);
        _agent.speed = MoveSpeed;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }

    void OnRangeAttack()
    {
        GameObject go = Managers.Resource.Instantiate($"Bullet/{Type}Bullet");
        go.transform.position = _firePos.position;
        Bullet bullet = go.GetOrAddComponent<Bullet>();
        bullet.SetTarget(this, _lockTarget);
    }

    void OnExitRangeAttack()
    {
        State = Define.State.Move;
    }

    protected override void OnAttacked()
    {
        _lockTarget.OnDamaged(this);
    }

    public override void OnDamaged(BaseController bc)
    {
        if (State == Define.State.Die)
            return;

        Hp -= bc.Attack;

        if ((int)Define.Priority.Tower > (int)Priority && bc is TowerController)
        {
            Priority = Define.Priority.Tower;
            _lockTarget = bc as TowerController;
        }

        if (Hp <= 0f)
        {
            Hp = 0f;
            _agent.SetDestination(transform.position);
            State = Define.State.Die;
        }
    }
}
