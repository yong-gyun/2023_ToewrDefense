using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ProtectedTowerHp_Subitem : UI_Base
{
    enum Scrollbars
    {
        HPBar
    }

    enum Texts
    { 
        NameText
    }

    [SerializeField] ProtectedTowerController _ptc;

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindScrollbar(typeof(Scrollbars));
        BindText(typeof(Texts));
        gameObject.BindEvent((evt) => 
        { 
            Camera.main.transform.position = new Vector3(_ptc.transform.position.x, Camera.main.transform.position.y, _ptc.transform.position.z); 
        }, Define.UIEvent.Click);
        return true;
    }

    private void Update()
    {
        if (_ptc == null)
            Managers.Resource.Destory(gameObject);

        Color a = Color.red;
        Color b = Color.green;
        Color color = Color.Lerp(a, b, _ptc.Hp / _ptc.MaxHp);
        GetScrollbar((int)Scrollbars.HPBar).targetGraphic.color = color;
        GetScrollbar((int)Scrollbars.HPBar).size = _ptc.Hp / _ptc.MaxHp;
    }

    public void SetInfo(ProtectedTowerController ptc, int idx = 1)
    {
        _ptc = ptc;

        if(ptc.Type == Define.TowerType.ProtectedTower)
            GetText((int)Texts.NameText).text = $"보호 시설 {idx}";
        else
            GetText((int)Texts.NameText).text = $"최종 보호 시설";
    }
}
