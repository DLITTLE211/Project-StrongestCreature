using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;

public class CharacterSelect_Cursor : MonoBehaviour
{
    public Player curPlayer;
    public int ID;
    public GameObject cursorObject;
    public Image cursorImage;
    public TMP_Text cursorText;
    public bool isConnected;
    [SerializeField] public float xVal, yVal;
    [SerializeField, Range(0f, 1f)] public float xYield, yYield;
}
