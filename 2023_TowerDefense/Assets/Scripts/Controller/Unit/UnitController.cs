using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : BaseController
{
    public virtual Define.State State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;

            if (_isAnimating == false)
                return;

            switch(_state)
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
            }
        }
    }

    [SerializeField] protected Define.State _state;

    public Define.Priority Priority { get { return _priority; } set { _priority = value; } }
    [SerializeField] protected Define.Priority _priority;

    public Define.UnitType Type { get { return _type; } set { _type = value; } }
    [SerializeField] protected Define.UnitType _type;
    
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public int RewardGold { get { return _rewardGold; } set { _rewardGold = value; } } 
    public int RewardScore { get { return _rewardScore; } set { _rewardScore = value; } }
    public bool IsAttackable { get; set; } = true;
    public bool IsMovable { get; set; } = true;
    [SerializeField] protected TowerController _lockTarget;
    [SerializeField] protected float _moveSpeed;
    protected float _attackInterval;
    [SerializeField] protected int _rewardScore;
    [SerializeField] protected int _rewardGold;
    
    protected bool _fireBullet;
    [SerializeField] protected bool _isAnimating;
    protected Animator _anim;
    protected NavMeshAgent _agent;
    protected Transform _firePos;
    protected static int _movePriority = 0;

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        WorldObjectType = Define.WorldObject.Unit;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _isAnimating = _anim != null;

        if(_agent != null)
        {
            _agent.angularSpeed = 360f;
            _agent.avoidancePriority = _movePriority++;
        }

        if (Managers.Object.IllusionTowers.Count == 0)
            Priority = Define.Priority.ProtectedTower;
        else
            Priority = Define.Priority.Illusion;

        WorldObjectType = Define.WorldObject.Unit;
        IsEnemy = true;
        tag = "Unit";
        UI_HPBar hpBar = GetComponentInChildren<UI_HPBar>();

        if (hpBar == null)
        {
            hpBar = Managers.UI.MakeWorldSpcaeUI<UI_HPBar>(transform);
            hpBar.SetController(this);
        }

        hpBar.transform.position = transform.position + (Vector3.up * GetComponent<Collider>().bounds.size.y * 1.2f);
        State = Define.State.Move;
        return true;
    }

    public void SetStat(Define.UnitType type)
    {
        Data.UnitStat stat = Managers.Data.UnitStatData[type];
        _type = type;
        _maxHp = stat.maxHp;
        _hp = stat.hp;
        _attack = stat.attack;
        _attackDelay = stat.attackDelay;
        _attackRange = stat.attackRange;
        _moveSpeed = stat.moveSpeed;
        _rewardScore = stat.rewardScore;
        _rewardGold = stat.rewardGold;
        _fireBullet = stat.fireBullet;

        if (_fireBullet)
            _firePos = gameObject.FindChild("FirePos", true).transform;
    }

    private void Start()
    {
        Init();
    }

    protected virtual void Update()
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
        }
    }

    protected virtual void UpdateSetTarget()
    {
        if(Managers.Object.ProtectedTowers.Count == 0 && Managers.Object.LastProtectedTower == null)
        {
            State = Define.State.Idle;
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, AttackRange, LayerMask.GetMask("Tower")))
        {
            TowerController tc = hit.transform.GetComponent<TowerController>();
            
            if (tc != null)
            {
                if(tc.Type == Define.TowerType.Obstacle || tc.Type == Define.TowerType.ProtectedTower)
                {
                    _lockTarget = tc;
                    return;
                }

                if(tc.Type == Define.TowerType.LastProtectedTower && Managers.Object.ProtectedTowers.Count == 0)
                {
                    _lockTarget = tc;
                    return;
                }
            }
        }

        switch (Priority)
        {
            case Define.Priority.Illusion:
                {
                    if (Managers.Object.IllusionTowers.Count == 0)
                    {
                        Priority = Define.Priority.ProtectedTower;
                        return;
                    }

                    if (_lockTarget == null || _lockTarget.Type != Define.TowerType.Illusion)
                    {
                        _lockTarget = Util.GetShortestDistance(gameObject, Managers.Object.IllusionTowers);
                    }
                }
                break;
            case Define.Priority.Tower:
                {
                    if (_lockTarget == null)
                        Priority = Define.Priority.ProtectedTower;
                }
                break;
            case Define.Priority.ProtectedTower:
                {
                    if (_lockTarget == null)
                    {
                        if(Managers.Object.ProtectedTowers.Count == 0)
                        {
                            if (Managers.Object.LastProtectedTower != null)
                            {
                                _lockTarget = Managers.Object.LastProtectedTower;
                                return;
                            }
                        }

                        _lockTarget = Util.GetShortestDistance(gameObject, Managers.Object.ProtectedTowers);
                    }
                }
                break;
        }

        if(_lockTarget != null)
            _attackInterval = AttackRange + _lockTarget.Size;
    }

    protected virtual void UpdateAttackToIdle()
    {
        if (_lockTarget != null && IsAttackable)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            
            if (dir.magnitude <= _attackInterval)
            {
                Quaternion qua = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);

                _currentAttackTime -= Time.deltaTime;
                
                if (_currentAttackTime <= 0f)
                {
                    _currentAttackTime = AttackDelay;

                    if (_isAnimating == false)
                    {
                        OnAttacked();
                        return;
                    }

                    State = Define.State.Attack;
                }

                return;
            }

            State = Define.State.Move;
            return;
        }
    }

    protected virtual void UpdateMove()
    {
        if(IsMovable == false || Cheat.IsEnemyMovealbe == false)
        {
            _agent.SetDestination(transform.position);
            return;
        }

        if (_lockTarget == null)
            return;

        Vector3 dir = _lockTarget.transform.position - transform.position;

        if (dir.magnitude <= _attackInterval)
        {
            State = Define.State.Attack_To_Idle;
            _agent.SetDestination(transform.position);
            return;
        }

        _agent.SetDestination(_lockTarget.transform.position);
        _agent.speed = MoveSpeed;
    }

    protected virtual void UpdateAttack()
    {
        if(_lockTarget == null)
        {
            State = Define.State.Move;
            return;
        }

        Vector3 dir = _lockTarget.transform.position - transform.position;

        if(dir.magnitude > _attackInterval)
        {
            State = Define.State.Move;
            return;
        }
    
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
    }

    protected virtual void OnAttacked()
    {
        if(_fireBullet)
        {
            GameObject go = Managers.Resource.Instantiate($"Bullet/{Type}Bullet");
            go.transform.position = _firePos.position;
            Bullet bullet = go.GetOrAddComponent<Bullet>();
            bullet.SetTarget(this, _lockTarget);
        }
        else
        {
            _lockTarget.OnDamaged(this);
        }

        State = Define.State.Attack_To_Idle;
    }

    public override void OnDamaged(BaseController bc)
    {
        Hp -= bc.Attack;
        Debug.Log(bc.name);
        if((int)Define.Priority.Tower > (int) Priority && bc is TowerController)
        {
            if (_lockTarget != null)
            {
                if (State != Define.State.Attack && State != Define.State.Attack_To_Idle)
                {
                    Priority = Define.Priority.Tower;
                    _lockTarget = bc as TowerController;
                }
            }
        }

        if (Hp <= 0f)
        {
            Hp = 0f;
            OnDead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BuildGrid"))
        {
            BuildGrid grid = other.GetComponent<BuildGrid>();
            grid.IsUnitOnTile = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BuildGrid"))
        {
            BuildGrid grid = other.GetComponent<BuildGrid>();
            grid.IsUnitOnTile = false;
        }
    }
}