using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager
{
    public List<BuildGrid> Grids { get; private set; } = new List<BuildGrid>();
    public List<UnitController> EnemyUnits { get { _enemyUnits.RemoveAll(uc => uc == null); return _enemyUnits; } }
    public List<ProtectedTowerController> ProtectedTowers { get { _protectedTowers.RemoveAll(pb => pb == null); return _protectedTowers; } }
    public List<IllusionTowerController> IllusionTowers { get { _illusionTowers.RemoveAll(it => it == null); return _illusionTowers; } }
    public List<PatrolUnitController> PatrolUnits { get { _patrolUnits.RemoveAll(uc => uc == null); return _patrolUnits; } }
    public List<Vector3> EnemySpawnPoints { get; private set; } = new List<Vector3>();
    public ProtectedTowerController LastProtectedTower { get { return _lastProtectedTower; } }
    public Transform TowerRoot
    {
        get
        {
            if (_towerRoot == null)
                _towerRoot = new GameObject("@Tower_Root").transform;
            return _towerRoot;
        }
    }

    public SpawnPool SpawnPool
    {
        get
        {
            if (_spawnPool == null)
                _spawnPool = Object.FindObjectOfType<SpawnPool>();

            return _spawnPool;
        }
    }

    public bool IsBuild { get; set; } = false;

    List<UnitController> _enemyUnits = new List<UnitController>();
    List<ProtectedTowerController> _protectedTowers = new List<ProtectedTowerController>();
    List<IllusionTowerController> _illusionTowers = new List<IllusionTowerController>();
    List<PatrolUnitController> _patrolUnits = new List<PatrolUnitController>();
    ProtectedTowerController _lastProtectedTower;
    SpawnPool _spawnPool;
    Transform _mapRoot;
    Transform _towerRoot;
    
    public GameObject IninMap()
    {
        if(_mapRoot == null)
            _mapRoot = new GameObject("@Map_Root").transform;
        
        GameObject go = GameObject.Find($"Stage{Managers.Game.CurrentStage}_Map");
        _mapRoot.transform.parent = go.transform;

        if (go == null)
            return null;

        BuildGrid[] buildGrids = go.GetComponentsInChildren<BuildGrid>();
        Grids = buildGrids.ToList();

        foreach (BuildGrid grid in Grids)
            grid.OnActive(false);

        Transform bridgeRoot = go.FindChild("Bridge_Root", true).transform;

        for (int i = 0; i < bridgeRoot.childCount; i++)
            EnemySpawnPoints.Add(bridgeRoot.GetChild(i).position + Vector3.up);

        Transform protectedTowerRoot = go.FindChild("ProtectedTowers", true).transform;

        for (int i = 0; i < protectedTowerRoot.childCount; i++)
            _protectedTowers.Add(protectedTowerRoot.GetChild(i).GetComponent<ProtectedTowerController>());

        Transform lastProtectedTower = go.FindChild<Transform>("LastProtectedTower", true);

        if(lastProtectedTower != null)
            _lastProtectedTower = lastProtectedTower.GetComponent<ProtectedTowerController>();

        return go;
    }

    public TowerController BuildTower(Define.TowerType type)
    {
        GameObject go = Managers.Resource.Instantiate($"Tower/{type}", TowerRoot);
        TowerController tc = go.GetOrAddComponent<TowerController>();
        tc.SetStat(type);

        if(type == Define.TowerType.Illusion)
            _illusionTowers.Add(tc as IllusionTowerController);

        if(type == Define.TowerType.ProtectedTower)
            _protectedTowers.Add(tc as ProtectedTowerController);

        if (type == Define.TowerType.LastProtectedTower)
            _lastProtectedTower = tc as ProtectedTowerController;
        return tc;
    }

    public UnitController SpawnUnit(Define.UnitType type)
    {
        GameObject go = Managers.Resource.Instantiate($"Unit/{type}");

        if (go == null)
            return null;
        
        UnitController uc = go.GetOrAddComponent<UnitController>();
        
        uc.SetStat(type);
        if (type != Define.UnitType.PatrolUnit)
            EnemyUnits.Add(uc);
        else
            PatrolUnits.Add(uc as PatrolUnitController);
        return uc;
    }
    
    public UnitController SummonPatrolUnit(bool isTimeLimit, float duration = 60f)
    {
        UnitController uc = SpawnUnit(Define.UnitType.PatrolUnit);
        PatrolUnitController patrolUnit = uc as PatrolUnitController;

        if (patrolUnit == null)
            return null;

        if(ProtectedTowers.Count == 0)
        {
            int idx = Random.Range(0, _lastProtectedTower.PatrolUnitSpawnPoints.Count);

            patrolUnit.transform.position = _lastProtectedTower.PatrolUnitSpawnPoints[idx].position;
            patrolUnit.SetTower(_lastProtectedTower);
        }
        else
        {
            int idx = Random.Range(0, ProtectedTowers.Count);
            int posIdx = Random.Range(0, ProtectedTowers[idx].PatrolUnitSpawnPoints.Count);

            Vector3 pos = ProtectedTowers[idx].PatrolUnitSpawnPoints[posIdx].position;
            patrolUnit.transform.position = pos;
            patrolUnit.SetTower(ProtectedTowers[idx]);
        }

        patrolUnit.transform.position += Vector3.up * 8f;
        patrolUnit.IsTimeLimitUnit = isTimeLimit;

        if(isTimeLimit)
        {
            patrolUnit.MaxHp = duration;
            patrolUnit.Hp = duration;
        }
        else
        {
            patrolUnit.MaxHp = Mathf.Infinity;
            patrolUnit.Hp = Mathf.Infinity;
        }


        return patrolUnit;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject worldObjectType = GetWorldObjectType(go);
        
        if (worldObjectType == Define.WorldObject.Tower)
        {
            TowerController tc = go.GetComponent<TowerController>();
            Managers.Sound.Play("Tower/TowerDestroy");

            switch (tc.Type)
            {
                case Define.TowerType.ProtectedTower:
                    {
                        ProtectedTowers.Remove(tc as ProtectedTowerController);

                        if (ProtectedTowers.Count == 0 && LastProtectedTower == null)
                        {
                            Managers.Game.OnGameResult(false);
                        }
                    }
                    break;
                case Define.TowerType.LastProtectedTower:
                    {
                        Managers.Game.OnGameResult(false);
                    }
                    break;
            }
        }
        else if (worldObjectType == Define.WorldObject.Unit)
        {
            UnitController uc = go.GetComponent<UnitController>();

            int rewardGold = uc.RewardGold * Managers.Game.GoldAcquisitionAmount * (Managers.Game.IsStageEnd == false ? 1 : 0);
            int rewardScore = uc.RewardScore * (Managers.Game.IsStageEnd == false ? 1 : 0);

            Managers.Game.CurrentGold += rewardGold;
            Managers.Game.CurrentScore += rewardScore;

            UI_Main mainUI = Managers.UI.SceneUI as UI_Main;

            string name = "";

            switch(uc.Type)
            {
                case Define.UnitType.MeleeUnit:
                    name = "±ÙÁ¢ °ø°Ý À¯´Ö";
                    break;
                case Define.UnitType.RangedAttackUnit:
                    name = "¿ø°Å¸® °ø°Ý À¯´Ö";
                    break;
                case Define.UnitType.QuickMoveUnit:
                    name = "ºü¸¥ ÀÌµ¿ À¯´Ö";
                    break;
                case Define.UnitType.FlyableUnit:
                    name = "°øÁß À¯´Ö";
                    break;
                case Define.UnitType.MiddleBossUnit:
                    name = "Áß°£ º¸½º À¯´Ö";
                    break;
                case Define.UnitType.GoldUnit:
                    name = "°ñµå À¯´Ö";
                    break;
                case Define.UnitType.StageBossUnit:
                    name = "½ºÅ×ÀÌÁö º¸½º À¯´Ö";
                    break;
            }

            if (uc.Type == Define.UnitType.StageBossUnit)
                Managers.Game.IsStageClear = true;

            mainUI.AddUsedAlert($"{name}À»/¸¦ Ã³Ä¡ÇÏ¿© {rewardGold}G È¹µæ");
        }

        Managers.Resource.Destory(go);
    }

    Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();

        if (bc == null)
            return Define.WorldObject.Unknow;

        return bc.WorldObjectType;
    }

    public void Clear()
    {
        Grids.Clear();
        EnemyUnits.Clear();
        ProtectedTowers.Clear();
        PatrolUnits.Clear();
        EnemySpawnPoints.Clear();
        IllusionTowers.Clear();
        _lastProtectedTower = null;
        _mapRoot = null;
        IsBuild = false;
    }
}
