using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedTowerController : TowerController
{
    public List<Transform> PatrolPoints { get; set; } = new List<Transform>(); 
    public List<Transform> PatrolUnitSpawnPoints { get; set; } = new List<Transform>();

    private void Awake()
    {
        OnStart();
    }

    public override void OnStart()
    {
        base.OnStart();

        Transform spawnPoint = gameObject.FindChild("SpawnPoints").transform;

        for (int i = 0; i < spawnPoint.childCount; i++)
            PatrolUnitSpawnPoints.Add(spawnPoint.GetChild(i));

        Transform patrolPoint = gameObject.FindChild("PatrolPoints").transform;

        for (int i = 0; i < patrolPoint.childCount; i++)
            PatrolPoints.Add(patrolPoint.GetChild(i));
        if (gameObject.name == "LastProtectedTower")
            SetStat(Define.TowerType.LastProtectedTower);
        else
            SetStat(Define.TowerType.ProtectedTower);
    }

    protected override void Update()
    {

    }
}
