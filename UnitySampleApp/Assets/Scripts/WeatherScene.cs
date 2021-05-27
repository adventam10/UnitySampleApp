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
    private MessageHandler _messageHandler;

    void Start()
    {
        GameObject resultObj = GameObject.Find("SceneHandler");
        CityListScene cityListScene = resultObj.GetComponent<CityListScene>();
        if (cityListScene != null)
        {
            Weather weather = cityListScene.result;
            _title.text = weather.title;
            _telop.text = weather.forecasts[0].telop;
            _messageHandler.SetupMessage(weather.description.text);
        }
        else
        {
            _messageHandler.SetupMessage("test1\n\ntest2");
        }

        Destroy(resultObj);
    }

    public void BackScene()
    {
        SceneManager.LoadScene("CityListScene");
    }
}
