using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

[System.Serializable]
public class HurtBox : CollisionDetection
{
    public HurtBoxType huBType;
    public ColliderType colliderType;
    private Attack_BaseProperties currentHitProperties;
    private void Start()
    {
        SetHurtBoxSize(this.transform.localScale.x, this.transform.localScale.y,colliderType);
    }
    void FixedUpdate()
    {
        CheckDefenseState();
    }
    public void CheckDefenseState()
    {
        switch (huBType)
        {
            case HurtBoxType.NoBlock:
                SetRendererColor(new Color32(255, 0, 170, 95)); //Pink
                break;
            case HurtBoxType.BlockLow:
                SetRendererColor(new Color32(102, 222, 255, 95)); //Teal
                break;
            case HurtBoxType.BlockHigh:
                SetRendererColor(new Color32(197, 255, 102, 95)); //Lime Green
                break;
            case HurtBoxType.ParryLow:
                SetRendererColor(new Color32(236, 220, 188, 95)); //Tan
                break;
            case HurtBoxType.ParryHigh:
                SetRendererColor(new Color32(155, 97, 52, 95)); //Brown
                break;
            case HurtBoxType.SoftKnockdown:
                SetRendererColor(new Color32(57, 207, 255, 95)); //Skyblue
                break;
            case HurtBoxType.HardKnockdown:
                SetRendererColor(new Color32(135, 135, 135, 95)); //grey
                break;
            case HurtBoxType.Invincible:
                SetRendererColor(new Color32(255, 255, 255, 95)); //White
                break;
            case HurtBoxType.Armor:
                SetRendererColor(new Color32(188, 106, 106, 95)); //Off Red
                break;
            default:
                DebugMessageHandler.instance.DisplayErrorMessage(1, $"Invalid HurtboxType Detected.");
                break;
        }
        SetText($"Current HurtboxType: {huBType}");
    }
    #region Multi-Hit Functions
    #region Hit Portion Functions
    IEnumerator DoMultiHit(HitBox _hitbox, HitCount hitCount, Character_Base Base_Target, Character_Base Base_Attacker)
    {
        //Base_Target.comboList3_0.Check(attackName, Base_Target);
        Base_Target._cAnimator.isHit = true;
        HandleMultiHitProperties(_hitbox, hitCount, Base_Target, Base_Attacker);
        Base_Target._cGravity.UpdateGravityScaleOnHit(_hitbox.hitboxProperties.hitstunValue);// Base_Attacker._cAnimator.lastAttack.hitstunValue);
        for (int i = 0; i < hitCount._count; i++)
        {
            yield return new WaitForSeconds(hitCount._refreshRate);
            Base_Target.comboList3_0.CheckAndApply(_hitbox.hitboxProperties, Base_Target, Base_Attacker,false);
        }
        _hitbox.DestroyHitbox(_hitbox, Base_Attacker.pSide.thisPosition.GiveHurtBox());
        hitCount.ResetRefresh();
        hitCount.ResetHitCount();
        Base_Attacker._cAnimator.ClearLastAttack();
    }
    async void HandleMultiHitProperties(HitBox _hitbox,HitCount hitCount, Character_Base Base_Target, Character_Base Base_Attacker)
    {
        Base_Target._cForce.SendKnockBackOnHit(_hitbox.hitboxProperties); 
        Base_Target._cHitController.HandleHitState(_hitbox.hitboxProperties);
        await Base_Target._cHitstun.ApplyHitStun(_hitbox.hitboxProperties.hitstunValue);
        await Character_Hitstop.Instance.CallHitStop(_hitbox.hitboxProperties, _hitbox.hitboxProperties.hitstopValue, Base_Target);
    }
    #endregion

