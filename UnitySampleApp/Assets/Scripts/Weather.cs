using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weather
{
    public string title;
    public Description description;
    public Forecast[] forecasts;
}

[System.Serializable]
public class Forecast
{
    public string telop;
    public Image image;
}

[System.Serializable]
public class Image
{
    public string title;
    public string url;
}

[System.Serializable]
public class Description
{
    public string headlineText;
    public string bodyText;
    public string text;
}