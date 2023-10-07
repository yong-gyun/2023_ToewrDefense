using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemUsed_Subitem : UI_Base
{
    enum Texts
    {
        DescriptionText,
        CooltimeText
    }

    enum GameObjets
    {
        Description
    }

    enum Images
    {
        IconImage,
        CooltimeImage
    }

    ItemController _item;

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindText(typeof(Texts));
        Bind<GameObject>(typeof(GameObjets));
        BindImage(typeof(Images));
        _item = gameObject.GetOrAddComponent<ItemController>();
        gameObject.BindEvent((evtData) => { Get<GameObject>((int)GameObjets.Description).SetActive(true); }, Define.UIEvent.PointerEnter);
        gameObject.BindEvent((evtData) => { Get<GameObject>((int)GameObjets.Description).SetActive(false); }, Define.UIEvent.PointerExit);
        Get<GameObject>((int)GameObjets.Description).SetActive(false);
        return true;
    }

    public void OnUsed(Define.ItemType type)
    {
        Init();

        string description = "";

        switch(type)
        {
            case Define.ItemType.TowerHeal:
                description = "Ÿ�� ü�� 30% ȸ��";
                break;
            case Define.ItemType.AllEnemysSlow:
                description = "10�ʰ� ��� ���� �̵� �ӵ� 50% ����";
                break;
            case Define.ItemType.GoldAcquisition:
                description = "1�а� ��� ȹ�淮�� �ι�� ����";
                break;
            case Define.ItemType.TowerAttackSpeedInc:
                description = "10�ʰ� Ÿ���� ���� ������ 50% ����";
                break;
            case Define.ItemType.AllEnemysAttackStop:
                description = "10�ʰ� ��� ���� ���� ����";
                break;
            case Define.ItemType.CreatePatrolUnit:
                description = "���� ���� ���� (1�� �� ����)";
                break;
        }

        GetImage((int)Images.IconImage).sprite = Managers.Resource.Load<Sprite>($"Sprites/Icon/Item/{type}");
        GetText((int)Texts.DescriptionText).text = description;
        _item.OnUsed(type);
    }

    private void Update()
    {
        GetImage((int)Images.CooltimeImage).fillAmount = _item.CurrentTime / _item.MaxTime;
        GetText((int)Texts.CooltimeText).text = string.Format("{0:0}", _item.MaxTime - _item.CurrentTime);
    }
}