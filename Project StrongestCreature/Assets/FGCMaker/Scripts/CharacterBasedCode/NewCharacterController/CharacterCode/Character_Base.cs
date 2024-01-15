using System;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Character_Base : MonoBehaviour
{
    #region Script References
    [Header("Character Script References")]
    public Character_AttackDetection _cADetection;
    public Character_ComboDetection _cComboDetection;
    public Character_Animator _cAnimator;
    public Character_InputDetection _cInput;
    public Character_InputTimer_Attacks _cAttackTimer;
    public Character_InputTimer_Mobility _cMobiltyTimer;
    public ControllerWidgetHandler widget;
    public Character_Force _cForce;
    public Character_StateMachine _cStateMachine;
    public Character_Health _cHealth;
    public Character_DamageCalculator _cDamageCalculator;
    public Character_ComboCounter _cComboCounter;
    public Character_GravityScaler _cGravity;
    public Character_HitStun _cHitstun;
    public Character_HurtboxController _cHurtBox;
    public Character_HitController _cHitController;
    public Character_SuperMeter _cSuperMeter;
    public Attack_Manager _aManager;
    [Space(10)]
    public Character_Timer _timer;
    [Space(20)]
    #endregion

    #region Rewired Controls
    [Header("Rewired Controls")]
    public int playerID;
    IList<InputAction> _actions;
    Dictionary<int, int> newActions;
    ActionElementMap[] newElements;
    ControllerMap _map;
    public Player player;
    [Space(20)]
    #endregion

    #region General Input Variables
    [Header("General Input Variables")]
    public List<Character_ButtonInput> moveAxes;
    public List<Character_ButtonInput> attackButtons;
    [Space(20)]

    #endregion

    #region Directional Input Detection
    [Header("Directional Input Detection")]
    public Character_MobilityAsset _extraMoveAsset;
    public List<Character_Mobility> _extraMoveControls;
    [SerializeField] public List<Character_Mobility> _removeList;
    [SerializeField] public float xVal, yVal;
    [SerializeField, Range(0f, 1f)] public float xYield, yYield;
    [SerializeField] public int numpadValue;
    [SerializeField] public Vector2 numpadVector;
    [Space(20)]
    #endregion

    #region Character Combos
    [Header("Button Input Detection")]
    //public NewComboList comboList3_0;
    public GameObject comboInstantiatedSpot;
    [SerializeField] private Character_MoveList sourceComboList3_0;
    public Character_MoveList comboList3_0;

    public List<Attack_NonSpecialAttack> simpleAttackList;
    public List<Attack_NonSpecialAttack> removeSimpleList;

    public List<Attack_BasicSpecialMove> specialMoveAttackTest;
    public List<Attack_BasicSpecialMove> removeSMList;

    public List<Attack_RekkaSpecialMove> rekkaAttackList;
    public List<Attack_RekkaSpecialMove> rekkaRemoveList;

    public List<Attack_StanceSpecialMove> stanceAttackList;
    public List<Attack_StanceSpecialMove> stanceRemoveList;
    [Space(20)]
    #endregion

    #region Character Force Variables
    [Header("Force Controller Variables")]
    public Rigidbody myRb;
    [SerializeField] private float _jumpForce;
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

    [SerializeField] private float _moveForce;
    public float MoveForce { get { return _moveForce; } set { _moveForce = value; } }

    [SerializeField] private float _jumpDirForce;
    public float JumpDirForce { get { return _jumpDirForce; } set { _jumpDirForce = value; } }

    public int movementPC; //movement priority check
    [Space(20)]
    #endregion  
    
    #region Player SubState
    [Header("Side Recognition")]
    public Character_SubStates _subState;
    [Space(20)]
    #endregion

    #region Side Recognition
    [Header("Side Recognition")]
    public Player_SideRecognition pSide; 
    [Space(20)]
    #endregion


    #region Opponent Reference
    [Header("Side Recognition")]
    public Character_Base opponentPlayer;
    [Space(20)]
    #endregion

    [SerializeField]
    public int newField;

    #region Initialization Code
    public void Initialize(Character_SubStates setSubState, int NewID = -1)
    {
        InitButtons(setSubState, NewID);
        ResetInputLog();
        ResetRemoveList();
        InitCombos();
        _cComboCounter.SetStartComboCounter();
    }
    void ResetRemoveList() 
    {
        removeSimpleList.Clear();
        removeSMList.Clear();
    }
    void ResetInputLog()
    {
        _timer.inputLogger.ResetAllText();
    }
    void InitCombos()
    {
        _extraMoveControls = _extraMoveAsset.MobilityOptions;
        GetCharacterMoveList();
        _cComboDetection.PrimeCombos();
    }
    void GetCharacterMoveList() 
    {
        Character_MoveList newComboList = Instantiate(sourceComboList3_0, comboInstantiatedSpot.transform);
        comboList3_0 = newComboList;
    }
    void InitButtons(Character_SubStates setSubState, int NewID)
    {
        switch (setSubState) 
        {
            case Character_SubStates.Controlled:
                playerID = NewID;
                HandleButtonInitialization();
                _subState = setSubState;
                break;
            case Character_SubStates.Dummy:
                _subState = setSubState;
                DesyncVariables();
                playerID = -1;
                break;
        }
    }
    void DesyncVariables() 
    {
        if (player != null)
        {
            // player.controllers.RemoveController(ControllerType.Joystick, playerID);
            player.controllers.maps.AddMap(ControllerType.Joystick, playerID, _map);
            //player.controllers.maps.RemoveMap(ControllerType.Joystick, playerID, $"TestPlayer", $"TestPlayer{playerID}");
            player = null;
            newElements = new ActionElementMap[0];
            moveAxes.Clear();
            attackButtons.Clear();
        }
    }
    void HandleButtonInitialization() 
    {
        player = ReInput.players.GetPlayer(playerID);
        player.controllers.AddController(ControllerType.Joystick, playerID,true);
        player.controllers.maps.LoadMap(ControllerType.Joystick, playerID, $"TestPlayer", $"TestPlayer{playerID}");

        _actions = ReInput.mapping.Actions;
        newActions = new Dictionary<int, int>();
        attackButtons = new List<Character_ButtonInput>();
        moveAxes = new List<Character_ButtonInput>();
        try
        {
            //newElements = map.GetElementMaps();
            _map = player.controllers.maps.GetMap(playerID);
            newElements = player.controllers.maps.GetMap(ControllerType.Joystick, playerID, $"TestPlayer", $"TestPlayer{playerID}").GetElementMaps();

            Debug.Log("Controller Map Count: " + player.controllers.maps.GetMaps(ControllerType.Joystick, playerID).Count);
        }
        catch (NullReferenceException) 
        {
            List<ControllerMap> results = new List<ControllerMap>();
            Debug.Log(player.controllers.maps.GetAllMaps(ControllerType.Joystick,results));
            newElements = results[0].GetElementMaps();
        }
        for (int i = 0; i < _actions.Count; i++) 
        {
            newActions.Add(_actions[i].id, i);
        }
        for (int i = 0; i < newElements.Length; i++)
        {
            Character_ButtonInput newButton = new Character_ButtonInput(newElements[i]);
            switch ((newButton.Button_Element.actionDescriptiveName).ToUpper())
            {
                case "VERTICAL":
                    newButton.Button_State.OnDirectional();
                    break;
                case "HORIZONTAL":
                    newButton.Button_State.OnDirectional();
                    break;
                case "FORWARD":
                    newButton.Button_State.OnDirectional();
                    break;
                case "BACKWARD":
                    newButton.Button_State.OnDirectional();
                    break;
                case "UPWARD":
                    newButton.Button_State.OnDirectional();
                    break;
                case "DOWNWARD":
                    newButton.Button_State.OnDirectional();
                    break;
                default:
                    newButton.Button_State.OnReleased();
                    break;
            }
            newButton.SetButton(newElements[i]);
            //buttons.Add(newButton);
            //buttons[i].SetButton(newElements[i]);
            int index = 0;
            if (newActions.TryGetValue(newButton.Button_Element.actionId, out index)) 
            {
                newButton.TryAddButton(_actions[index].name);
                if (newButton.Button_State._state == ButtonStateMachine.InputState.released)
                {
                    attackButtons.Add(newButton);
                }
                else
                {
                    moveAxes.Add(newButton);
                }
            }

        }
    }
    #endregion

    private void Update()
    {
        _cADetection.CheckButtonPressed();
        _cADetection.CallReturnButton();
        _timer.TimerCountDown();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        _cInput.ReceiveInput();
    }
    public int ReturnInputValue(int value) 
    {
        if (_subState != Character_SubStates.Controlled) 
        { 
            return -1; 
        }

        moveAxes[0].Button_State.directionalInput = value;
        _cComboDetection.CheckPossibleCombos(moveAxes[0]);
        return value; 
    }
    public Character_ButtonInput ReturnMovementInputs()
    {
        if (_subState != Character_SubStates.Controlled) 
        { 
            return null; 
        }
        return moveAxes[0];
    }
    public Character_ButtonInput ReturnBlockButton()
    {
        if (_subState != Character_SubStates.Controlled)
        { 
            return null; 
        }
        return attackButtons[9];
    }
    public Character_ButtonInput ReturnTechButton()
    {
        if (_subState != Character_SubStates.Controlled)
        {
            return null;
        }
        return attackButtons[0];
    }
}
[System.Serializable]
public class ButtonInput
{
    public InputAction inputAction;
    public ButtonStateMachine buttonState;
    public void SetButton(InputAction action)
    {
        inputAction = action;
        buttonState.OnReleased();
    }
}

[System.Serializable]
public class ButtonStateMachine
{
    public enum InputState { pressed, held, released, directional };
    public InputState _state;
    public int directionalInput;
    public void SetDI(int numInput)
    {
        directionalInput = numInput;
    }
    public InputState returnButtonState()
    {
        return this._state;
    }
    public void OnPressed()
    {
        _state = InputState.pressed;
    }
    public void OnHeld()
    {
        _state = InputState.held;
    }
    public void OnDirectional()
    {
        _state = InputState.directional;
    }
    public void OnReleased()
    {
        _state = InputState.released;
    }
}