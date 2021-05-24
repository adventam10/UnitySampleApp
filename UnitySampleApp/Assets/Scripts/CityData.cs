using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TmpList
{
    public CityData[] list;
}

[System.Serializable]
public class CityData : ISerializationCallbackReceiver
{
    public string cityId;
    public string name;
    public int area;

    [SerializeField]
    [HideInInspector]
    private string name_en;
    [System.NonSerialized]
    public string nameEn;

    public void OnBeforeSerialize()
    {
        name_en = nameEn;
    }

    public void OnAfterDeserialize()
    {
        nameEn = name_en;
    }
}
