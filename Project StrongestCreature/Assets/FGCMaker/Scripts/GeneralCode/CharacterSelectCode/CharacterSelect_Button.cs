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
    public BoxCollider2D hoverCollider2D;
    [SerializeField] private Transform leftCursor, rightCursor;
    public enum hoverState {none,left,right,both }
    public hoverState _hoverState;

    bool leftHover,rightHover;
    public void Start()
    {
        hoverCollider2D = hoverImage.GetComponent<BoxCollider2D>();
        selectionState.onClick.AddListener(() => SendCharacterSelected());
    }
    public void GetLeftCursor(Transform Cursor) 
    {
        leftCursor = Cursor;
    }
    public void GetRightCursor(Transform Cursor)
    {
        rightCursor = Cursor;
    }
    private void Update()
    {
        CheckCursorPos();
        SetHoverColor();
    }
    void CheckCursorPos() 
    {
        if (leftCursor != null && leftCursor.gameObject.activeInHierarchy)
        {
            if (CheckCursorOverlap(leftCursor))
            {
                leftHover = true;
                HighlightSelection(leftCursor.GetComponent<CharacterSelect_Cursor>());
            }
            else
            {
                leftHover = false;
                UnselectButton(leftCursor.GetComponent<CharacterSelect_Cursor>().ID);
            }
        }
        if (rightCursor != null && rightCursor.gameObject.activeInHierarchy)
        {
            if (CheckCursorOverlap(rightCursor))
            {
                rightHover = true;
                HighlightSelection(rightCursor.GetComponent<CharacterSelect_Cursor>());
            }
            else
            {
                rightHover = false;
                UnselectButton(rightCursor.GetComponent<CharacterSelect_Cursor>().ID);
            }
        }
    }
    void SetHoverColor() 
    {
        if (!leftHover && !rightHover)
        {
            _hoverState = hoverState.none;
        }
        switch (_hoverState) 
        {
            case hoverState.both:
                hoverImage.color = Color.white;
                break;
            case hoverState.left:
                hoverImage.color = Color.red;
                break;
            case hoverState.right:
                hoverImage.color = Color.blue;
                break;
            case hoverState.none:
                hoverImage.color = Color.black;
                break;
        }
    }
    public void HighlightSelection(CharacterSelect_Cursor cursor)
    {
        if (leftHover && rightHover)
        {
            _hoverState = hoverState.both;
            if (hoverImage.color != Color.white)
            {
                Messenger.Broadcast<Character_Profile, int>(Events.DisplayCharacterInfo, characterProfile, cursor.ID);
            }
        }
        else
        {

            if (cursor.ID == 0)
            {
                _hoverState = hoverState.left;
                if (hoverImage.color != Color.red)
                {
                    Messenger.Broadcast<Character_Profile, int>(Events.DisplayCharacterInfo, characterProfile, cursor.ID);
                }

            }
            else if (cursor.ID == 1)
            {
                _hoverState = hoverState.right;
                if (hoverImage.color != Color.blue)
                {
                    Messenger.Broadcast<Character_Profile, int>(Events.DisplayCharacterInfo, characterProfile, cursor.ID);
                }
            }
        }
    }
    public void UnselectButton(int ID)
    {
        if (hoverImage.color != Color.black)
        {
            Messenger.Broadcast<int>(Events.ClearCharacterInfo, ID);
        }
    }
    public void SendCharacterSelected() 
    {

    }
    bool CheckCursorOverlap(Transform cursor) 
    {
        Bounds buttonSize = new Bounds(Vector3.zero, hoverCollider2D.transform.lossyScale);
        if (cursor.GetComponent<BoxCollider2D>().transform.localPosition.x < buttonSize.max.x + 60 && cursor.GetComponent<BoxCollider2D>().transform.localPosition.x > buttonSize.min.x - 60
            && cursor.GetComponent<BoxCollider2D>().transform.localPosition.y < buttonSize.max.y + 60 && cursor.GetComponent<BoxCollider2D>().transform.localPosition.y > buttonSize.min.y - 110)
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
    public void OnApplicationQuit()
    {
        selectionState.onClick.RemoveListener(() => SendCharacterSelected());
    }
}