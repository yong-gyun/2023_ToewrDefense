using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : BaseController
{
    public GameObject AttackRangeViewer { get; protected set; }
    public int Size { get { return _size; } }
    public Define.TowerType Type { get { return _type; } set { _type = value; } }
    public bool IsStart { get; private set; } = false;
    [SerializeField] Define.TowerType _type;
    [SerializeField] int _size;
    protected Transform _firePos;
    [SerializeField] protected Transform _lockTarget;
    
    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        GameObject go = gameObject.FindChild("FirePos", true);
        if (go != null)
            _firePos = go.transform;

        WorldObjectType = Define.WorldObject.Tower;

        IsEnemy = false;
        UI_HPBar hpBar = GetComponentInChildren<UI_HPBar>();

        if (hpBar == null)
        {
            hpBar = Managers.UI.MakeWorldSpcaeUI<UI_HPBar>(transform);
            hpBar.SetController(this);
        }

        hpBar.transform.position = transform.position + (Vector3.up * GetComponent<Collider>().bounds.size.y * 1.25f);
        return true;
    }

    public void SetStat(Define.TowerType type)
    {
        Data.TowerStat stat = Managers.Data.TowerStatData[type];
        _type = type;
        _maxHp = stat.maxHp;
        _hp = stat.hp;
        _attack = stat.attack;
        _attackDelay = stat.attackDelay;
        _attackRange = stat.attackRange;
        _size = stat.size;
    }

    public void SetAttackRangeViewer(GameObject go)
    {
        AttackRangeViewer = go;
        AttackRangeViewer.transform.localPosition = new Vector3(0f, -0.1f, 0f);
        go.SetActive(false);
    }

    public virtual void OnStart()
    {
        Init();
        IsStart = true;
    }

    protected virtual void Update()
    {
        if (IsStart == false)
            return;

        if (Type == Define.TowerType.Obstacle)
            return;

        if (_lockTarget == null)
            UpdateFind();
        else
            UpdateAttack();
    }

    protected virtual void UpdateFind()
    {
        float maxDistance = Mathf.Infinity;

        foreach (UnitController unit in Managers.Object.EnemyUnits)
        {
            Vector3 dir = (unit.transform.position - transform.position);
            dir.y = 0f;
            float distance = dir.magnitude;

            if (distance < maxDistance && distance <= AttackRange && unit.State != Define.State.Die)
            {
                if (Physics.Raycast(transform.position, dir.normalized, distance, LayerMask.GetMask("Block")))
                    continue;

                maxDistance = distance;
                _lockTarget = unit.transform;
            }
        }
    }

    protected virtual void UpdateAttack()
    {
        _currentAttackTime -= Time.deltaTime;

        Vector3 dir = (_lockTarget.position - transform.position);
        dir.y = 0f;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
        
        if(_currentAttackTime <= 0f)
        {
            _currentAttackTime = _attackDelay;
            OnAttacked();
        }
    }

    protected virtual void OnAttacked()
    {
        if (!Cheat.IsTowerAttackable)
            return;

        if (Type == Define.TowerType.Focus)
            Managers.Sound.Play("Tower/FocusTowerAttack");
        else
            Managers.Sound.Play("Tower/TowerAttack");
        GameObject go = Managers.Resource.Instantiate($"Bullet/{Type}Bullet", _firePos.position, Quaternion.identity);
        Bullet bullet = go.GetOrAddComponent<Bullet>();
        bullet.SetTarget(this, _lockTarget.GetComponent<BaseController>());
    }

    public override void OnDamaged(BaseController bc)
    {
        if (IsStart == false)
            return;

        Hp -= bc.Attack;

        if(Hp <= 0f)
        {
            Hp = 0f;
            OnDead();
        }
    }
}