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
                towerName = "기본 타워";
                description = $"{stat.attackRange}M 이내 1개의 타겟을 향해 공격 한다.";
                break;
            case Define.TowerType.Multiply:
                towerName = "다중 공격 타워";
                description = $"{stat.attackRange}M 이내 5개의 타겟을 공격 한다.";
                break;
            case Define.TowerType.Focus:
                towerName = "집중 공격 타워";
                description = $"{stat.attackRange}M 이내의 타겟을 중심으로 3M 모든 적에게 대미지를 준다."; 
                break;
            case Define.TowerType.Illusion:
                towerName = "환영 보호 시설";
                description = $"적들이 해당 타워를 우선 공격 한다.(30초 후 소멸)";
                break;
            case Define.TowerType.Obstacle:
                towerName = "장애물";
                description = $"적들이 지나가는 것을 방해 할 수 있다";
                break;
        }

        GetText((int)Texts.DescriptionText).text = 
@$"{towerName}
체력: {stat.maxHp}
공격력: {stat.attack}
공격 시전 시간: {stat.attackDelay}
사거리: {stat.attackRange}
가격: {stat.price}
크기: {stat.size} {Environment.NewLine}
{description}";
    }
}