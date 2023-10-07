using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBossController : UnitController
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
                case Define.State.MiddleBoss_Rush:
                    _anim.CrossFade("Rush", 0.1f);
                    break;
                case Define.State.MiddleBoss_RushEnd:
                    _anim.CrossFade("RushEnd", 0.1f);
                    break;
            }
        }
    }

    float _rushDistance = 5f;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

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
            case Define.State.Attack_To_Idle:
                UpdateAttackToIdle();
                break;
            case Define.State.Move:
                UpdateMove();
                break;
            case Define.State.Attack:
                UpdateAttack();
                break;
            case Define.State.MiddleBoss_Rush:
                UpdateRush();
                break;
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

        Debug.DrawRay(transform.position, dir * (_rushDistance * Define.TILE_SIZE), Color.red, 0.5f);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, _rushDistance * Define.TILE_SIZE, LayerMask.GetMask("Block") | LayerMask.GetMask("Tower") | LayerMask.GetMask("ProtectedTower")))
        {
            if (hit.transform.CompareTag("Block") == false)
            {
                if(hit.transform.gameObject == _lockTarget.gameObject)
                    State = Define.State.MiddleBoss_Rush;
            }
        }

        _agent.SetDestination(_lockTarget.transform.position);
        _agent.speed = MoveSpeed;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }

    void UpdateRush()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, Define.TILE_SIZE / 2, LayerMask.GetMask("Tower") | LayerMask.GetMask("ProtectedTower")))
        {
            _lockTarget = hit.transform.GetComponent<TowerController>();
            OnCollisionForRush();
            return;
        }

        _agent.enabled = false;
        Vector3 dir = (_lockTarget.transform.position - transform.position);
        transform.position += dir.normalized * MoveSpeed * 2f * Time.deltaTime;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }

    void OnCollisionForRush()
    {
        _agent.enabled = true;
        Attack *= 1.5f;
        _lockTarget.OnDamaged(this);
        Attack /= 1.5f;
        State = Define.State.MiddleBoss_RushEnd;
        Managers.Sound.Play("Unit/MiddleBossRush");
    }

    void OnRushEnd()
    {
        _agent.enabled = true;
        _agent.SetDestination(transform.position);

        if(_lockTarget != null)
            State = Define.State.Attack;
        else
            State = Define.State.Move;
    }

    public override void OnDamaged(BaseController bc)
    {
        Hp -= bc.Attack;

        if (Hp <= 0f)
        {
            Hp = 0f;
            OnDead();
        }
    }

    protected override void OnAttacked()
    {
        _lockTarget.OnDamaged(this);
    }
}