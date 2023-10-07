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
                towerName = "�⺻ Ÿ��";
                break;
            case Define.TowerType.Multiply:
                towerName = "���� ���� Ÿ��";
                break;
            case Define.TowerType.Focus:
                towerName = "���� ���� Ÿ��";
                break;
            case Define.TowerType.Illusion:
                towerName = "ȯ�� ��ȣ �ü�";
                break;
            case Define.TowerType.Obstacle:
                towerName = "��ֹ�";
                break;
            case Define.TowerType.ProtectedTower:
                towerName = "��ȣ �ü�";
                break;
            case Define.TowerType.LastProtectedTower:
                towerName = "���� ��ȣ �ü�";
                break;
        }

        GetText((int)Texts.NameText).text = towerName;
        GetText((int)Texts.StatText).text =
@$"���ݷ�: {tc.Attack}
���� �ð�: {tc.AttackDelay}
��Ÿ�: {tc.AttackRange}";
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
