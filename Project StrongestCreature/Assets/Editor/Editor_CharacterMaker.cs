using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_CharacterMaker : EditorWindow
{
    [MenuItem("Window/Roster/Characters/CreateNewCharacter")]
    static void CreateNewCharacter()
    {
        Editor_CharacterMaker window = (Editor_CharacterMaker)GetWindow(typeof(Editor_CharacterMaker));
        window.minSize = new Vector2(650f, 300f);
        window.maxSize = new Vector2(1500f, 800f);
        window.Show();
    }
}
