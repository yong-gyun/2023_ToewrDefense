using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    public static bool IsTowerAttackable = false; 
    public static bool IsEnemyMovealbe = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            IsEnemyMovealbe = !IsEnemyMovealbe;

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            string message = "";
            if (IsEnemyMovealbe)
                message = "모든 적 이동 중단 (ON)";
            else
                message = "모든 적 이동 중단 (OFF)";

            alert.SetAlert(message, 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Managers.Game.CurrentGold += 1000;
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("골드 증가 1000", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            int goldAcquisitionAmount = Managers.Game.GoldAcquisitionAmount;
            Managers.Game.GoldAcquisitionAmount = 0;

            foreach (UnitController unit in Managers.Object.EnemyUnits)
                unit.OnDead();

            Managers.Game.GoldAcquisitionAmount = goldAcquisitionAmount;

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("모든 적 사망(골드 획득X)", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (UnitController unit in Managers.Object.EnemyUnits)
                unit.OnDead();

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("모든 적 사망(골드 획득O)", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            Managers.Scene.Load(Define.SceneType.Menu);
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("메뉴 화면으로 전환", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            Managers.Scene.Load(Define.SceneType.Stage1);
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("스테이지 1로 전환", 0.5f);
        }
        else if(Input.GetKeyDown(KeyCode.F7))
        {
            Managers.Scene.Load(Define.SceneType.Stage2);
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("스테이지 2로 전환", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            Managers.Object.SummonPatrolUnit(true);

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("순찰 유닛 생성", 0.5f);
        }
        else if(Input.GetKeyDown(KeyCode.F9))
        {
            IsTowerAttackable = !IsTowerAttackable;

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            string message = "";
            if (IsEnemyMovealbe)
                message = "모든 타워 공격 중단 (ON)";
            else
                message = "모든 타워 공격 중단 (OFF)";

            alert.SetAlert(message, 0.5f);
        }
    }
}
