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
                message = "��� �� �̵� �ߴ� (ON)";
            else
                message = "��� �� �̵� �ߴ� (OFF)";

            alert.SetAlert(message, 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Managers.Game.CurrentGold += 1000;
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("��� ���� 1000", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            int goldAcquisitionAmount = Managers.Game.GoldAcquisitionAmount;
            Managers.Game.GoldAcquisitionAmount = 0;

            foreach (UnitController unit in Managers.Object.EnemyUnits)
                unit.OnDead();

            Managers.Game.GoldAcquisitionAmount = goldAcquisitionAmount;

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("��� �� ���(��� ȹ��X)", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (UnitController unit in Managers.Object.EnemyUnits)
                unit.OnDead();

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("��� �� ���(��� ȹ��O)", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            Managers.Scene.Load(Define.SceneType.Menu);
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("�޴� ȭ������ ��ȯ", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            Managers.Scene.Load(Define.SceneType.Stage1);
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("�������� 1�� ��ȯ", 0.5f);
        }
        else if(Input.GetKeyDown(KeyCode.F7))
        {
            Managers.Scene.Load(Define.SceneType.Stage2);
            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("�������� 2�� ��ȯ", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            Managers.Object.SummonPatrolUnit(true);

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            alert.SetAlert("���� ���� ����", 0.5f);
        }
        else if(Input.GetKeyDown(KeyCode.F9))
        {
            IsTowerAttackable = !IsTowerAttackable;

            UI_MiddleAlert alert = Managers.UI.MakeEffectUI<UI_MiddleAlert>();
            string message = "";
            if (IsEnemyMovealbe)
                message = "��� Ÿ�� ���� �ߴ� (ON)";
            else
                message = "��� Ÿ�� ���� �ߴ� (OFF)";

            alert.SetAlert(message, 0.5f);
        }
    }
}
