using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    interface ILoader<TKey, TValue>
    {
        Dictionary<TKey, TValue> MakeDict();
    }

    [Serializable]
    public class TowerStat
    {
        public Define.TowerType type;
        public float maxHp;
        public float hp;
        public float attack;
        public float attackDelay;
        public float attackRange;
        public int size;
        public int price;
    }

    [Serializable]
    public class TowerStatData : ILoader<Define.TowerType, TowerStat>
    {
        public List<TowerStat> stats = new List<TowerStat>();

        public Dictionary<Define.TowerType, TowerStat> MakeDict()
        {
            Dictionary<Define.TowerType, TowerStat> dict = new Dictionary<Define.TowerType, TowerStat>();

            foreach (TowerStat stat in stats)
            {
                dict.Add(stat.type, stat);
            }

            return dict;
        }
    }

    [Serializable]
    public class UnitStat
    {
        public Define.UnitType type;
        public float maxHp;
        public float hp;
        public float attack;
        public float attackDelay;
        public float attackRange;
        public float moveSpeed;
        public int rewardGold;
        public int rewardScore;
        public bool fireBullet;
    }

    [Serializable]
    public class UnitStatData : ILoader<Define.UnitType, UnitStat>
    {
        public List<UnitStat> stats = new List<UnitStat>();

        public Dictionary<Define.UnitType, UnitStat> MakeDict()
        {
            Dictionary<Define.UnitType, UnitStat> dict = new Dictionary<Define.UnitType, UnitStat>();

            foreach (UnitStat stat in stats)
            {
                dict.Add(stat.type, stat);
            }
            
            return dict;
        }      
    }

    public struct RankData
    {
        public string name;
        public int score;

        public RankData(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }
}