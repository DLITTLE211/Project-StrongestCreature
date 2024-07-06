using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Rewired;

public class CharacterSelect_Button : MonoBehaviour
{
    [SerializeField] private CharacterSelect_CharacterButton selectionState;
    public Character_Profile characterProfile;
    public Image hoverImage;

    public void Start()
    {
        selectionState.onClick.AddListener(() => SendCharacterSelected());
    }
    private void Update()
    {
        if (selectionState.isHovered())
        {
            HighlightSelection();
        }
        else 
        {
            UnselectButton();
        }

    }
    public void HighlightSelection()
    {
        if (ReInput.players.GetSystemPlayer().id == 0)
        {
            hoverImage.color = Color.red;
            Messenger.Broadcast<Character_Profile, int>(Events.DisplayCharacterInfo, characterProfile, ReInput.players.GetSystemPlayer().id);
        }
        else if (ReInput.players.GetSystemPlayer().id == 1)
        {
            hoverImage.color = Color.blue;
            Messenger.Broadcast<Character_Profile, int>(Events.DisplayCharacterInfo, characterProfile, ReInput.players.GetSystemPlayer().id);
        }
    }
    public void UnselectButton()
    {
        hoverImage.color = Color.black;
    }
    public void SendCharacterSelected() 
    {

    }
    public void OnApplicationQuit()
    {
        selectionState.onClick.RemoveListener(() => SendCharacterSelected());
    }
}