using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_CharacterMaker : EditorWindow
{
    Texture2D _headerSection, 
              _mainSection, 
              _completedBoolSection,
              _createCharacterButtonSection;

    Color32 headerColor = new Color32((byte)150f,(byte)255f, (byte)45f, (byte)255f);
    Color32 mainSectionColor = new Color32((byte)180f, (byte)18f, (byte)95f, (byte)255f);
    Color32 boolSectionColor = new Color32((byte)145f, (byte)0f, (byte)50f, (byte)255f);
    Color32 createCharacterColor = new Color32((byte)255f, (byte)255f, (byte)255f, (byte)255f);

    Rect _headerSectionRect, 
         _mainSectionRect,
         _completedBoolRect,
         _createCharacterRect;


    [MenuItem("Window/Roster/Characters/CreateNewCharacter")]
    static void CreateNewCharacter()
    {
        Editor_CharacterMaker window = (Editor_CharacterMaker)GetWindow(typeof(Editor_CharacterMaker));
        window.minSize = new Vector2(800f, 450f);
        window.maxSize = new Vector2(1600f, 900f);
        window.Show();
    }

    private void OnEnable()
    {
        InitTextures();
    }
    void InitTextures() 
    {
        _headerSection = new Texture2D(1, 1);
        _headerSection.SetPixel(0, 0, headerColor);
        _headerSection.Apply();


        _mainSection = new Texture2D(1, 1);
        _mainSection.SetPixel(0, 0, mainSectionColor);
        _mainSection.Apply();


        _completedBoolSection = new Texture2D(1, 1);
        _completedBoolSection.SetPixel(0, 0, boolSectionColor);
        _completedBoolSection.Apply();


        _createCharacterButtonSection = new Texture2D(1, 1);
        _createCharacterButtonSection.SetPixel(0, 0, createCharacterColor);
        _createCharacterButtonSection.Apply();
    }
    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawMainCharacterSettings();
        DrawCompletedBool();
        DrawCreateCharacter();
    }


    #region Drawing Functions
    void DrawLayouts() 
    {
        _headerSectionRect.x = 0f;
        _headerSectionRect.y = 0f;
        _headerSectionRect.width = Screen.width;
        _headerSectionRect.height = 35f;
        GUI.DrawTexture(_headerSectionRect,_headerSection);


        _mainSectionRect.x = 0f;
        _mainSectionRect.y = _headerSectionRect.height;
        _mainSectionRect.width = Screen.width - (Screen.width /2.75f);
        _mainSectionRect.height = Screen.height - 135f;
        GUI.DrawTexture(_mainSectionRect, _mainSection);


        _completedBoolRect.x = 0f + _mainSectionRect.width;
        _completedBoolRect.y = _headerSectionRect.height;
        _completedBoolRect.width = Screen.width - _mainSectionRect.width;
        _completedBoolRect.height = Screen.height - 135f;
        GUI.DrawTexture(_completedBoolRect, _completedBoolSection);

        _createCharacterRect.width = Screen.width;
        _createCharacterRect.height = 75f;
        _createCharacterRect.x = 0;
        _createCharacterRect.y = (Screen.height - 20f) - _createCharacterRect.height;
        GUI.DrawTexture(_createCharacterRect, _createCharacterButtonSection);

    }
    void DrawHeader() 
    {
        GUILayout.BeginArea(_headerSectionRect);
        #region FillArea

        #endregion
        GUILayout.EndArea();
    }
    void DrawMainCharacterSettings() 
    {
        GUILayout.BeginArea(_mainSectionRect);
        #region FillArea

        #endregion
        GUILayout.EndArea();
    }
    void DrawCompletedBool()
    {
        GUILayout.BeginArea(_completedBoolRect);
        #region FillArea

        #endregion
        GUILayout.EndArea();
    }
    void DrawCreateCharacter()
    {
        GUILayout.BeginArea(_createCharacterRect);
        #region FillArea

        #endregion
        GUILayout.EndArea();
    }
    #endregion
}
