using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const float TILE_SIZE = 2F;

    public enum Sound
    {
        Bgm = 0,
        Effect,
        MaxCount
    }

    public enum ItemType
    {
        Unknow = 0,
        TowerHeal,
        AllEnemysSlow,
        GoldAcquisition,
        TowerAttackSpeedInc,
        AllEnemysAttackStop,
        CreatePatrolUnit,
        MaxCount
    }


    public enum UIEvent
    {
        Click,
        PointerEnter,
        PointerExit,
    }

    public enum WorldObject
    {
        Unknow,
        Unit,
        Tower
    }

    public enum TowerType
    {
        Default = 0,
        Multiply,
        Focus,
        Illusion,
        Obstacle,
        ProtectedTower,
        LastProtectedTower
    }

    public enum Priority
    {
        Illusion = 3,
        Tower = 2,
        ProtectedTower = 1
    }

    public enum UnitType
    {
        MeleeUnit = 0,
        RangedAttackUnit,
        QuickMoveUnit,
        FlyableUnit,
        MiddleBossUnit,
        GoldUnit,
        StageBossUnit,
        PatrolUnit
    }

    public enum State
    {
        Idle,
        Attack_To_Idle,
        Move,
        Attack,
        MiddleBoss_Rush,
        MiddleBoss_RushEnd,
        RangeAttack,
        Die
    }

    public enum SceneType
    {
        Menu,
        Stage1,
        Stage2,
        InputRank
    }

    public enum TileType
    {
        Unknow,
        Block,
        Buildable,
        Movable,
        Bridge,
    }
}
