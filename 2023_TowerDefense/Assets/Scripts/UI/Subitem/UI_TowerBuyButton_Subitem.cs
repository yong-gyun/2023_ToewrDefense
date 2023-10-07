using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TowerBuyButton_Subitem : UI_Base
{
    enum Texts
    {
        DescriptionText
    }

    enum GameObjects
    {
        DescriptionObject
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        gameObject.BindEvent((evt) => { Get<GameObject>((int)GameObjects.DescriptionObject).SetActive(true); }, Define.UIEvent.PointerEnter);
        gameObject.BindEvent((evt) => { Get<GameObject>((int)GameObjects.DescriptionObject).SetActive(false); }, Define.UIEvent.PointerExit);
        gameObject.BindEvent((evt) => { Get<GameObject>((int)GameObjects.DescriptionObject).SetActive(false); }, Define.UIEvent.Click);
        Get<GameObject>((int)GameObjects.DescriptionObject).SetActive(false);
        return true;
    }

    public void SetInfo(Define.TowerType type)
    {
        Init();
        Data.TowerStat stat = Managers.Data.TowerStatData[type];
        string towerName = "";
        string description = "";
        switch(type)
        {
            case Define.TowerType.Default:
                towerName = "�⺻ Ÿ��";
                description = $"{stat.attackRange}M �̳� 1���� Ÿ���� ���� ���� �Ѵ�.";
                break;
            case Define.TowerType.Multiply:
                towerName = "���� ���� Ÿ��";
                description = $"{stat.attackRange}M �̳� 5���� Ÿ���� ���� �Ѵ�.";
                break;
            case Define.TowerType.Focus:
                towerName = "���� ���� Ÿ��";
                description = $"{stat.attackRange}M �̳��� Ÿ���� �߽����� 3M ��� ������ ������� �ش�."; 
                break;
            case Define.TowerType.Illusion:
                towerName = "ȯ�� ��ȣ �ü�";
                description = $"������ �ش� Ÿ���� �켱 ���� �Ѵ�.(30�� �� �Ҹ�)";
                break;
            case Define.TowerType.Obstacle:
                towerName = "��ֹ�";
                description = $"������ �������� ���� ���� �� �� �ִ�";
                break;
        }

        GetText((int)Texts.DescriptionText).text = 
@$"{towerName}
ü��: {stat.maxHp}
���ݷ�: {stat.attack}
���� ���� �ð�: {stat.attackDelay}
��Ÿ�: {stat.attackRange}
����: {stat.price}
ũ��: {stat.size} {Environment.NewLine}
{description}";
    }
}