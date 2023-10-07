using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WaitingForBuildTower : UI_Base
{
    enum Scrollbars
    {
        Bar
    }

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindScrollbar(typeof(Scrollbars));
        return true;
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void ForWait(float t, Action evt)
    {
        StartCoroutine(CoForWait(t, evt));
    }

    IEnumerator CoForWait(float t, Action evt)
    {
        float currentTime = 0f;

        while(currentTime <= t)
        {
            currentTime += Time.deltaTime;
            GetScrollbar((int)Scrollbars.Bar).size = currentTime / t;
            yield return null;
        }

        evt.Invoke();
        Managers.Resource.Destory(gameObject);
    }
}
