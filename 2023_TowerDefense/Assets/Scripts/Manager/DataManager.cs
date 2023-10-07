using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class DataManager
{
    public Dictionary<Define.TowerType, TowerStat> TowerStatData { get; private set; } = new Dictionary<Define.TowerType, TowerStat>();
    public Dictionary<Define.UnitType, UnitStat> UnitStatData { get; private set; } = new Dictionary<Define.UnitType, UnitStat>();
    public List<RankData> Ranks 
    { 
        get 
        {
            if (_ranks.Count == 0)
                return _ranks;

            var ranks = from n in _ranks orderby n.score descending select n; 
            _ranks = ranks.ToList(); 
            return _ranks; 
        }
    }

    List<RankData> _ranks = new List<RankData>();

    public void Init()
    {
        TowerStatData = LoadJson<TowerStatData, Define.TowerType, TowerStat>("TowerStatData").MakeDict();
        UnitStatData = LoadJson<UnitStatData, Define.UnitType, UnitStat>("UnitStatData").MakeDict();
        LoadRankData();
    }

    public void LoadRankData()
    {
        if(PlayerPrefs.HasKey("1st_score"))
        {
            for (int i = 0; i < 10; i++)
            {
                string name = PlayerPrefs.GetString($"{i + 1}st_name");
                int score = PlayerPrefs.GetInt($"{i + 1}st_score");
                Ranks.Add(new RankData(name, score));
            }
        }
    }

    public void SaveData()
    {
        string fileText = "";

        for (int i = 0; i < Ranks.Count; i++)
        {
            if (i == 10)
                break;

            PlayerPrefs.SetString($"{i + 1}st_name", _ranks[i].name);
            PlayerPrefs.SetInt($"{i + 1}st_score", _ranks[i].score);

            if (i + 1<= 3)
            {
                fileText += string.Format("{0},{1},{2}", i + 1, _ranks[i].name, _ranks[i].score);
                fileText += Environment.NewLine;
            }
        }

        File.WriteAllText("rank.txt", fileText);
    }

    Loader LoadJson<Loader, TKey, TValue>(string path) where Loader : ILoader<TKey, TValue>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
