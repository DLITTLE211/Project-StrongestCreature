using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_CharacterMaker : EditorWindow
{
    #region Visual Related code for Window
    Texture2D _headerSection, 
              _mainSection, 
              _completedBoolSection,
              _createCharacterButtonSection;

    Color32 headerColor = new Color32((byte)15f,(byte)15f, (byte)15f, (byte)255f);
    Color32 mainSectionColor = new Color32((byte)25f, (byte)25f, (byte)25f, (byte)255f);
    Color32 boolSectionColor = new Color32((byte)35f, (byte)35f, (byte)35f, (byte)255f);
    Color32 createCharacterColor = new Color32((byte)20f, (byte)20f, (byte)20f, (byte)255f);

    Rect _headerSectionRect, 
         _mainSectionRect,
         _completedBoolRect,
         _createCharacterRect;
    #endregion

    Character_Profile _newProfile;
    public Character_Profile CurrentProfile { get { return _newProfile; } }

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
        InitData();
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
    void InitData() 
    {
        _newProfile = (Character_Profile)ScriptableObject.CreateInstance(typeof(Character_Profile));
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
        _mainSectionRect.y = _headerSectionRect.height + 2.5f;
        _mainSectionRect.width = Screen.width - (Screen.width /2.75f);
        _mainSectionRect.height = Screen.height - 135f;
        GUI.DrawTexture(_mainSectionRect, _mainSection);


        _completedBoolRect.x = 0f + _mainSectionRect.width;
        _completedBoolRect.y = _headerSectionRect.height + 2.5f;
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
        GUILayout.Label("Character Creator");
        #endregion
        GUILayout.EndArea();
    }
    void DrawMainCharacterSettings() 
    {
        GUILayout.BeginArea(_mainSectionRect);

        #region FillArea
        GUI.skin.label.fontSize = 20;
        GUILayout.Label("Character Data");
        GUILayout.Label("Character Identification Info");
        EditorGUILayout.BeginHorizontal();
        _newProfile.CharacterName = (string)EditorGUILayout.TextField("Character Name: ", _newProfile.CharacterName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _newProfile.CharacterID = (int)EditorGUILayout.IntField("Character ID", _newProfile.CharacterID);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        var spriteRect = new Rect(position.x, position.y, 50, 50);
        _newProfile.CharacterProfileImage = (Sprite)EditorGUILayout.ObjectField("Character Sprite",_newProfile.CharacterProfileImage,typeof(Texture2D),false);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _newProfile.CharacterName = (string)EditorGUILayout.TextField(_newProfile.CharacterName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _newProfile.CharacterName = (string)EditorGUILayout.TextField(_newProfile.CharacterName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _newProfile.CharacterName = (string)EditorGUILayout.TextField(_newProfile.CharacterName);
        EditorGUILayout.EndHorizontal();
        #endregion
        GUILayout.EndArea();
    }
    void DrawCompletedBool()
    {
        GUILayout.BeginArea(_completedBoolRect);
        #region FillArea
        GUILayout.Label("Character Data Filled");

        #endregion
        GUILayout.EndArea();
    }
    void DrawCreateCharacter()
    {
        GUILayout.BeginArea(_createCharacterRect);
        #region FillArea

        GUILayout.Label("Confirm Character");
        if (GUILayout.Button("Create New Character", GUILayout.Width(155), GUILayout.Height(30)))
        {
            if (CheckIfFieldsFilled())
            {

            }
            else 
            {
                Debug.LogError("Not All Fields have been filled. Fill in all new character data!");
            }
            
        }
        #endregion
        GUILayout.EndArea();
    }
    bool CheckIfFieldsFilled() 
    {
        return false;
    }
    #endregion
}
