using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WeatherScene : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _telop;
    [SerializeField]
    private Text _description;

    void Start()
    {
        GameObject resultObj = GameObject.Find ("SceneHandler");
        Weather weather = resultObj.GetComponent<CityListScene> ().result;
        _title.text = weather.title;
        _telop.text = weather.forecasts[0].telop;
        _description.text = weather.description.bodyText;
        Destroy(resultObj);
    }

    public void BackScene()
    {
        SceneManager.LoadScene("CityListScene");
    }
}
