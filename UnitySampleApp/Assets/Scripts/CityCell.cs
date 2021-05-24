using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCell : ListViewCell
{
    [SerializeField]
    private Text _text;

    private string _nameJa;
    private string _nameEn;

    public override void UpdateData(int index, object obj)
    {
        base.UpdateData(index, obj);
        var cityData = obj as CityData;
        _nameJa = cityData.name;
        _nameEn = cityData.nameEn;
    }

    void Update() {
        SystemLanguage sl = Application.systemLanguage;
        switch (sl)
        {
            case SystemLanguage.Japanese:
                _text.text = _nameJa;
                break;
            default:
                _text.text = _nameEn;
                break;
        }
    }
}