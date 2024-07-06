using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class CharacterSelect_AmplifySelecter :MonoBehaviour
{
    public TMP_Text chosenAmplifier;
    public Button LeftButton;
    public Button RightButton;
    private List<Amplifiers> totalAmplifiers;
    int curAmplifier;
    public void GetListOfAmplifiers(List<Amplifiers> _totalAmplifiers)
    {
        totalAmplifiers = _totalAmplifiers;
        curAmplifier = 0;
    }
    public void UpdateInfoDown()
    {
        if (curAmplifier <= 0)
        {
            curAmplifier = totalAmplifiers.Count - 1;
        }
        else { curAmplifier--; }
        SetInfo(totalAmplifiers[curAmplifier]);
    }
    public void UpdateInfoUp()
    {
        if (curAmplifier >= totalAmplifiers.Count - 1)
        {
            curAmplifier = 0;
        }
        else { curAmplifier++; }
        SetInfo(totalAmplifiers[curAmplifier]);
    }

    public void SetInfo(Amplifiers curAmplifier)
    {
        chosenAmplifier.text = curAmplifier.amplifier.ToString();
    }
}
