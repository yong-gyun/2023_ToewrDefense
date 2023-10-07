using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionTowerController : TowerController
{
    public override void OnStart()
    {
        base.OnStart();
        StartCoroutine(CoDisappear());


        foreach (UnitController uc in Managers.Object.EnemyUnits)
            uc.Priority = Define.Priority.Illusion;
    }

    IEnumerator CoDisappear()
    {
        float maxTime = 30f;
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime / maxTime;
            _hp = Mathf.Lerp(0f, _maxHp, t);
            yield return null;
        }

        OnDead();
    }

    public override void OnDamaged(BaseController bc)
    {
        
    }
}