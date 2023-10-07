using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplyTowerController : TowerController
{
    List<UnitController> _targets = new List<UnitController>();

    protected override void Update()
    {
        if (IsStart == false)
            return;

        UpdateFind();
        UpdateAttack();
    }

    protected override void UpdateFind()
    {
        _targets.RemoveAll(uc => uc == null || (uc.transform.position - transform.position).magnitude > AttackRange || uc.State == Define.State.Die);

        foreach (UnitController unit in Managers.Object.EnemyUnits)
        {
            Vector3 dir = (unit.transform.position - transform.position);
            dir.y = 0f;
            float distance = dir.magnitude;

            if (Physics.Raycast(transform.position, dir, AttackRange, LayerMask.GetMask("Block")))
                continue;
            if(distance <= AttackRange && _targets.Contains(unit) == false && unit.State != Define.State.Die)
                _targets.Add(unit);
        }

        _targets.OrderBy(uc => (uc.transform.position - transform.position).magnitude);
    }

    protected override void UpdateAttack()
    {
        if (_targets.Count == 0)
        {
            _currentAttackTime = _attackDelay;
            return;
        }
        
        _currentAttackTime -= Time.deltaTime;

        if(_currentAttackTime <= 0f)
        {
            _currentAttackTime = _attackDelay;
            OnAttacked();
        }

        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.position - transform.position;
            dir.y = 0f;
            Quaternion qua = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, qua, 20 * Time.deltaTime);
        }

    }

    protected override void OnAttacked()
    {
        _targets.RemoveAll(uc => uc == null || (uc.transform.position - transform.position).magnitude > AttackRange || uc.State == Define.State.Die);

        if (_targets.Count == 0)
            return;

        for (int i = 0; i < 5; i++)
        {
            if (i == _targets.Count)
                return;

            Managers.Sound.Play("Tower/TowerAttack");
            UnitController uc = _targets[i];

            if (uc == null)
                continue;
            _lockTarget = uc.transform;
            GameObject go = Managers.Resource.Instantiate($"Bullet/{Type}Bullet");
            go.transform.position = _firePos.position;
            Bullet bullet = go.GetOrAddComponent<Bullet>();
            bullet.SetTarget(this, uc);
        }
    }
}