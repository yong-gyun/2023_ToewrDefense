using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define.UnitType;

public class Stage2_SpawnPool : SpawnPool
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.CurrentWave = 0;
        Managers.Game.MaxWave = 6;
        return true;
    }

    public override void OnStart()
    {
        base.OnStart();
        OnChangeWave(++Managers.Game.CurrentWave);
    }

    IEnumerator OnStartFirstWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.75f);

        for (int i = 0; i < 10; i++)
        {
            OnSpawn(MeleeUnit, 0);
            yield return wait;
        }

        for (int i = 0; i < 10; i++)
        {
            OnSpawn(MeleeUnit, 1);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(QuickMoveUnit, 0);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(QuickMoveUnit, 1);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(QuickMoveUnit, 0);
            yield return wait;
        }


        for (int i = 0; i < 5; i++)
        {
            OnSpawn(MeleeUnit, 1);
            yield return wait;
        }

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartSecondWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.75f);

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(QuickMoveUnit, 2);

            yield return wait;
        }

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(QuickMoveUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(QuickMoveUnit, 3);
            yield return wait;
        }

        OnSpawn(GoldUnit, 0);
        OnSpawn(GoldUnit, 3);
        yield return wait;

        for (int i = 0; i < 10; i++)
        {
            OnSpawn(RangedAttackUnit, 2);
            yield return wait;
        }

        for (int i = 0; i < 10; i++)
        {
            OnSpawn(RangedAttackUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 2; i++)
        {
            OnSpawn(FlyableUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 2; i++)
        {
            OnSpawn(FlyableUnit, 2);
            yield return wait;
        }

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartThirdWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(QuickMoveUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(QuickMoveUnit, 1);
            yield return wait;
        }

        for (int i = 0; i < 4; i++)
        {
            OnSpawn(GoldUnit, 1);
            yield return wait;
        }

        OnSpawn(RangedAttackUnit, 2);
        yield return wait;

        for (int i = 0; i < 2; i++)
        {
            OnSpawn(GoldUnit, 2);
            yield return wait;
        }

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(RangedAttackUnit, 2);
            yield return wait;
        }

        for (int i = 0; i < 4; i++)
        {
            OnSpawn(FlyableUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 4; i++)
        {
            OnSpawn(FlyableUnit, 1);
            yield return wait;
        }

        OnSpawn(MiddleBossUnit, 0);
        yield return wait;

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(RangedAttackUnit, 2);
            yield return wait;
        }

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartFourWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(MeleeUnit, 2);
            OnSpawn(MeleeUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(QuickMoveUnit, 2);
            OnSpawn(QuickMoveUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(RangedAttackUnit, 0);
            OnSpawn(RangedAttackUnit, 1);
            yield return wait;
        }

        for (int i = 0; i < 2; i++)
        {
            OnSpawn(GoldUnit, 2);
            OnSpawn(GoldUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 4; i++)
        {
            OnSpawn(FlyableUnit, 2);
            OnSpawn(FlyableUnit, 3);
            yield return wait;
        }

        OnSpawn(MiddleBossUnit, 1);
        OnSpawn(MiddleBossUnit, 3);
        yield return wait;

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartFifthWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.5f);

        for (int i = 0; i < 3; i++)
        {
            OnSpawn(MeleeUnit, 1);
            OnSpawn(MeleeUnit, 2);
            OnSpawn(MeleeUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 4; i++)
        {
            OnSpawn(QuickMoveUnit, 1);
            OnSpawn(QuickMoveUnit, 2);
            OnSpawn(QuickMoveUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 8; i++)
        {
            OnSpawn(RangedAttackUnit, 0);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(MeleeUnit, 0);
            OnSpawn(MeleeUnit, 1);
            OnSpawn(MeleeUnit, 3);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(FlyableUnit, 0);
            OnSpawn(FlyableUnit, 1);
            OnSpawn(FlyableUnit, 3);
            yield return wait;
        }

        OnSpawn(MiddleBossUnit, 0);
        OnSpawn(MiddleBossUnit, 1);
        yield return wait;

        for (int i = 0; i < 4; i++)
        {
            OnSpawn(RangedAttackUnit, 0);
            OnSpawn(MeleeUnit, 1);
            yield return wait;
        }

        for (int i = 0; i < 5; i++)
        {
            OnSpawn(QuickMoveUnit, 2);
            OnSpawn(QuickMoveUnit, 3);
            yield return wait;
        }

        OnCompletedSpawnCurrentWave();
    }

    IEnumerator OnStartSixthWave()
    {
        WaitForSeconds wait = new WaitForSeconds(1.25f);
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
        OnSpawn(StageBossUnit, Random.Range(0, 3));

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
            case 5:
                _couroutine = StartCoroutine(OnStartFifthWave());
                break;
            case 6:
                _couroutine = StartCoroutine(OnStartSixthWave());
                break;
        }
    }
}