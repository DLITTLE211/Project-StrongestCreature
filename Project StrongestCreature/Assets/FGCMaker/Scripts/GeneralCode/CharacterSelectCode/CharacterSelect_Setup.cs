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
    [SerializeField] private Player _leftPlayer,_rightPlayer;
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
                _leftPlayer = ReInput.players.GetPlayer(players.UsedID.Item1[0]);
                _leftPlayer.controllers.AddController(ControllerType.Joystick, players.UsedID.Item1[0], true);
                _leftPlayer.controllers.maps.LoadMap(ControllerType.Joystick, players.UsedID.Item1[0], $"UI_CanvasController", $"TestPlayer{players.UsedID.Item1[0]}");
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
                        _leftPlayer = ReInput.players.GetPlayer(players.UsedID.Item1[i]);
                        _leftPlayer.controllers.AddController(ControllerType.Joystick, players.UsedID.Item1[i], true);
                        _leftPlayer.controllers.maps.LoadMap(ControllerType.Joystick, players.UsedID.Item1[i], $"UI_CanvasController", $"TestPlayer{players.UsedID.Item1[i]}");
                    }
                    if (i == 1)
                    {
                        _rightPlayer = ReInput.players.GetPlayer(players.UsedID.Item1[i]);
                        _rightPlayer.controllers.AddController(ControllerType.Joystick, players.UsedID.Item1[i], true);
                        _rightPlayer.controllers.maps.LoadMap(ControllerType.Joystick, players.UsedID.Item1[i], $"UI_CanvasController", $"TestPlayer{players.UsedID.Item1[i]}");
                    }
                    else { continue; }
                }
            }
        }
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
