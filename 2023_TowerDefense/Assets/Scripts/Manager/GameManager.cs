using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    public int CurrentStage { get; set; } = 1;
    public int CurrentScore { get; set; } = 0;
    public int CurrentGold { get; set; } = 100;
    public int CurrentWave { get; set; }
    public int MaxWave { get; set; }
    public Dictionary<Define.ItemType, ItemController> UsedItems { get; private set; } = new Dictionary<Define.ItemType, ItemController>();
    public Define.ItemType[] Items { get; set; } = new Define.ItemType[3] { Define.ItemType.Unknow, Define.ItemType.Unknow, Define.ItemType.Unknow };
    public int[] CurrentItemPrices { get; set; } = new int[5] { 50, 100, 120, 150, 200 };
    public int CurrentItemBuyableCount { get; set; } = 5;
    public int MaxItemBuyableCount { get; set; } = 5;
    public int GoldAcquisitionAmount { get; set; } = 1;
    public int CurrentHaveItemAmount { get; set; } = 0;
    public float CurrentTime { get; set; } = 0;
    public bool IsStageClear { get; set; }
    public bool IsStageEnd { get; set; }

    public PlayerController Player
    { 
        get 
        {
            if(_player == null)
                _player = GameObject.FindObjectOfType<PlayerController>();
            return _player; 
        } 
    }
    
    PlayerController _player;

    public void Init()
    {
        for (int i = 1; i < (int) Define.ItemType.MaxCount; i++)
            UsedItems.Add((Define.ItemType)i, null);
    }

    public bool GetRandomItem()
    {
        if (CurrentHaveItemAmount == Items.Length || CurrentItemBuyableCount == 0)
        {
            Managers.UI.MakeEffectUI<UI_MiddleAlert>().SetAlert("아이템을 더 구매 할 수 없습니다", 1f);
            return false;
        }
        int idx = MaxItemBuyableCount - CurrentItemBuyableCount;

        if (CurrentGold < CurrentItemPrices[idx])
            return false;

        CurrentGold -= CurrentItemPrices[idx];
        Define.ItemType type = Define.ItemType.Unknow;

        while (true)
        {
            int percent = UnityEngine.Random.Range(1, 100);
            
            if (percent <= 15)
                type = Define.ItemType.TowerHeal;
            else if (percent <= 35)
                type = Define.ItemType.AllEnemysSlow;
            else if (percent <= 55)
                type = Define.ItemType.GoldAcquisition;
            else if (percent <= 70)
                type = Define.ItemType.TowerAttackSpeedInc;
            else if (percent <= 90)
                type = Define.ItemType.AllEnemysAttackStop;
            else
                type = Define.ItemType.CreatePatrolUnit;

            if (Items.Contains(type) == false)
            {
                Debug.Log(type);
                break;
            }
        }

        Items[CurrentHaveItemAmount++] = type;
        CurrentItemBuyableCount--;
        return true;
    }

    public void OnGameResult(bool result)
    {
        IsStageClear = result;
        IsStageEnd = true;
        UI_BigAlert alert = Managers.UI.MakeEffectUI<UI_BigAlert>();

        if(result)
        {
            Managers.Sound.Play("Interaction/Victory");
            Time.timeScale = 0f;
            alert.SetInfo("Complete", 2f, () =>
            {
                Managers.UI.ShowPopupUI<UI_StageResultPopup>();
            });
        }   
        else
        {
            Managers.Sound.Play("Interaction/Defeat");
            Time.timeScale = 0f;
            alert.SetInfo("Failed", 2f, () =>
            {
                Managers.UI.ShowPopupUI<UI_StageResultPopup>();
            });
        }
            
    }

    public void Clear()
    {
        Array.Fill(Items, Define.ItemType.Unknow);
        IsStageClear = false;
        IsStageEnd = false;
        CurrentItemBuyableCount = MaxItemBuyableCount;
        GoldAcquisitionAmount = 1;
        CurrentHaveItemAmount = 0;
    }
}