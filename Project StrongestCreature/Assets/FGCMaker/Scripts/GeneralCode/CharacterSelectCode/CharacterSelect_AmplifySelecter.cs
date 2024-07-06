using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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

    public void SetAmplifyInfo()
    {
        chosenAmplifier.DOFade(1f, 0f);
        LeftButton.interactable = true;
        RightButton.interactable = true;
        LeftButton.image.DOFade(1f, 0f);
        RightButton.image.DOFade(1f, 0f);
    }
    public void ClearAmplifyInfo()
    {
        LeftButton.interactable = false;
        RightButton.interactable = false;
        chosenAmplifier.DOFade(0f, 1.5f);
        LeftButton.image.DOFade(0f, 1.5f);
        RightButton.image.DOFade(0f, 1.5f);
    }
}
