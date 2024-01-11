using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Character/Character Profile")]
public class Character_Profile : ScriptableObject
{
    #region Character Identification Info
    [Header("Character Indentity Information")]
    [SerializeField] private string _CharacterName;
    [SerializeField] private int _CharacterID;
    [SerializeField] private Sprite _CharacterProfileImage;
    #endregion

    #region Character Sizing Info
    [Header("Character Sizing Information")]
    [SerializeField] private int _Mass = 10;
    [SerializeField] private float _Height = 60f;
    [SerializeField] private float _Weight = 100f;
    #endregion

    #region Character Movement Info
    [Header("Character Movement Information")]
    [SerializeField] private float _MoveVelocity = 5f;
    [SerializeField] private float _JumpForce = 100f;
    [SerializeField] private float _InAirMoveForce = 25f;
    #endregion

    #region Character Health Info
    [Header("Character Health Information")]
    [SerializeField, Range(100f, 250f)] private float _MaxHealth = 100f;
    [SerializeField, Range(35f, 75f)] private float _MaxStunValue = 100f;
    [SerializeField, Range(10f, 150f)] private float _DefenseValue = 100f;
    [SerializeField,Range(1f,8f)] private float _HealthRegenRate = 5f;
    #endregion

    #region Character Animator Info
    [Header("Character Animator Information")]
    [SerializeField] private Animator _Animator;
    [SerializeField] private List<AnimationClip> _AllCharacterAnimations = new List<AnimationClip>();
    [SerializeField] private List<AnimationLayerInfo> _LayerInfo = new List<AnimationLayerInfo>();
    [SerializeField] private GameObject _CharacterModel;
    #endregion

    #region Character MoveList Info
    [Header("Character MoveList Information")]
    [SerializeField] private GameObject _CharacterMoveListPrefab;
    //[SerializeField] private MobilityOptions _CharacterMobility;
    #endregion


    #region Character Interaction Info
    [Header("Character Interaction Information")]
    [SerializeField] private List<CharacterIntro> _BasicCharacterInteractions = new List<CharacterIntro>();
    [SerializeField] private List<CharacterIntro> _SpecialCharacterInteractions = new List<CharacterIntro>();
    #endregion
}

[Serializable]
public class AnimationLayerInfo 
{
    public AnimatorLayerType Type;
    public int LayerIndex;
}
[Serializable]
public enum AnimatorLayerType 
{
    Neutral_IdleAnimLayer,
    MovementAnimLayer,
    AttackAnimLayer,
    HitResponseAnimLayer,
}

[Serializable]
public class CharacterIntro 
{
    public string targetCharacter;
    public AnimationClip introAnim;
    public DialogueOption dialogueOption;
}
[Serializable]
public class DialogueOption
{
    public string statementSaid;
    public List<DialgueAudio> statementAudio = new List<DialgueAudio>();
}
[Serializable]
public class DialgueAudio
{
    public enum languageType {English, Japanese};
    public languageType Type;
    public AudioClip statementAudio;
    public float statementAudioLength;
}
