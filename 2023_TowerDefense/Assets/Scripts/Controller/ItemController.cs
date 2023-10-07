using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{ 
    public float CurrentTime { get; private set; }
    public float MaxTime { get; private set; }
    Coroutine _coroutine;
    Define.ItemType _type;

    public void OnUsed(Define.ItemType type)
    {
        _type = type;
        Managers.Game.UsedItems[_type] = this;

        switch (type)
        {
            case Define.ItemType.TowerHeal:
                {
                    foreach (TowerController tc in Managers.Object.TowerRoot.GetComponentsInChildren<TowerController>())
                    {
                        tc.Hp += 30 % tc.MaxHp;

                        if (tc.MaxHp < tc.Hp)
                            tc.Hp = tc.MaxHp;
                    }

                    Clear();
                }
                break;
            case Define.ItemType.AllEnemysSlow: 
                {
                    foreach (UnitController uc in Managers.Object.EnemyUnits)
                        uc.MoveSpeed = uc.MoveSpeed / 2f;
                    ForWait(10f, () =>
                    {
                        foreach (UnitController uc in Managers.Object.EnemyUnits)
                            uc.MoveSpeed = Managers.Data.UnitStatData[uc.Type].moveSpeed;
                    }
                    );
                }
                break;
            case Define.ItemType.GoldAcquisition:
                {
                    Managers.Game.GoldAcquisitionAmount = 2;
                    ForWait(60f, () => { Managers.Game.GoldAcquisitionAmount = 1; } );
                }
                break;
            case Define.ItemType.AllEnemysAttackStop:
                {
                    foreach (UnitController uc in Managers.Object.EnemyUnits)
                        uc.IsAttackable = false;
                    ForWait(10f, () =>
                    {
                        foreach (UnitController uc in Managers.Object.EnemyUnits)
                            uc.IsAttackable = true;
                    });
                }
                break;
            case Define.ItemType.TowerAttackSpeedInc:
                {
                    foreach (TowerController tc in Managers.Object.TowerRoot.GetComponentsInChildren<TowerController>())
                        tc.AttackDelay /= 2f;

                    ForWait(10f,
                        () =>
                        {
                            foreach (TowerController tc in Managers.Object.TowerRoot.GetComponentsInChildren<TowerController>())
                                tc.AttackDelay = Managers.Data.TowerStatData[tc.Type].attackDelay;
                        });
                }
                break;
            case Define.ItemType.CreatePatrolUnit:
                {
                    Managers.Object.SummonPatrolUnit(true, 60f); 
                    Clear();
                }
                break;
        }
    }

    void ForWait(float t, Action evt)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CoForWait(t, evt));
    }

    IEnumerator CoForWait(float t, Action evt)
    {
        MaxTime = t;
        CurrentTime = 0f;
        
        while(CurrentTime < MaxTime)
        {
            CurrentTime += Time.deltaTime;
            yield return null;
        }

        CurrentTime = MaxTime;
        evt.Invoke();
        Clear();
    }

    void Clear()
    {
        Managers.Game.UsedItems[_type] = null;
        Managers.Resource.Destory(gameObject);
    }
}