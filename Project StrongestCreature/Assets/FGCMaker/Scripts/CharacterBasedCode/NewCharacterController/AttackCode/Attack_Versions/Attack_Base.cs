using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[Serializable]
public class Attack_BaseProperties
{
    public string _attackName;
    [SerializeField] protected Character_InputTimer_Attacks _cTimer;
    public Character_InputTimer_Attacks InputTimer 
    { 
        get { return _cTimer; } 
        set { _cTimer = value; } 
    }
    #region Attack Damage Numbers
    [Space(20)]
    [Header("Attack Damage Numbers")]
    public float rawAttackDamage;
    public float counterHitDamageMult;
    [Range(1, 100)] public int hitstopValue;
    [Range(1, 200)] public int hitstunValue;
    [Range(15, 75)] public int _attackScaling;
    public HitLevel hitLevel;
    [SerializeField] public List<int> attackHashes;
    #endregion

    [Range(0, 3)] public int _meterRequirement;
    [Range(0, 30)] public int _meterAwardedOnHit;
    public bool dashCancelable, JumpCancelable;

    #region MoveType Properties
    [Space(20)]
    [Header("Move Variables and Cancel Information")]
    public AirAttackInfo _airInfo;
    public Attack_CancelInfo cancelProperty;
    public MoveType _moveType;
    public AttackHandler_Attack AttackAnims;
    #endregion

    #region KnockBack/KnockDown Variables
    [Space(20)]
    [Header("Knockdown Variables")]
    public Horizontal_KnockBack lateralKBP; // Lateral KnockBack Properties
    public Vertical_KnockBack verticalKBP; // Vertical KnockBack Properties
    public Attack_KnockDown KnockDown; // Vertical KnockBack Properties
    #endregion
    public void SetAttackAnims(Character_Animator animator)
    {
        attackHashes = new List<int>();
        AttackAnims.SetAttackAnim(animator);
        attackHashes.Add(Animator.StringToHash(AttackAnims.animName));

    }
}
[Serializable]
public class Attack_Input 
{
    public string attackString;
    public char[] attackStringArray;
    public void turnStringToArray() 
    {
        attackStringArray = attackString.ToCharArray();
    }
}
[Serializable]
public class Vertical_KnockBack
{
    public Attack_KnockBack_Vertical verticalKBP;
    [Range(0f, 50f)] public int Value;
}
[Serializable]
public class Horizontal_KnockBack
{
    public Attack_KnockBack_Lateral lateralKBP;
    [Range(0f, 50f)] public int Value;
}
[Serializable]
public enum Attack_KnockBack_Lateral
{
    No_KB,
    Crumple,
    Slight_KB,
    Medium_KB,
    Heavy_KB,
    FullForceWallBounce,
}
[Serializable]
public enum Attack_KnockBack_Vertical
{
    No_KUD,
    Slight_KU,
    Medium_KU,
    Heavy_KU,
    Slight_KD,
    Medium_KD,
    Heavy_KD,
    FullForceGroundBounce,
}
[Serializable]
public enum HitLevel
{
    SlightKnockback,
    MediumKnockback,
    SoaringHit,
    Crumple,
    Spiral,
}
[Serializable]
public enum Attack_KnockDown
{
    /*
     * SKD = Soft KnockDown
     * HKD = Hard KnockDown
     */
    NONE,
    SKD,
    HKD
}
[Serializable]
public enum AirAttackInfo
{
    /*
     * SKD = Soft KnockDown
     * HKD = Hard KnockDown
     */
    GroundOnly,
    AirOk,
    AirOnly
}