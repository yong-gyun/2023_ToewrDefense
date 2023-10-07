using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    BaseController _launchedObject;
    BaseController _lockTarget;
    float _moveSpeed = 20f;
    bool _init = false;
    
    public void SetTarget(BaseController launchedObject, BaseController lockTarget)
    {
        _init = true;
        _launchedObject = launchedObject;
        _lockTarget = lockTarget;
    }

    private void Update()
    {
        if (_init == false)
            return;

        if(_lockTarget == null)
        {
            Managers.Resource.Destory(gameObject);
            return;
        }

        transform.LookAt(_lockTarget.transform);
        Vector3 dir = _lockTarget.transform.position - transform.position;
        transform.position += dir.normalized * _moveSpeed * Time.deltaTime;
    }

    void OnAttacked()
    {
        _lockTarget.OnDamaged(_launchedObject);
        Managers.Resource.Destory(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_init == false)
            return;

        if(other.gameObject == _lockTarget.gameObject)
        {
            if (_launchedObject.WorldObjectType == Define.WorldObject.Tower)
            {
                TowerController tc = _launchedObject as TowerController;

                if (tc.Type == Define.TowerType.Focus)
                {
                    foreach (UnitController uc in Managers.Object.EnemyUnits)
                    {
                        float distance = (uc.transform.position - transform.position).magnitude;

                        if (distance <= Util.GetDistance(3f))
                        {
                            uc.OnDamaged(_launchedObject);
                        }
                    }

                    return;
                }
            }
            
            OnAttacked();
        }
    }
}