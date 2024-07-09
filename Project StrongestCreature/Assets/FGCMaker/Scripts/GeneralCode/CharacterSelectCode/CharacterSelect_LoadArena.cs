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
    public static Character_Profile leftPlayerChosenProfile, rightPlayerChosenProfile;
    public static Stage_StageAsset chosenStage;
    private void Awake()
    {
        _arenaLoaded = false;
    }
    public async void OnCharactersAndStageSelected() 
    {
        await LoadArena();
    }

    async Task LoadArena()
    {
        leftPlayerChosenProfile = _characterSelectSetup.GetLeftPlayerProfile();
        rightPlayerChosenProfile = _characterSelectSetup.GetRightPlayerProfile();
        chosenStage = _characterSelectSetup.GetChosenStage();
        Task[] tasks = new Task[]
        {
            _characterSelectSetup.ClearStageSelect(),
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
