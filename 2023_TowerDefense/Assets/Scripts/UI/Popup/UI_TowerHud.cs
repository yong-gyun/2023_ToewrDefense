using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TowerHud : UI_Popup
{
    enum Texts
    {
        StatText,
        HPText,
        NameText
    }

    enum Scrollbars
    {
        HPBar
    }

    TowerController _tc;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindScrollbar(typeof(Scrollbars));
        return true;
    }

    public void SetInfo(TowerController tc)
    {
        string towerName = "";
        _tc = tc;

        switch (tc.Type)
        {
            case Define.TowerType.Default:
                towerName = "기본 타워";
                break;
            case Define.TowerType.Multiply:
                towerName = "다중 공격 타워";
                break;
            case Define.TowerType.Focus:
                towerName = "집중 공격 타워";
                break;
            case Define.TowerType.Illusion:
                towerName = "환영 보호 시설";
                break;
            case Define.TowerType.Obstacle:
                towerName = "장애물";
                break;
            case Define.TowerType.ProtectedTower:
                towerName = "보호 시설";
                break;
            case Define.TowerType.LastProtectedTower:
                towerName = "최종 보호 시설";
                break;
        }

        GetText((int)Texts.NameText).text = towerName;
        GetText((int)Texts.StatText).text =
@$"공격력: {tc.Attack}
공격 시간: {tc.AttackDelay}
사거리: {tc.AttackRange}";
    }

    void Update()
    {
        if(_tc == null)
        {
            ClosePopupUI();
            return;
        }

        GetScrollbar((int)Scrollbars.HPBar).size = _tc.Hp / _tc.MaxHp;
        GetText((int)Texts.HPText).text = $"{_tc.Hp} / {_tc.MaxHp}";
    }
}
