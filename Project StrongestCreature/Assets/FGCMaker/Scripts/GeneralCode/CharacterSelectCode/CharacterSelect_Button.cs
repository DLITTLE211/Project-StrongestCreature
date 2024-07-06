using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Rewired;

public class CharacterSelect_Button : MonoBehaviour
{
    public Character_Profile characterProfile;
    public Image hoverImage;

    public void OnMouseOver()
    {
        if (ReInput.players.GetSystemPlayer().id == 0)
        {
            hoverImage.color = Color.red;
        }
        else if (ReInput.players.GetSystemPlayer().id == 1)
        {
            hoverImage.color = Color.blue;
        }
        //IncludeCharacter Who Highlighted Over Button
        Messenger.Broadcast<Character_Profile,int>(Events.DisplayCharacterInfo,characterProfile, ReInput.players.GetSystemPlayer().id);
    }
    public void OnMouseExit()
    {
        hoverImage.color = Color.black;
    }
}
