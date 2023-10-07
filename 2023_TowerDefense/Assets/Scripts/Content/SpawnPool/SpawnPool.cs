using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Define;

public abstract class SpawnPool : MonoBehaviour
{
    #region Data

    protected class EnemySpawnData
    {
        public UnitType Type;
        public int SpawnIdx;

        public EnemySpawnData(UnitType type, int spawnIdx)
        {
            Type = type;
            SpawnIdx = spawnIdx;
        }
    }
    #endregion

    public bool IsStarted { get; protected set; }
    protected Coroutine _couroutine;
    protected bool _isLastWave;
    bool _init;

    protected virtual void Start()
    {
        Init();
    }

    protected virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    public virtual void OnStart()
    {
        IsStarted = true;
        Managers.Object.SummonPatrolUnit(false);
        Managers.Sound.Play($"Stage{Managers.Game.CurrentStage}", Sound.Bgm);
    }

    protected virtual void OnSpawn(UnitType type, int idx)
    {
        UnitController uc = Managers.Object.SpawnUnit(type);
        uc.transform.parent = transform;

        NavMeshAgent nma = uc.GetComponent<NavMeshAgent>();

        if (nma != null)
        {
            nma.enabled = false;
            uc.transform.position = Managers.Object.EnemySpawnPoints[idx];
            nma.enabled = true;
        }
        else
        {
            uc.transform.position = Managers.Object.EnemySpawnPoints[idx];
        }

        if (uc.Type == UnitType.FlyableUnit)
            uc.transform.position += Vector3.up * 5f;
    }

    protected virtual void OnCompletedSpawnCurrentWave()
    {
        if (Managers.Game.CurrentWave != Managers.Game.MaxWave)
            Managers.UI.ShowPopupUI<UI_NextWavePopup>();
        else
            StartCoroutine(CoUpdate());
    }

    public abstract void OnChangeWave(int idx);

    IEnumerator CoUpdate()
    {
        while (true)
        {
            if (Managers.Object.EnemyUnits.Count == 0)
            {
                Managers.Game.OnGameResult(true);
                break;
            }
            
            yield return null;
        }
    }
}