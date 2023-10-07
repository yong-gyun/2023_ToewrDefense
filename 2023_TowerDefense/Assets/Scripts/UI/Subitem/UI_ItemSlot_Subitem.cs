using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot_Subitem : UI_Base
{
    enum Images
    {
        ItemIcon,
        CooltimeImage
    }

    public Define.ItemType Type { get { return _type; } }
    Define.ItemType _type = Define.ItemType.Unknow;
    public int Index { get; private set; }

    public void OnCooltime(float cooltime = 5f)
    {
        StartCoroutine(CoCooltime(cooltime));
    }

    IEnumerator CoCooltime(float cooltime = 5f)
    {
        float currentCooltime = cooltime;

        while(currentCooltime > 0)
        {
            currentCooltime -= Time.deltaTime;
            GetImage((int)Images.CooltimeImage).fillAmount = currentCooltime / cooltime;
            yield return null;
        }

        GetImage((int)Images.CooltimeImage).fillAmount = 0;
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));

        if (_type == Define.ItemType.Unknow)
            GetImage((int)Images.ItemIcon).gameObject.SetActive(false);

        return true;
    }

    public void SetInfo(int idx)
    {
        _type = Managers.Game.Items[idx];
        Index = idx;

        if (_type == Define.ItemType.Unknow)
        {
            GetImage((int)Images.ItemIcon).gameObject.SetActive(false);
            return;
        }

        GetImage((int)Images.ItemIcon).gameObject.SetActive(true);
        GetImage((int)Images.ItemIcon).sprite = Managers.Resource.Load<Sprite>($"Sprites/Icon/Item/{_type}");
    }

    public void Clear()
    {
        Managers.Game.Items[Index] = Define.ItemType.Unknow;
        _type = Managers.Game.Items[Index];
        GetImage((int)Images.ItemIcon).gameObject.SetActive(false);
    }
}
