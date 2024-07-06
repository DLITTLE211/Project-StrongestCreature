using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class CharacterSelect_Setup : MonoBehaviour
{
    [SerializeField] private List<Character_Profile> _activeProfiles;
    [SerializeField] private GameObject characterSelectButtonPrefab;
    [SerializeField] private GameObject characterSelectHolder;
    [SerializeField] private List<CharacterSelect_Button> activeCharacterSelectButtons;
    [SerializeField] private CharacterSelectPage _leftPlayerPage,_rightPlayerPage;
    // Start is called before the first frame update
    void Start()
    {
        AddCharacterSelectButtons();
        Messenger.AddListener<Character_Profile,int> (Events.DisplayCharacterInfo, DisplayCharacterSelectInformation);
    }
    public void AddCharacterSelectButtons()
    {
        StartCoroutine(CascadeActivateSelectButtons());
    }
    IEnumerator CascadeActivateSelectButtons()
    {
        for (int i = 0; i < _activeProfiles.Count; i++)
        {
            yield return new WaitForSeconds(0.05f);
            GameObject selectButton = Instantiate(characterSelectButtonPrefab, characterSelectHolder.transform);
            selectButton.gameObject.transform.localPosition = new Vector3(1, 1, 1);
            selectButton.gameObject.transform.localRotation = Quaternion.identity;
            selectButton.gameObject.transform.localScale = Vector3.zero;
            selectButton.GetComponentInChildren<Button>().image.sprite = _activeProfiles[i].CharacterProfileImage;
            Vector3 selectButtonFirstSize = new Vector3(1.25f, 1.25f, 1.25f);
            selectButton.gameObject.transform.DOScale(selectButtonFirstSize, 0.15f).OnComplete(() => 
            {
                selectButton.gameObject.transform.DOScale(Vector3.one, 0.05f);
                CharacterSelect_Button _selectButtonInfo = selectButton.GetComponentInChildren<CharacterSelect_Button>();
                _selectButtonInfo.characterProfile = _activeProfiles[i];
                activeCharacterSelectButtons.Add(_selectButtonInfo);
            });
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
    public AmplifyCycler characterAmplify;
    public void UpdateInfo(Character_Profile profile) 
    {
        characterBackgroundImage.preserveAspect = true;
        characterBackgroundImage.sprite = profile.CharacterProfileImage;
        characterName.text = profile.CharacterName;
    }
}
[Serializable]
public class AmplifyCycler
{
    public TMP_Text chosenAmplifier;
    public Button LeftButton;
    public Button RightButton;
    private List<Amplifiers> totalAmplifiers;
    int curAmplifier;
    public void GetListOfAmplifiers(List<Amplifiers> _totalAmplifiers) 
    {
        totalAmplifiers = _totalAmplifiers;
        curAmplifier = 0;
    }
    public void UpdateInfoDown()
    {
        if (curAmplifier <= 0) 
        {
            curAmplifier = totalAmplifiers.Count - 1;
        }
        else { curAmplifier--; }
        SetInfo(totalAmplifiers[curAmplifier]);
    }
    public void UpdateInfoUp()
    {
        if (curAmplifier >= totalAmplifiers.Count - 1)
        {
            curAmplifier = 0;
        }
        else { curAmplifier++; }
        SetInfo(totalAmplifiers[curAmplifier]);
    }

    public void SetInfo(Amplifiers curAmplifier) 
    {
       chosenAmplifier.text = curAmplifier.amplifier.ToString();
    }
}
