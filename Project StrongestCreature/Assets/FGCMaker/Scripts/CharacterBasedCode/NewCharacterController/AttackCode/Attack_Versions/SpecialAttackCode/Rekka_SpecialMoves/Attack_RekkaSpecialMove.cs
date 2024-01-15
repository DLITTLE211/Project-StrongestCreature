using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Attack_RekkaSpecialMove : Attack_Special_Rekka  , IAttack_RekkaFuctionality
{
    [SerializeField] private int curInput;
    [SerializeField] private int curRekkaInput, rekkaInputCount;
    [SerializeField] private int movementPortionLength;
    [SerializeField] private char finalAttackButton;
    [SerializeField] private bool moveComplete;
    [SerializeField] internal int leewayTime;
    [SerializeField] internal bool inRekkaState;
    [SerializeField] internal List<Attack_BaseProperties> usedRekkas;
    (Attack_BaseInput.MoveInput, Attack_BaseInput.AttackInput) _newinput;

    #region Special_Rekka Functions
    public override void TurnInputsToString()
    {
        curInput = 0;
        curRekkaInput= 0;
        rekkaInput.mainAttackProperty.InputTimer.SetTimerType();
        try
        {
            rekkaInput.mainAttackInput.turnStringToArray();
            movementPortionLength = rekkaInput.mainAttackInput.attackStringArray.Length - 1;
            finalAttackButton = rekkaInput.mainAttackInput.attackStringArray[rekkaInput.mainAttackInput.attackStringArray.Length - 1];
            inRekkaState = false;
            rekkaInputCount = rekkaInput._rekkaPortion.Count;
            usedRekkas = new List<Attack_BaseProperties>();
        }
        catch (ArgumentNullException e)
        {
            DebugMessageHandler.instance.DisplayErrorMessage(3, $"{e.Message} has taken place. Skipping Step...");
        }
    }
    public void SetAttackAnims(Character_Animator animator) 
    {
        rekkaInput.mainAttackProperty.SetAttackAnims(animator);
        for (int i = 0; i < rekkaInput._rekkaPortion.Count; i++)
        {
            rekkaInput._rekkaPortion[i].individualRekkaAttack.ActivateAttackInfo();
        }
    }

    public override void ResetCombo()
    {
        inRekkaState = false; 
        curInput = 0;
        curRekkaInput = 0;
        moveComplete = false;
        usedRekkas = new List<Attack_BaseProperties>();
    }

    public void ResetRekkaInputs()
    {
        inRekkaState = false;
        curRekkaInput = 0;
        usedRekkas = new List<Attack_BaseProperties>();
    }

    public override bool ContinueCombo(Character_ButtonInput moveInput, Character_Base curBase, Character_ButtonInput attackInput)
    {
        if (attackInput == moveInput) 
        {
            return CheckCombo(moveInput, curBase);
        }
        else 
        {
            return CheckCombo(moveInput, curBase, attackInput);
        }
    }
    public override void ResetMoveCombo()
    {
        rekkaInput.mainAttackProperty.InputTimer.ResetTimerSuccess();
    }

    public void PreformAttack(Character_Base curBase, RekkaAttack _rekka = null)
    {
        if (_rekka != null)
        {
            curBase._aManager.ReceiveAttack(_rekka.individualRekkaAttack._correctInput[0].property);
        }
        else 
        {
            curBase._aManager.ReceiveAttack(rekkaInput.mainAttackProperty);
        }
    }

    public bool CheckCombo(Character_ButtonInput Input, Character_Base curBase, Character_ButtonInput attackInput = null)
    {
        rekkaInput.mainAttackProperty.InputTimer.CheckForInput = true;
        #region Non-Rekka Input Check
        if (inRekkaState == false)
        {
            if (!moveComplete)
            {
                if (IsCorrectInput(Input, curBase, curInput))
                {
                    curInput++;
                    rekkaInput.mainAttackProperty.InputTimer.ResetTimerSuccess();
                    if (curInput >= movementPortionLength && moveComplete == false)
                    {
                        moveComplete = true;
                        ResetMoveCombo();
                    }
                    if (curInput >= movementPortionLength + 1 && moveComplete == true)
                    {
                        PreformAttack(curBase);
                    }
                    return true;
                }
                return false;
            }
            else
            {
                if (IsCorrectInput(Input, curBase, curInput, attackInput))
                {
                    curInput++;
                    rekkaInput.mainAttackProperty.InputTimer.ResetTimerSuccess();
                    if (curInput >= movementPortionLength && moveComplete == false)
                    {
                        moveComplete = true;
                        ResetMoveCombo();
                    }
                    if (curInput >= movementPortionLength + 1 && moveComplete == true)
                    {
                        PreformAttack(curBase);
                    }
                    return true;
                }
                return false;
            }
        }
        #endregion

        #region In Rekka Input Check
        else
        {
            if (attackInput != null)
            {
                if (IsRekkaCorrectInput(Input, curBase, curInput, attackInput) != null)
                {
                    RekkaAttack confirmedRekkaAttack = IsRekkaCorrectInput(Input, curBase, curInput, attackInput);
                    if (curRekkaInput > rekkaInputCount)
                    {
                        Debug.Log("Hit rekka limit... not performing");
                        ResetRekkaInputs();
                        return false;
                    }
                    curRekkaInput++;
                    rekkaInput.mainAttackProperty.InputTimer.ResetTimeOnRekka((leewayTime * (1 / 60f)));
                    PreformAttack(curBase, confirmedRekkaAttack);
                     return true;
                }
                else { return false; }
            }
            return false;
        }
        #endregion
    }
    #endregion


    #region Interface Functions

    #region Input Verification Code
    int TransfigureDirectionOnSideSwitch(Character_ButtonInput move)
    {
        int switchValue = 5;
        switch (move.Button_State.directionalInput)
        {
            case 9:
                switchValue = 7;
                break;
            case 6:
                switchValue = 4;
                break;
            case 3:
                switchValue = 1;
                break;
            case 7:
                switchValue = 9;
                break;
            case 4:
                switchValue = 6;
                break;
            case 1:
                switchValue = 3;
                break;
            default:
                switchValue = move.Button_State.directionalInput;
                break;
        }
        return switchValue;
    }
    bool ButtonStateCheck(Character_ButtonInput attack,RekkaAttack curRekkaAttack)
    {
        bool checkButtonState = attack.Button_State._state != ButtonStateMachine.InputState.released;
        if (checkButtonState) 
        {
            return true;
        }
        return attack.Button_State._state == curRekkaAttack.individualRekkaAttack._correctInput[0].attackInputState._state;
    }
    bool itemCheck(RekkaAttack curRekkaAttack)
    {
        if (curRekkaInput >= rekkaInput._rekkaPortion.Count)
        {
            ResetCombo();
        }
        bool DirectionInputCheck = false;
        if (curRekkaAttack.individualRekkaAttack._correctInput[0].verifyAttackInput.Item1 == (Attack_BaseInput.MoveInput)0)
        {
            DirectionInputCheck = true;
        }
        else
        {
            DirectionInputCheck = _newinput.Item1 == curRekkaAttack.individualRekkaAttack._correctInput[0].verifyAttackInput.Item1;
        }
        char attackInput = curRekkaAttack.individualRekkaAttack._correctInput[0].verifyAttackInput.Item2.ToString().ToCharArray()[0];
        bool AttackInputCheck = (char)_newinput.Item2 == attackInput;
        return DirectionInputCheck && AttackInputCheck;
    }
    public bool IsCorrectInput(Character_ButtonInput testInput, Character_Base _curBase, int curInput, Character_ButtonInput attackInput = null)
    {
        if (moveComplete == false)
        {
            bool moveInput = false;
            if (_curBase.pSide.thisPosition._directionFacing == Character_Face_Direction.FacingRight)
            {
                moveInput = rekkaInput.mainAttackInput.attackStringArray[curInput].ToString() == testInput.Button_State.directionalInput.ToString();
            }
            else
            {
                moveInput = rekkaInput.mainAttackInput.attackStringArray[curInput].ToString() == TransfigureDirectionOnSideSwitch(testInput).ToString();
            }

            // DebugMessageHandler.instance.DisplayErrorMessage(3, $"Current Direction Inputted: {TransfigureDirectionOnSideSwitch(testInput)}");
            bool moveState = testInput.Button_State._state == ButtonStateMachine.InputState.directional;
            bool thisMove = moveInput && moveState;
            return thisMove;
        }
        else
        {
            bool buttonInput = finalAttackButton.ToString() == attackInput.Button_Name.ToString();
            bool CorrectState = attackInput.Button_State._state == rekkaInput.mainAttackInputState._state;
            bool thisAttack = buttonInput && CorrectState;
            return thisAttack;
        }
    }

    public RekkaAttack IsRekkaCorrectInput(Character_ButtonInput testInput, Character_Base _curBase, int curInput, Character_ButtonInput attackInput = null)
    {
        for (int i = 0; i < rekkaInput._rekkaPortion.Count; i++)
        {
            int _lastDirection = testInput.Button_State.directionalInput;
            _newinput.Item1 = (Attack_BaseInput.MoveInput)_lastDirection;
            char buttonInput = attackInput.Button_Name.ToCharArray()[0];
            _newinput.Item2 = (Attack_BaseInput.AttackInput)buttonInput;
            if (ButtonStateCheck(attackInput, rekkaInput._rekkaPortion[i]) && itemCheck(rekkaInput._rekkaPortion[i]))
            {
                return rekkaInput._rekkaPortion[i];
            }
            else
            {
                continue;
            }
        }
        return null;
    }
    #endregion

    public void SetRekkaStateTrue() 
    {
        inRekkaState = true; 
    }
    public void SendCounterHitInfo(Character_Base curBase, RekkaAttack _rekkaAttack = null)
    {
        if (_rekkaAttack != null)
        {
            curBase._cDamageCalculator.ReceiveCounterHitMultiplier(_rekkaAttack.individualRekkaAttack._correctInput[0].property.counterHitDamageMult);
        }
        else
        {
            curBase._cDamageCalculator.ReceiveCounterHitMultiplier(rekkaInput.mainAttackProperty.counterHitDamageMult);
        }
    }

    public void SendSuccessfulDamageInfo(Character_Base curBase, bool blockedAttack, RekkaAttack _rekkaAttack = null)
    {
        if (_rekkaAttack != null)
        {
            SendCounterHitInfo(curBase,_rekkaAttack);
            if (!blockedAttack)
            {
                curBase._cDamageCalculator.TakeDamage(_rekkaAttack.individualRekkaAttack._correctInput[0].property);
            }
            else 
            {
                curBase._cDamageCalculator.TakeChipDamage(_rekkaAttack.individualRekkaAttack._correctInput[0].property);
            }
        }
        else
        {
            SendCounterHitInfo(curBase); 
            if (!blockedAttack)
            {
                curBase._cDamageCalculator.TakeDamage(_rekkaAttack.individualRekkaAttack._correctInput[0].property);
            }
            else
            {
                curBase._cDamageCalculator.TakeChipDamage(_rekkaAttack.individualRekkaAttack._correctInput[0].property);
            }
        }
    }

    public void SetComboTimer(Character_InputTimer_Attacks timer)
    {
       rekkaInput.mainAttackProperty.InputTimer = timer;
        for (int i = 0; i < rekkaInput._rekkaPortion.Count; i++) 
        {
            rekkaInput._rekkaPortion[i].individualRekkaAttack._correctInput[0].property.InputTimer = timer;
        }
    }
    #endregion

}
