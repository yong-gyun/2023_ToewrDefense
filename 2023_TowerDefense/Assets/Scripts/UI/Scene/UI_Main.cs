using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Main : UI_Scene
{
    enum Images
    {
        ChangeValueImage
    }

    enum Texts
    {
        GoldText,
        ScoreText,
        WaveText,
        TimeText, 
        CurrentStageText,
        ItemBuyText
    }

    enum Buttons
    {
        GameSpeedChangeButton,
        ItemBuyButton,
        BuyDefaultButton,
        BuyMultiplyButton,
        BuyFocusButton,
        BuyObstacleButton,
        BuyIllusionButton
    }

    enum GameObjects
    {
        ProtectedTowerList,
        ItemSlotContent,
        UsedItemTab,
        ItemUsedAlertContetnt
    }

    UI_ItemSlot_Subitem[] _itemSlots = new UI_ItemSlot_Subitem[3];
    List<UI_ProtectedTowerHp_Subitem> _protectedTowerHps = new List<UI_ProtectedTowerHp_Subitem>();
    bool _isItemUsed = false;

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindImage(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        for (int i = 0; i < 3; i++)
        {
            UI_ItemSlot_Subitem subitem = Managers.UI.MakeSubitemUI<UI_ItemSlot_Subitem>(Get<GameObject>((int)GameObjects.ItemSlotContent).transform);
            subitem.gameObject.BindEvent((evtData) => { OnClickItemButton(subitem); }, Define.UIEvent.Click);
            subitem.SetInfo(i);
            _itemSlots[i] = subitem;
        }

        GetButton((int)Buttons.ItemBuyButton).gameObject.BindEvent(OnClickItemBuyButton, Define.UIEvent.Click);
        GetButton((int)Buttons.GameSpeedChangeButton).onClick.AddListener(OnClickGameSpeedChangeButton);
        GetButton((int)Buttons.BuyDefaultButton).onClick.AddListener(OnClickBuyDefaultButton);
        GetButton((int)Buttons.BuyMultiplyButton).onClick.AddListener(OnClickMultiplyButton);
        GetButton((int)Buttons.BuyFocusButton).onClick.AddListener(OnClickBuyFocusButton);
        GetButton((int)Buttons.BuyObstacleButton).onClick.AddListener(OnClickBuyObstacleButton);
        GetButton((int)Buttons.BuyIllusionButton).onClick.AddListener(OnClickBuyIllusionButton);

        GetButton((int)Buttons.BuyDefaultButton).gameObject.GetComponent<UI_TowerBuyButton_Subitem>().SetInfo(Define.TowerType.Default);
        GetButton((int)Buttons.BuyMultiplyButton).gameObject.GetComponent<UI_TowerBuyButton_Subitem>().SetInfo(Define.TowerType.Multiply);
        GetButton((int)Buttons.BuyFocusButton).gameObject.GetComponent<UI_TowerBuyButton_Subitem>().SetInfo(Define.TowerType.Focus);
        GetButton((int)Buttons.BuyObstacleButton).gameObject.GetComponent<UI_TowerBuyButton_Subitem>().SetInfo(Define.TowerType.Obstacle);
        GetButton((int)Buttons.BuyIllusionButton).gameObject.GetComponent<UI_TowerBuyButton_Subitem>().SetInfo(Define.TowerType.Illusion);
        GetText((int)Texts.CurrentStageText).text = $"스테이지{Managers.Game.CurrentStage}";
        GetText((int)Texts.ItemBuyText).text =
@$"아이템 구매
(구매 가능 횟수:{Managers.Game.CurrentItemBuyableCount})";
        return true;
    }

    private void Start()
    {
        for (int i = 0; i < Managers.Object.ProtectedTowers.Count; i++)
        {
            UI_ProtectedTowerHp_Subitem subitem = Managers.UI.MakeSubitemUI<UI_ProtectedTowerHp_Subitem>(Get<GameObject>((int)GameObjects.ProtectedTowerList).transform);
            subitem.SetInfo(Managers.Object.ProtectedTowers[i], i + 1);
            _protectedTowerHps.Add(subitem);
        }

        if(Managers.Object.LastProtectedTower != null)
        {
            UI_ProtectedTowerHp_Subitem subitem = Managers.UI.MakeSubitemUI<UI_ProtectedTowerHp_Subitem>(Get<GameObject>((int)GameObjects.ProtectedTowerList).transform);
            subitem.SetInfo(Managers.Object.LastProtectedTower);
            _protectedTowerHps.Add(subitem);
        }
    }

    private void Update()
    {
        GetText((int)Texts.WaveText).text = $"웨이브 {Managers.Game.CurrentWave}/{Managers.Game.MaxWave}";
        GetText((int)Texts.ScoreText).text = $"점수:{Managers.Game.CurrentScore}";
        GetText((int)Texts.GoldText).text = $"{Managers.Game.CurrentGold}";
        
        int min = (int)Managers.Game.CurrentTime / 60;
        int sec = (int)Managers.Game.CurrentTime % 60;
        GetText((int)Texts.TimeText).text = $"{string.Format("{0:00}:{1:00}", min, sec)}(x{Time.timeScale})";
    }


    void OnClickGameSpeedChangeButton()
    {
        Managers.Sound.Play("Interaction/ButtonClick");
        if (Time.timeScale == 1f)
        {
            GetImage((int)Images.ChangeValueImage).sprite = Managers.Resource.Load<Sprite>("Sprites/2");
            Time.timeScale = 2f;
        }
        else
        {
            GetImage((int)Images.ChangeValueImage).sprite = Managers.Resource.Load<Sprite>("Sprites/1");
            Time.timeScale = 1f;
        }
    }

    void OnClickItemBuyButton(PointerEventData evt)
    {
        int idx = Managers.Game.MaxItemBuyableCount - Managers.Game.CurrentItemBuyableCount;
        int price = Managers.Game.CurrentGold - Managers.Game.CurrentItemPrices[idx];

        Managers.Sound.Play("Interaction/ButtonClick");
        if (price < 0)
        {
            Managers.UI.MakeEffectUI<UI_MiddleAlert>().SetAlert($"{Mathf.Abs(price)}골드가 부족합니다", 1f);
            return;
        }

        if (Managers.Game.GetRandomItem() == false)
            return;
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            if (_itemSlots[i].Type == Define.ItemType.Unknow)
            {
                _itemSlots[i].SetInfo(Managers.Game.CurrentHaveItemAmount - 1);
                break;
            }
        }

        GetText((int)Texts.ItemBuyText).text =
@$"아이템 구매 {Managers.Game.CurrentItemPrices[Managers.Game.MaxItemBuyableCount - Managers.Game.CurrentItemBuyableCount]}G
(구매 가능 횟수:{Managers.Game.CurrentItemBuyableCount})";
    }

    void OnClickItemButton(UI_ItemSlot_Subitem subitem)
    {
        if (_isItemUsed)
            return;

        if (subitem.Type == Define.ItemType.Unknow)
            return;
        Managers.Sound.Play("Interaction/ButtonClick");
        Define.ItemType type = subitem.Type;

        if (Managers.Game.UsedItems[subitem.Type] == null || subitem.Type == Define.ItemType.CreatePatrolUnit)
        {
            UI_ItemUsed_Subitem itemUsedSubitem = Managers.UI.MakeSubitemUI<UI_ItemUsed_Subitem>(Get<GameObject>((int)GameObjects.UsedItemTab).transform);
            itemUsedSubitem.OnUsed(type);
        }
        else
        {
            ItemController item = Managers.Game.UsedItems[type];
            item.OnUsed(type);
        }

        
        string description = "";

        switch (type)
        {
            case Define.ItemType.TowerHeal:
                description = "타워 체력 30% 회복";
                break;
            case Define.ItemType.AllEnemysSlow:
                description = "10초간 모든 적의 이동 속도 50% 감소";
                break;
            case Define.ItemType.GoldAcquisition:
                description = "1분간 골드 획득량이 두배 증가";
                break;
            case Define.ItemType.TowerAttackSpeedInc:
                description = "10초간 타워의 공격 딜레이 50% 감소";
                break;
            case Define.ItemType.AllEnemysAttackStop:
                description = "10초간 모든 적의 공격 중지";
                break;
            case Define.ItemType.CreatePatrolUnit:
                description = "순찰 유닛 생성 (1분 후 삭제)";
                break;
        }

        AddUsedAlert(description);
        subitem.Clear();
        Managers.Game.CurrentHaveItemAmount--;

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].OnCooltime(5f);
        }

        StartCoroutine(CoWait(5f, () => { _isItemUsed = false; }));

    }

    IEnumerator CoWait(float time, Action action)
    {
        yield return time;
        action.Invoke();
    }

    public void AddUsedAlert(string description)
    {
        UI_UsedAlertContent alert = Managers.UI.MakeSubitemUI<UI_UsedAlertContent>(Get<GameObject>((int)GameObjects.ItemUsedAlertContetnt).transform);
        alert.SetInfo(description);
    }

    void OnClickBuyDefaultButton()
    {
        CreatePreviewer(Define.TowerType.Default);
    }
    void OnClickMultiplyButton()
    {
        CreatePreviewer(Define.TowerType.Multiply);
    }
    void OnClickBuyFocusButton()
    {
        CreatePreviewer(Define.TowerType.Focus);
    }
    void OnClickBuyObstacleButton()
    {
        CreatePreviewer(Define.TowerType.Obstacle);
    }
    void OnClickBuyIllusionButton()
    {
        CreatePreviewer(Define.TowerType.Illusion);
    }
    void CreatePreviewer(Define.TowerType type)
    {
        int price = Managers.Game.CurrentGold - Managers.Data.TowerStatData[type].price;
        Managers.Sound.Play("Interaction/ButtonClick");

        if (price < 0)
        {
            Managers.UI.MakeEffectUI<UI_MiddleAlert>().SetAlert($"{Mathf.Abs(price)}골드가 부족합니다", 1f);
            return;
        }

        GameObject go = GameObject.Find("BuildPreviewer");

        if (go != null)
            go.GetComponent<PreviewBuildSpaceController>().OnExit();

        go = new GameObject("BuildPreviewer");
        PreviewBuildSpaceController pbc = go.GetOrAddComponent<PreviewBuildSpaceController>();
        pbc.OnBuild(type);
    }
}
