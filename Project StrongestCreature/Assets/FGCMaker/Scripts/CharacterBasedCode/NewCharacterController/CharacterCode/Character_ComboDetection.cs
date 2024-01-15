using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_ComboDetection : MonoBehaviour
{
    [SerializeField] private Character_Base _base;
    [SerializeField] private Character_Animator _animator;
    [SerializeField] private string lastInput;
    public bool inStance, inRekka, superMobilityOption;

    private void Start()
    {
        lastInput = "";
    }
    public void PrimeCombos()
    {
        PrimeNormal();
        ResetComboList();
        PrimeMobility();
        PrimeSpecialMoves();



       
    }
    void PrimeSpecialMoves()
    {
        for (int i = 0; i < _base.stanceAttackList.Count; i++)
        {
            _base.stanceAttackList[i].SetComboTimer(_base._cAttackTimer);
            _base.stanceAttackList[i].TurnInputsToString();
            _base.stanceAttackList[i].stanceStartProperty.SetAttackAnims(_base._cAnimator);
            _base.stanceAttackList[i].stanceInput.stanceAttack._stanceButtonInput._correctInput[0].SetInnerAttackAnimations(_base._cAnimator);
            _base.stanceAttackList[i].stanceInput.stanceKill._stanceButtonInput._correctInput[0].SetInnerAttackAnimations(_base._cAnimator);

            _base.stanceAttackList[i].SetAttackAnims(_base._cAnimator);

        }
        for (int i = 0; i < _base.rekkaAttackList.Count; i++)
        {
            _base.rekkaAttackList[i].SetComboTimer(_base._cAttackTimer);
            _base.rekkaAttackList[i].TurnInputsToString();
            _base.rekkaAttackList[i].SetAttackAnims(_base._cAnimator);
            for (int j = 0; j < _base.rekkaAttackList[i].rekkaInput._rekkaPortion.Count; j++) 
            {
                _base.rekkaAttackList[i].rekkaInput._rekkaPortion[j].individualRekkaAttack ._correctInput[0].SetInnerAttackAnimations(_base._cAnimator);
            }
        }
        for (int i = 0; i < _base.specialMoveAttackTest.Count; i++)
        {
            _base.specialMoveAttackTest[i].SetComboTimer(_base._cAttackTimer);
            _base.specialMoveAttackTest[i].TurnInputsToString();
            _base.specialMoveAttackTest[i].property.SetAttackAnims(_base._cAnimator);
        }
    }
    void PrimeNormal() 
    {
        for (int i = 0; i < _base.comboList3_0.simpleAttacks.Count; i++)
        {
            _base.simpleAttackList.Add(_base.comboList3_0.simpleAttacks[i]);
            _base.simpleAttackList[i].SetComboTimer(_base._cAttackTimer);
            _base.simpleAttackList[i].SetStarterInformation();
        }
        for (int i = 0; i < _base.simpleAttackList.Count; i++)
        {
            for (int j = 0; j < _base.simpleAttackList[i]._attackInput._correctInput.Count; j++)
            {
                _base.simpleAttackList[i]._attackInput._correctInput[j].SetInnerAttackAnimations(_base._cAnimator);
            }
        }
    }
    void PrimeMobility() 
    {
        for (int i = 0; i < _base._extraMoveAsset.MobilityOptions.Count; i++)
        {
            _base._extraMoveControls[i].cTimer = _base._cMobiltyTimer;
            _base._extraMoveControls[i].baseCharacter = _base;
            MobilityAnimation mobilityAnim = _base._extraMoveControls[i].mobilityAnim;
            _base._extraMoveControls[i].mobilityAnim = new MobilityAnimation(_animator.myAnim, mobilityAnim.animClip, mobilityAnim.animName, mobilityAnim.totalWaitTime, mobilityAnim.animLength, mobilityAnim.frameData);

            _base._extraMoveControls[i].TurnInputsToString(_base);
            _base._extraMoveControls[i].ResetCurrentInput();
            _base._extraMoveControls[i].SetAnims(_base._cAnimator);
        }
    }
    void ResetComboList() 
    {
        SetSimpleButtons();
        SetSpecialButtons();
    }
    void ResetMovementList()
    {
        _base._extraMoveControls.Clear();
        PrimeMobility();
    }
    void SetSimpleButtons()
    {
        _base.simpleAttackList.Clear();
        for (int i = 0; i < _base.comboList3_0.simpleAttacks.Count; i++)
        {
            _base.simpleAttackList.Add(_base.comboList3_0.simpleAttacks[i]);
        }
    }
    void SetSpecialButtons()
    {
        _base.specialMoveAttackTest.Clear();
        _base.rekkaAttackList.Clear();
        _base.stanceAttackList.Clear();

        for (int i = 0; i < _base.comboList3_0.special_Simple.Count; i++)
        {
            _base.specialMoveAttackTest.Add(_base.comboList3_0.special_Simple[i]);
        }
        for (int i = 0; i < _base.comboList3_0.rekkaSpecials.Count; i++)
        {
            _base.rekkaAttackList.Add(_base.comboList3_0.rekkaSpecials[i]);
        }
        for (int i = 0; i < _base.comboList3_0.stanceSpecials.Count; i++)
        {
            _base.stanceAttackList.Add(_base.comboList3_0.stanceSpecials[i]);
        }
    }
    public void CheckPossibleCombos(Character_ButtonInput newInput)
    {
        StoreNewInput(newInput);
    }
    public void StoreNewInput(Character_ButtonInput input)
    {
        if (input.Button_State._state == ButtonStateMachine.InputState.directional)
        {
            if (lastInput != input.Button_State.directionalInput.ToString())
            {
                lastInput = input.Button_State.directionalInput.ToString();
                SpecialInputVerifier(input);
                ExtraMovementVerifier(input);
            }
        }
        else
        {
            if (_base._cAnimator.inputWindowOpen)
            {
                SimpleInputVerifier(input);
                SpecialInputVerifier(input);
            }
        }
    }
    void SimpleInputVerifier(Character_ButtonInput input)
    {
        for (int i = 0; i < _base.simpleAttackList.Count; i++)
        {
            Attack_NonSpecialAttack c = _base.simpleAttackList[i];
            if (c.ContinueCombo(_base.moveAxes[0],input, _base))
            {
                //Current input is correct");
            }
            else
            {
                _base.removeSimpleList.Add(_base.simpleAttackList[i]);
            }
            if (_base.removeSimpleList.Count >= _base.simpleAttackList.Count)
            {
                // c.ResetCombo();
                _base.removeSimpleList.Clear();
            }
        }
        foreach (Attack_NonSpecialAttack possibleAttack in _base.removeSimpleList)
        {
            _base.simpleAttackList.Remove(possibleAttack);
        }
        if (_base.simpleAttackList.Count <= _base.comboList3_0.simpleAttacks.Count)
        {
            _base.simpleAttackList.Clear();
            ResetComboList();
            //Messenger.Broadcast(Events.ComboReset);
        }
    }
    void SpecialInputVerifier(Character_ButtonInput input)
    {
        #region Basic Special Moves
        for (int i = 0; i < _base.specialMoveAttackTest.Count; i++)
        {
            Attack_BasicSpecialMove c = _base.specialMoveAttackTest[i];
            if (c.ContinueCombo(input,_base))
            {
                //Current input is correct");
            }
            else
            {
                _base.removeSMList.Add(_base.specialMoveAttackTest[i]);
            }
            if (_base.removeSMList.Count >= _base.specialMoveAttackTest.Count)
            {
               // c.ResetCombo();
                _base.removeSMList.Clear();
            }
        }
        foreach (Attack_BasicSpecialMove possibleAttack in _base.removeSMList)
        {
            _base.specialMoveAttackTest.Remove(possibleAttack);
        }
        if (_base.specialMoveAttackTest.Count <= _base.comboList3_0.simpleAttacks.Count)
        {
            ResetComboList();
            _base.removeSMList.Clear();
        }
        #endregion

        #region Rekka Special Moves
        for (int i = 0; i < _base.rekkaAttackList.Count; i++)
        {
            Attack_RekkaSpecialMove c = _base.rekkaAttackList[i];
            if (c.ContinueCombo(_base.moveAxes[0], _base, input))
            {
                //Current input is correct");
            }
            else
            {
                _base.rekkaRemoveList.Add(_base.rekkaAttackList[i]);
            }
            if (_base.rekkaRemoveList.Count >= _base.rekkaAttackList.Count)
            {
                // c.ResetCombo();
                _base.rekkaRemoveList.Clear();
            }
        }
        foreach (Attack_RekkaSpecialMove possibleAttack in _base.rekkaRemoveList)
        {
            _base.rekkaAttackList.Remove(possibleAttack);
        }
        if (_base.rekkaAttackList.Count <= _base.comboList3_0.rekkaSpecials.Count)
        {
            ResetComboList();
            _base.rekkaRemoveList.Clear();
        }
        #endregion

        #region Stance Special Moves
        for (int i = 0; i < _base.stanceAttackList.Count; i++)
        {
            Attack_StanceSpecialMove c = _base.stanceAttackList[i];
            if (c.ContinueCombo(_base.moveAxes[0], _base,input))
            {
                //Current input is correct");
            }
            else
            {
                _base.stanceRemoveList.Add(_base.stanceAttackList[i]);
            }
            if (_base.stanceRemoveList.Count >= _base.stanceAttackList.Count)
            {
                // c.ResetCombo();
                _base.rekkaRemoveList.Clear();
            }
        }
        foreach (Attack_StanceSpecialMove possibleAttack in _base.stanceRemoveList)
        {
            _base.stanceAttackList.Remove(possibleAttack);
        }
        if (_base.stanceAttackList.Count <= _base.comboList3_0.stanceSpecials.Count)
        {
            ResetComboList();
            _base.stanceRemoveList.Clear();
        }
        #endregion
    }

    void ExtraMovementVerifier(Character_ButtonInput input)
    {
        for (int i = 0; i < _base._extraMoveControls.Count; i++)
        {
            Character_Mobility c = _base._extraMoveControls[i];
            if (c.ContinueCombo(input,_base, superMobilityOption))
                {/*Current Input Is Correct Per (i) MovementOption*/}

            else{ _base._removeList.Add(_base._extraMoveControls[i]); }
        }
        foreach (Character_Mobility i in _base._removeList)
        {
           // _base._extraMoveControls.Remove(i);
        }
        if (_base._removeList.Count >= _base._extraMoveControls.Count)
        {
           // _base._removeList.Clear();
        }
        if (_base._extraMoveControls.Count < _base._extraMoveAsset.MobilityOptions.Count)
        {
           // ResetMovementList();
            //Messenger.Broadcast(Events.ClearLastInput);
        }
    }
    public void ResetCombos()
    {
        for (int i = 0; i < _base.simpleAttackList.Count; i++)
        {
            _base.comboList3_0.simpleAttacks[i].ResetCombo();
            _base.simpleAttackList[i].ResetCombo();
        }
        for (int i = 0; i < _base.specialMoveAttackTest.Count; i++)
        {
            _base.comboList3_0.special_Simple[i].ResetCombo();
            _base.specialMoveAttackTest[i].ResetCombo();
        }
        for (int i = 0; i < _base.rekkaAttackList.Count; i++)
        {
            _base.comboList3_0.rekkaSpecials[i].ResetCombo();
            _base.rekkaAttackList[i].ResetCombo();
        }
        for (int i = 0; i < _base.stanceAttackList.Count; i++)
        {
            _base.comboList3_0.stanceSpecials[i].ResetCombo();
            _base.stanceAttackList[i].ResetCombo();
        }
    }
}
