using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define.UnitType;

public class Stage1_SpawnPool : SpawnPool
{
    //Stage1 : 5 wave 
    //근거리 20: / 00:30
    //근거리 20: 원거리 10: / 1:00
    //근거리 15: 원거리 10: 빠른 이동8: / 1:30
    //근거리 15: 원거리
    //Stage Boss
    //
    //Stage2 : 7 Wave

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.CurrentWave = 0;
        Managers.Game.MaxWave = 4;
        return true;
    }

    public override void OnStart()
    {
        base.OnStart();
        OnChangeWave(++Managers.Game.CurrentWave);
    }

    IEnumerator OnStartFirstWave()
    {
        OnSpawn(MeleeUnit, 0);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 0);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 0);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 1);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 1);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 1);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return new WaitForSeconds(4f);

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return new WaitForSeconds(4f);

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return new WaitForSeconds(2f);

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return new WaitForSeconds(2f);

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return new WaitForSeconds(2f);

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartSecondWave()
    {
        WaitForSeconds wait = new WaitForSeconds(2f);

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(MeleeUnit, 0);
            OnSpawn(MeleeUnit, 1);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(QuickMoveUnit, 0);
            OnSpawn(QuickMoveUnit, 1);
            yield return wait;
        }

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return wait;
        
        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return wait;

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return wait;

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartThirdWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(GoldUnit, 1);
        yield return wait;

        OnSpawn(GoldUnit, 0);
        yield return wait;

        OnSpawn(FlyableUnit, 0);
        OnSpawn(FlyableUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return wait;

        OnSpawn(MiddleBossUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(QuickMoveUnit, 0);
        OnSpawn(QuickMoveUnit, 1);
        yield return wait;

        OnSpawn(QuickMoveUnit, 0);
        OnSpawn(QuickMoveUnit, 1);
        yield return wait;

        OnSpawn(QuickMoveUnit, 0);
        OnSpawn(QuickMoveUnit, 1);
        yield return wait;

        OnSpawn(FlyableUnit, 0);
        OnSpawn(FlyableUnit, 1);
        yield return wait;

        OnSpawn(FlyableUnit, 0);
        OnSpawn(FlyableUnit, 1);
        yield return wait;

        OnSpawn(GoldUnit, 0);
        OnSpawn(GoldUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(MiddleBossUnit, 0);

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartFourWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(QuickMoveUnit, 0);
        OnSpawn(QuickMoveUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(QuickMoveUnit, 0);
        OnSpawn(QuickMoveUnit, 1);
        yield return wait;

        OnSpawn(QuickMoveUnit, 0);
        OnSpawn(QuickMoveUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(GoldUnit, 1);
        yield return wait;

        OnSpawn(GoldUnit, 0);
        yield return wait;

        OnSpawn(FlyableUnit, 0);
        OnSpawn(FlyableUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(RangedAttackUnit, 0);
        OnSpawn(RangedAttackUnit, 1);
        yield return wait;

        OnSpawn(MiddleBossUnit, 1);
        yield return wait;

        OnSpawn(MeleeUnit, 0);
        OnSpawn(MeleeUnit, 1);
        yield return wait;

        OnSpawn(StageBossUnit, 1);
        OnCompletedSpawnCurrentWave();
    }


    public override void OnChangeWave(int idx)
    {
        switch (idx)
        {
            case 1:
                _couroutine = StartCoroutine(OnStartFirstWave());
                break;
            case 2:
                _couroutine = StartCoroutine(OnStartSecondWave());
                break;
            case 3:
                _couroutine = StartCoroutine(OnStartThirdWave());
                break;
            case 4:
                _couroutine = StartCoroutine(OnStartFourWave());
                break;
        }
    }
}
