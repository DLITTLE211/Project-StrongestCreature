using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class CharacterSelect_LoadArena : MonoBehaviour
{
    [SerializeField] private CharacterSelect_Setup _characterSelectSetup;
    private bool _arenaLoaded;
    private void Awake()
    {
        _arenaLoaded = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!_arenaLoaded)
            {
                OnCharacterSelected();
            }
        }
    }
    public void OnCharacterSelected() 
    {
        LoadArena();
    }

    async Task LoadArena()
    {   
        Task[] tasks = new Task[]
        {
            _characterSelectSetup.ClearCharacterSelectInfo(),
            _characterSelectSetup.ClearLeftPlayerInfo(),
            _characterSelectSetup.ClearRightPlayerInfo(),
        };
        await Task.WhenAll(tasks);
        SceneManager.LoadScene("MainGame_Arena", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MainGame_CharacterSelect");
    }

    private void OnDestroy()
    {
        SceneManager.LoadScene("MainGame_CharacterSelect", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("MainGame_Arena");
        _arenaLoaded = false;
    }
}
