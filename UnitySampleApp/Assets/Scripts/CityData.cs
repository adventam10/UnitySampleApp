using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TmpList
{
    public CityData[] list;
}

[System.Serializable]
public class CityData
{
    public string cityId;
    public string name;
    public int area;
}
