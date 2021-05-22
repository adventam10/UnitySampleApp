using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCell : ListViewCell
{
    [SerializeField]
    private Text _text;

    public override void UpdateData(int index, object obj)
    {
        base.UpdateData(index, obj);
        var cityData = obj as CityData;
        _text.text = cityData.name;
    }
}