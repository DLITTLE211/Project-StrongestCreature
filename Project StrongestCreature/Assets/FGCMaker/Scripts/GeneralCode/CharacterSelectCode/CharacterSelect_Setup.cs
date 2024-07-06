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
    [SerializeField] private List<Character_Profile> _activeProfiles;
    [SerializeField] private List<Amplifiers> _activeAmplifiers;
    [SerializeField] private GameObject characterSelectButtonPrefab;
    [SerializeField] private GameObject characterSelectHolder;
    [SerializeField] private List<GameObject> activeCharacterSelectButtons;
    [SerializeField] private CharacterSelectPage _leftPlayerPage,_rightPlayerPage;
    // Start is called before the first frame update
    void Start()
    {
        AddCharacterSelectButtons();
        Messenger.AddListener<Character_Profile,int> (Events.DisplayCharacterInfo, DisplayCharacterSelectInformation);
        _leftPlayerPage.characterAmplify.GetListOfAmplifiers(_activeAmplifiers);
        _rightPlayerPage.characterAmplify.GetListOfAmplifiers(_activeAmplifiers);
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
}
