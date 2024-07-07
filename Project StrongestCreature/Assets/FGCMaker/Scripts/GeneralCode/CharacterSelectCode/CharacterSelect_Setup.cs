using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public class CharacterSelect_Setup : MonoBehaviour
{
    [SerializeField] private GameObject characterSelectButtonPrefab;
    [SerializeField] private GameObject characterSelectHolder;
    [SerializeField] private GameObject characterSelect_Header;
    [SerializeField] private List<GameObject> characterSelect_Assets;


    [SerializeField] private List<Character_Profile> _activeProfiles;
    [SerializeField] private List<Amplifiers> _activeAmplifiers;
    [SerializeField] private Image characterSelectBackgroundImage;
    [SerializeField] private List<GameObject> activeCharacterSelectButtons;
    [SerializeField] private CharacterSelectPage _leftPlayerPage,_rightPlayerPage;


    public Character_AvailableID players;
    [SerializeField] private PlayerCursor _leftPlayer,_rightPlayer;
    public Transform upBound,downBound,leftBound,rightBound;
    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener<Character_Profile, int>(Events.DisplayCharacterInfo, DisplayCharacterSelectInformation);
        _leftPlayerPage.characterAmplify.GetListOfAmplifiers(_activeAmplifiers);
        _rightPlayerPage.characterAmplify.GetListOfAmplifiers(_activeAmplifiers);
        _leftPlayerPage.SetPlayerInfo();
        _rightPlayerPage.SetPlayerInfo();
        if (activeCharacterSelectButtons.Count > 0)
        {
            ReactivateCharacterSelectInfo();
        }
        else
        {
            AddCharacterSelectButtons();
        }
    }
    public void AddCharacterSelectButtons()
    {
        for (int i = 0; i < _activeProfiles.Count; i++)
        {
            GameObject selectButton = Instantiate(characterSelectButtonPrefab, characterSelectHolder.transform);
            selectButton.gameObject.transform.localPosition = new Vector3(1, 1, 1);
            selectButton.gameObject.transform.localRotation = Quaternion.identity;
            selectButton.gameObject.transform.localScale = Vector3.one;
            selectButton.GetComponentInChildren<Button>().image.sprite = _activeProfiles[i].CharacterProfileImage;
            selectButton.GetComponentInChildren<Button>().interactable = true;
            GameObject _selectButtonInfo = selectButton;
            _selectButtonInfo.GetComponentInChildren<CharacterSelect_Button>().characterProfile = _activeProfiles[i];
            activeCharacterSelectButtons.Add(_selectButtonInfo);
        }
        StartCoroutine(CascadeScaleSelectButtons());
    }
    IEnumerator CascadeScaleSelectButtons()
    {
        for (int i = 0; i < activeCharacterSelectButtons.Count; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Vector3 selectButtonFirstSize = new Vector3(1.15f, 1.15f, 1.15f);
            activeCharacterSelectButtons[i].transform.DOScale(selectButtonFirstSize, 0.15f);
            yield return new WaitForSeconds(0.025f);
            activeCharacterSelectButtons[i].transform.DOScale(Vector3.one, 0.15f);
        }
        SetPlayerControllers();
    }

    void SetPlayerControllers() 
    {
        if (ReInput.controllers.GetJoystickNames().Length <= 0)
        {
            return;
        }
        else
        {
            players.InitAvailableIDs();
            players.AddToJoystickNames(ReInput.controllers.GetJoystickNames());
            if (ReInput.controllers.GetJoystickNames().Length == 1)
            {
                players.AddUsedID(players.joystickNames[0]);
                SetCharacterSelectCursorState(_leftPlayer, 0);
            }
            else
            {
                for (int i = 0; i < ReInput.controllers.GetJoystickNames().Length; i++)
                {
                    if (players.totalPlayers[i].playerID == -1)
                    {
                        players.AddUsedID(players.joystickNames[i]);
                    }
                    if (i == 0)
                    {
                        SetCharacterSelectCursorState(_leftPlayer, i);
                    }
                    if (i == 1)
                    {
                        SetCharacterSelectCursorState(_rightPlayer, i);
                    }
                    else { continue; }
                }
            }
        }
    }
    void SetCharacterSelectCursorState(PlayerCursor player, int ID) 
    {
        player.curPlayer = ReInput.players.GetPlayer(players.UsedID.Item1[ID]);
        player.curPlayer.controllers.AddController(ControllerType.Joystick, players.UsedID.Item1[ID], true);
        player.curPlayer.controllers.maps.LoadMap(ControllerType.Joystick, players.UsedID.Item1[ID], $"UI_CanvasController", $"TestPlayer{players.UsedID.Item1[ID]}");
        player.cursorObject.SetActive(true);
        player.cursorText.text = $"{players.UsedID.Item1[ID] + 1}";
        player.isConnected = true;
    }
    public void DisplayCharacterSelectInformation(Character_Profile hoveredProfile, int curHighlightedPlayerID)
    {
        if (curHighlightedPlayerID == 0)
        {
            _leftPlayerPage.UpdateInfo(hoveredProfile);
        }
        if (curHighlightedPlayerID == 1)
        {
            _rightPlayerPage.UpdateInfo(hoveredProfile);
        }
    }
    public async Task ClearCharacterSelectInfo() 
    {
        for (int i = 0; i < activeCharacterSelectButtons.Count; i++)
        {
            activeCharacterSelectButtons[i].GetComponentInChildren<Button>().interactable = false;
            activeCharacterSelectButtons[i].GetComponentInChildren<Button>().image.DOFade(0f, 1.5f);
            activeCharacterSelectButtons[i].SetActive(false);
        }
        characterSelectBackgroundImage.gameObject.SetActive(false);
        characterSelect_Header.SetActive(false);
        characterSelectHolder.SetActive(false);
        await Task.Delay(400);
    }
    public void ReactivateCharacterSelectInfo()
    {
        characterSelectBackgroundImage.gameObject.SetActive(true);
        characterSelect_Header.SetActive(true);
        characterSelectHolder.SetActive(true);
        for (int i = 0; i < activeCharacterSelectButtons.Count; i++)
        {
            activeCharacterSelectButtons[i].GetComponentInChildren<Button>().interactable = true;
            activeCharacterSelectButtons[i].GetComponentInChildren<Button>().image.DOFade(1f, 0f);
            activeCharacterSelectButtons[i].SetActive(true);
        }

        for (int i = 0; i < characterSelect_Assets.Count; i++)
        {
            characterSelect_Assets[i].SetActive(true);
        }
    }
    public async Task ClearLeftPlayerInfo()
    {
        _leftPlayerPage.ClearPlayerInfo();
        await Task.Delay(400);
    }
    public async Task ClearRightPlayerInfo()
    {
        _rightPlayerPage.ClearPlayerInfo();
        for (int i = 0; i < characterSelect_Assets.Count; i++)
        {
            characterSelect_Assets[i].SetActive(false);
        }
        await Task.Delay(400);
    }
    private void Update()
    {
        if (_leftPlayer.isConnected) 
        {
            _leftPlayer.xVal = _leftPlayer.curPlayer.GetAxisRaw("Horizontal");
            _leftPlayer.yVal = _leftPlayer.curPlayer.GetAxisRaw("Vertical");
            _leftPlayer.xVal = (_leftPlayer.xVal >= _leftPlayer.xYield) ? 1 : ((_leftPlayer.xVal <= -_leftPlayer.xYield) ? -1 : 0);
            _leftPlayer.yVal = (_leftPlayer.yVal >= _leftPlayer.yYield) ? 1 : ((_leftPlayer.yVal <= -_leftPlayer.yYield) ? -1 : 0);
            if (_leftPlayer.xVal == 0 && _leftPlayer.yVal == 0) 
            {
                _leftPlayer.cursorObject.GetComponent<Rigidbody2D>().drag = 10000f;
            }
            else
            {
                float xVal = _leftPlayer.xVal * 2;
                float yVal = _leftPlayer.yVal * 2;
                _leftPlayer.cursorObject.GetComponent<Rigidbody2D>().drag = 0;
                if (HitHeightBound(_leftPlayer.cursorObject.transform)) 
                {
                    xVal = 0;
                }
                if (HitWidthBound(_leftPlayer.cursorObject.transform))
                {
                    yVal = 0;
                }
                _leftPlayer.cursorObject.transform.Translate(new Vector3(xVal, yVal, 0));
            }
        }
        if (_rightPlayer.isConnected) 
        {
            _rightPlayer.xVal = _rightPlayer.curPlayer.GetAxisRaw("Horizontal");
            _rightPlayer.yVal = _rightPlayer.curPlayer.GetAxisRaw("Vertical");
            _rightPlayer.xVal = (_rightPlayer.xVal >= _rightPlayer.xYield) ? 1 : ((_rightPlayer.xVal <= -_rightPlayer.xYield) ? -1 : 0);
            _rightPlayer.yVal = (_rightPlayer.yVal >= _rightPlayer.yYield) ? 1 : ((_rightPlayer.yVal <= -_rightPlayer.yYield) ? -1 : 0);
            if (_rightPlayer.xVal == 0 && _rightPlayer.yVal == 0)
            {
                _rightPlayer.cursorObject.GetComponent<Rigidbody2D>().drag = 10000f;
            }
            else
            {
                float xVal = _rightPlayer.xVal * 2;
                float yVal = _rightPlayer.yVal * 2;
                _rightPlayer.cursorObject.GetComponent<Rigidbody2D>().drag = 0;
                if (HitHeightBound(_rightPlayer.cursorObject.transform))
                {
                    xVal = 0;
                }
                if (HitWidthBound(_rightPlayer.cursorObject.transform))
                {
                    yVal = 0;
                }
                _rightPlayer.cursorObject.transform.Translate(new Vector3(xVal, yVal, 0));
            }
        }
    }

    bool HitHeightBound(Transform cursorTransform) 
    {
        Bounds heightBounds = new Bounds(cursorTransform.localPosition, Vector3.zero);
        heightBounds.SetMinMax(downBound.localPosition, upBound.localPosition);
        float yPos = cursorTransform.localPosition.y;
        if (yPos > heightBounds.max.y - 1f)
        {
            cursorTransform.GetComponent<Rigidbody2D>().drag = 10000f;
            cursorTransform.localPosition = new Vector3(cursorTransform.localPosition.x, heightBounds.max.y - 10f, 0);
            return true;
        }
        if (yPos < heightBounds.min.y + 1f)
        {
            cursorTransform.GetComponent<Rigidbody2D>().drag = 10000f;
            cursorTransform.localPosition = new Vector3(cursorTransform.localPosition.x, heightBounds.min.y + 10f, 0); ;
            return true;
        }
        return false;
    }
    bool HitWidthBound(Transform cursorTransform)
    {
        Bounds widthBounds = new Bounds(cursorTransform.localPosition, Vector3.zero);
        widthBounds.SetMinMax(leftBound.localPosition, rightBound.localPosition);
        float xPos = cursorTransform.localPosition.x;
        if (xPos > widthBounds.max.x - 1)
        {
            cursorTransform.GetComponent<Rigidbody2D>().drag = 10000f;

            cursorTransform.localPosition = new Vector3(widthBounds.max.x - 10f, cursorTransform.localPosition.y, 0);
            return true;
        }
        if (xPos < widthBounds.min.x + 1)
        {
            cursorTransform.GetComponent<Rigidbody2D>().drag = 10000f;
            cursorTransform.localPosition = new Vector3(widthBounds.min.x + 10f, cursorTransform.localPosition.y, 0);
            return true;
        }
        return false;
    }
}

[Serializable]
public class CharacterSelectPage 
{
    public Image characterBackgroundImage;
    public TMP_Text characterName;
    public CharacterSelect_AmplifySelecter characterAmplify;
    public void UpdateInfo(Character_Profile profile) 
    {
        characterBackgroundImage.preserveAspect = true;
        characterBackgroundImage.sprite = profile.CharacterProfileImage;
        characterName.text = profile.CharacterName;
    }

    public void SetPlayerInfo()
    {
        characterBackgroundImage.DOFade(1f, 0f);
        characterName.DOFade(1f, 0f);
        characterAmplify.SetAmplifyInfo();
    }

    public void ClearPlayerInfo() 
    {
        characterBackgroundImage.DOFade(0f, 1.5f);
        characterName.DOFade(0f, 1.5f);
        characterAmplify.ClearAmplifyInfo();
    }
}
[Serializable]
public class PlayerCursor 
{
    public Player curPlayer;
    public GameObject cursorObject;
    public Image cursorImage;
    public TMP_Text cursorText;
    public bool isConnected;
    [SerializeField] public float xVal, yVal;
    [SerializeField, Range(0f, 1f)] public float xYield, yYield;
}
