using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect_LoadArena : MonoBehaviour
{
    public void OnCharacterSelected() 
    {
        LoadArena();
    }

    void LoadArena() 
    {
        SceneManager.LoadScene("MainGame_Arena");
    }
}
