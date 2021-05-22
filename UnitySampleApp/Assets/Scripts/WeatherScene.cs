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

    private List<string> _splitMessages = new List<string>();
    private int _splitMessagesIndex = 0;
    private int _nowMessageIndex = 0;
    private bool _isNowMessageEnd = false;
    private float _textSpeed = 0.05f;
    private float _elapsedTime = 0f;

    void Start()
    {
        GameObject resultObj = GameObject.Find ("SceneHandler");
        Weather weather = resultObj.GetComponent<CityListScene> ().result;
        _title.text = weather.title;
        _telop.text = weather.forecasts[0].telop;
        string allMessage = weather.description.text;
        allMessage = allMessage.Replace(" ", "");
        allMessage = allMessage.Replace("\n\n", "\n");
        Destroy(resultObj);

        _description.text = "";
        int count = 64;
        int length = Mathf.CeilToInt(allMessage.Length / count);
        for (int i = 0; i < length; i++) 
        {
            int start = count * i;
            if (start >= allMessage.Length) 
            {
                break;
            }
            if (start + count > allMessage.Length) 
            {
                _splitMessages.Add(allMessage.Substring(start));
            } 
            else 
            {
                _splitMessages.Add(allMessage.Substring(start, count));
            }
        }
    }

    void Update() {
        if (_isNowMessageEnd) 
        {
            _elapsedTime += Time.deltaTime; 
            if (Input.GetMouseButtonDown(0)) 
            {
                _nowMessageIndex = 0;
                _splitMessagesIndex++;
                _description.text = "";
                _elapsedTime = 0f;
                _isNowMessageEnd = false;
            }

            if (_splitMessagesIndex >= _splitMessages.Count) 
            {
                // 全部表示終了の場合最初から
                _nowMessageIndex = 0;
                _splitMessagesIndex = 0;
                _isNowMessageEnd = false;
            }
        } 
        else 
        {
            if (_elapsedTime >= _textSpeed) 
            {
                // 文字表示一文字ずつ足して表示していく
                _description.text += _splitMessages[_splitMessagesIndex][_nowMessageIndex];

                _nowMessageIndex++;
                _elapsedTime = 0f;
 
                if (_nowMessageIndex >= _splitMessages[_splitMessagesIndex].Length) 
                {
                    _isNowMessageEnd = true;
                }
            }
            _elapsedTime += Time.deltaTime;
 
            if (Input.GetMouseButtonDown(0)) 
            {
                //　表示中にタップされた場合残りをすべて表示
                _description.text += _splitMessages[_splitMessagesIndex].Substring(_nowMessageIndex);
                _isNowMessageEnd = true;
            }
        }
    }

    public void BackScene()
    {
        SceneManager.LoadScene("CityListScene");
    }
}