    #region Block Portion Functions
    IEnumerator DoMultiHit_OnBlock(HitBox _hitbox, HitCount hitCount, Character_Base Base_Target, Character_Base Base_Attacker)
    {
        //Base_Target.comboList3_0.Check(attackName, Base_Target);
        Base_Target._cAnimator.isHit = true;
        HandleMultiHitProperties_OnBlock(_hitbox, hitCount, Base_Target, Base_Attacker);
        for (int i = 0; i < hitCount._count; i++)
        {
            yield return new WaitForSeconds(hitCount._refreshRate);
            Base_Target.comboList3_0.CheckAndApply(_hitbox.hitboxProperties, Base_Target, Base_Attacker, true);
        }
        _hitbox.DestroyHitbox(_hitbox, Base_Attacker.pSide.thisPosition.GiveHurtBox());
        hitCount.ResetRefresh();
        hitCount.ResetHitCount();
        Base_Attacker._cAnimator.ClearLastAttack();
    }
    async void HandleMultiHitProperties_OnBlock(HitBox _hitbox, HitCount hitCount, Character_Base Base_Target, Character_Base Base_Attacker)
    {
        Base_Target._cForce.SendKnockBackOnHit(_hitbox.hitboxProperties);
        Base_Target._cHitController.HandleBlockState(_hitbox.hitboxProperties);
        await Base_Target._cHitstun.ApplyHitStun(_hitbox.hitboxProperties.hitstunValue/5f);
        await Character_Hitstop.Instance.CallHitStop(_hitbox.hitboxProperties, _hitbox.hitboxProperties.hitstopValue/5f, Base_Target);
    }
    #endregion
    #endregion
    public void ReceieveHitBox(HitBox _hitbox, HitCount hitCount, Transform target)
    {
        switch (huBType)
        {
            case HurtBoxType.NoBlock:
                //Send Do Damage;
                OnSuccessfulHit(_hitbox, hitCount, target);
                break;
            case HurtBoxType.BlockLow:
                if (_hitbox.HBType == HitBoxType.Low)
                {
                    OnSuccessfulBlock(_hitbox, hitCount, target);
                }
                else 
                {
                    if (_hitbox.HBType == HitBoxType.Throw)
                    {
                        //Send Throw;
                    }
                    else if (_hitbox.HBType == HitBoxType.CommandGrab)
                    {
                        //Send Command Throw;
                    }
                    else 
                    {
                        //Send Do Damage;
                    }
                }
                break;
            case HurtBoxType.BlockHigh:
                if (_hitbox.HBType == HitBoxType.High ^ _hitbox.HBType == HitBoxType.Overhead)
                {
                    OnSuccessfulBlock(_hitbox, hitCount, target);
                }
                else
                {
                    if (_hitbox.HBType == HitBoxType.Throw)
                    {
                        //Send Throw;
                    }
                    else if (_hitbox.HBType == HitBoxType.CommandGrab)
                    {
                        //Send Command Throw;
                    }
                    else
                    {
                        //Send Do Damage;
                    }
                }
                break;
            case HurtBoxType.ParryLow:
                if (_hitbox.HBType == HitBoxType.Low ^ _hitbox.HBType == HitBoxType.Unblockable)
                {
                    //Send Parry Low;
                }
                else
                {
                    if (_hitbox.HBType == HitBoxType.Throw)
                    {
                        //Send Throw With Counter Hit;
                    }
                    else if (_hitbox.HBType == HitBoxType.CommandGrab)
                    {
                        //Send Command Throw With Counter Hit;
                    }
                    else if (_hitbox.HBType == HitBoxType.High)
                    {
                        //Send High With Counter Hit;
                    }
                }
                break;
            case HurtBoxType.ParryHigh:
                if (_hitbox.HBType == HitBoxType.High ^ _hitbox.HBType == HitBoxType.Overhead ^ _hitbox.HBType == HitBoxType.Unblockable)
                {
                    //Send Parry High;
                }
                else
                {
                    if (_hitbox.HBType == HitBoxType.Throw)
                    {
                        //Send Throw With Counter Hit;
                    }
                    else if (_hitbox.HBType == HitBoxType.CommandGrab)
                    {
                        //Send Command Throw With Counter Hit;
                    }
                    else if (_hitbox.HBType == HitBoxType.Low)
                    {
                        //Send Low With Counter Hit;
                    }
                }
                break;
            case HurtBoxType.SoftKnockdown:
                //Send Hit on SoftKnockdown
                break;
            case HurtBoxType.HardKnockdown:
                if (_hitbox.HBType == HitBoxType.Low)
                {
                    //Send Hit with Low;
                }
                break;
            case HurtBoxType.Invincible:
                //Send Hit InvulBody
                break;
            case HurtBoxType.Armor:
                if (_hitbox.HBType == HitBoxType.Throw)
                {
                    //Send Throw;
                }
                else if (_hitbox.HBType == HitBoxType.CommandGrab)
                {
                    //Send Command Throw;
                }
                else if (_hitbox.HBType == HitBoxType.Unblockable)
                {
                    //Send Hit with Unblockable;
                }
                else 
                {
                    //Send reduce unit of Armor by 1
                }
                break;
            default:
                DebugMessageHandler.instance.DisplayErrorMessage(1, $"Invalid HurtboxType Detected.");
                break;
        }
    }
    public async void OnSuccessfulBlock(HitBox _hitbox, HitCount hitCount, Transform target)
    {
        Character_Base Base_Target = target.GetComponent<Character_Base>();
        Character_Base Base_Attacker = _hitbox.GetComponentInParent<Character_Base>();
        currentHitProperties = _hitbox.hitboxProperties;
        if (hitCount._count > 1)
        {
            if (_hitbox.HBType != HitBoxType.nullified)
            {
                StartCoroutine(DoMultiHit_OnBlock(_hitbox, hitCount, Base_Target, Base_Attacker));
            }
        }
        else if (hitCount._count <= 1)
        {
            if (_hitbox.HBType != HitBoxType.nullified)
            {
                Attack_BaseProperties currentAttack = Base_Attacker.pSide.thisPosition.ReturnPhysicalSideHitBox().hitboxProperties;

                Base_Target.comboList3_0.CheckAndApply(currentAttack, Base_Target, Base_Attacker,true);
                await Character_Hitstop.Instance.CallHitStop(currentAttack, currentAttack.hitstopValue/5f, Base_Target);
                Base_Target._cHitController.HandleBlockState(currentAttack);
                await Base_Target._cHitstun.ApplyHitStun(currentAttack.hitstunValue/5f);
                _hitbox.DestroyHitbox(_hitbox, Base_Attacker.pSide.thisPosition.GiveHurtBox());
            }
        }
    }
    public async void OnSuccessfulHit(HitBox _hitbox, HitCount hitCount, Transform target) 
    {
        Character_Base Base_Target = target.GetComponent<Character_Base>();
        Character_Base Base_Attacker = _hitbox.GetComponentInParent<Character_Base>();
        currentHitProperties = _hitbox.hitboxProperties;
        if (hitCount._count > 1)
        {
            if (_hitbox.HBType != HitBoxType.nullified)
            {
                StartCoroutine(DoMultiHit(_hitbox, hitCount, Base_Target, Base_Attacker));
            }
        }
        else if (hitCount._count <= 1)
        {
            if (_hitbox.HBType != HitBoxType.nullified)
            {
                Attack_BaseProperties currentAttack = Base_Attacker.pSide.thisPosition.ReturnPhysicalSideHitBox().hitboxProperties;

                Base_Target.comboList3_0.CheckAndApply(currentAttack, Base_Target, Base_Attacker,false);
                await Character_Hitstop.Instance.CallHitStop(currentAttack, currentAttack.hitstopValue, Base_Target);
                Base_Target._cHitController.HandleHitState(currentAttack);
                Base_Target._cGravity.UpdateGravityScaleOnHit(currentAttack.hitstunValue);
                await Base_Target._cHitstun.ApplyHitStun(currentAttack.hitstunValue);
                _hitbox.DestroyHitbox(_hitbox, Base_Attacker.pSide.thisPosition.GiveHurtBox());
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            if (currentHitProperties != null)
            {
                Character_Force thisPlayerForce = this.gameObject.transform.root.GetComponent<Character_Base>()._cForce;
                Character_HurtboxController thisPlayerGroundedState = this.gameObject.transform.root.GetComponent<Character_Base>()._cHurtBox;
                if (currentHitProperties.lateralKBP.lateralKBP == Attack_KnockBack_Lateral.FullForceWallBounce)
                {
                    StartCoroutine(thisPlayerForce.DoWallLaunch());
                    Debug.Log("Will Wall Bounce on Hit");
                }
                currentHitProperties = null;
            }

            else
            {
                Character_Base thisBase = this.gameObject.transform.root.GetComponent<Character_Base>();
                Player_SideRecognition thisPSide = this.gameObject.transform.root.GetComponent<Character_Base>().pSide;
                thisBase._cForce.AddForceOnCommand(0.75f);
                Debug.Log("Hit Wall Frame 1");
            }
        }
        else if (other.gameObject.tag == "Ground") 
        {
            if (currentHitProperties != null)
            {
                Character_Force thisPlayerForce = this.gameObject.transform.root.GetComponent<Character_Base>()._cForce;
                Character_HurtboxController thisPlayerGroundedState = this.gameObject.transform.root.GetComponent<Character_Base>()._cHurtBox;
                if (currentHitProperties.verticalKBP.verticalKBP == Attack_KnockBack_Vertical.FullForceGroundBounce)
                {
                    if (thisPlayerGroundedState.IsGrounded() == false)
                    {
                        thisPlayerForce.DoGroundBounce(currentHitProperties);
                        Debug.Log("Will Ground Bounce");
                    }
                    currentHitProperties = null;
                }
            }
        }
    }
}
