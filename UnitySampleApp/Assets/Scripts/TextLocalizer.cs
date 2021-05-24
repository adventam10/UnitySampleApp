using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    [SerializeField]
    private string _text_jp;
    [SerializeField]
    private string _text_en;

    void Start()
    {
        SetLanguageUI();
    }

    private void SetLanguageUI() {
        Text text = GetComponent<Text>();
        SystemLanguage sl = Application.systemLanguage;
        switch (sl) {
            case SystemLanguage.Japanese:
                if (text) 
                {
                    text.text = _text_jp;
                }
                break;
            default:
                if (text) 
                {
                    text.text = _text_en;
                }
                break;
        }
    }
}
