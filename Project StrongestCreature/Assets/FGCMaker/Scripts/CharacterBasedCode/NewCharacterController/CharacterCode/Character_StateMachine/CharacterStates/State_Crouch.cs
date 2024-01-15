using System.Threading.Tasks;
using UnityEngine;

public class State_Crouch : BaseState
{
    public State_Crouch(Character_Base playerBase) : base(playerBase)
    {}
    public override async void OnEnter()
    {
        DebugMessageHandler.instance.DisplayErrorMessage(1, "Enter CrouchState");

        _base._cHurtBox.SetHitboxSize(HurtBoxSize.Crouching);
        _base._cHurtBox.SetHurboxState(HurtBoxType.NoBlock);
        if (_base.ReturnMovementInputs().Button_State.directionalInput <= 3)
        {
            _cAnim.PlayNextAnimation(crouchHash, _crossFade);
        }
        await WaitToChargeSuperMobility();
    }
    async Task WaitToChargeSuperMobility() 
    {
        float OneFrame = 1 / 60f;
        float waitTime = 10 * OneFrame;
        int timeInMS = (int)(waitTime * 1000f);
        await Task.Delay(timeInMS);
        if (_base.ReturnMovementInputs().Button_State.directionalInput <= 3)
        {
            _base._cComboDetection.superMobilityOption = true;
        }
    }
    public override void OnUpdate()
    {
        if (_base._cAnimator._lastMovementState != Character_Animator.lastMovementState.nullified)
        {
           // _base._cAnimator.NullifyMobilityOption();
        }
        base.OnUpdate();
    }
    public override void OnRecov()
    {
        base.OnRecov();
    }

    public override void OnExit()
    {
        _base._cHurtBox.SetHitboxSize(HurtBoxSize.Standing);
        base.OnExit();
    }
}
