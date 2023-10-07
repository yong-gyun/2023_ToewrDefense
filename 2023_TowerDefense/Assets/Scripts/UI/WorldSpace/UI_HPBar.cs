using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HPBar : UI_Base
{
    enum Scrollbars
    {
        Bar
    }

    BaseController _bc;
    
    public void SetController(BaseController bc)
    {
        _bc = bc;
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindScrollbar(typeof(Scrollbars));
        GetScrollbar((int)Scrollbars.Bar).size = 1f;
        return true;
    }

    private void Update()
    {
        if(_bc.IsEnemy == false)
        {
            Color a = Color.red;
            Color b = Color.green;

            Color color = Color.Lerp(a, b, _bc.Hp / _bc.MaxHp);
            GetScrollbar((int)Scrollbars.Bar).targetGraphic.color = color; 
        }
        else
        {
            GetScrollbar((int)Scrollbars.Bar).targetGraphic.color = Color.red;
        }

        if(_bc.Hp == Mathf.Infinity)
            GetScrollbar((int)Scrollbars.Bar).targetGraphic.color = Color.green;


        SetRatio(_bc.Hp / _bc.MaxHp);
        transform.rotation = Camera.main.transform.rotation;
    }

    void SetRatio(float value)
    {
        GetScrollbar((int)Scrollbars.Bar).size = value;
    }
}