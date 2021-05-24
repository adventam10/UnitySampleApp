using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CityListScene : MonoBehaviour, ListViewDataSource
{
    [SerializeField]
    private ListView _listView;
    private CityData[] _cityList;

    public Weather result;

    void Start()
    {
        DontDestroyOnLoad(this);

        var textAsset = Resources.Load ("CityDataList") as TextAsset;
        var tmpList = JsonUtility.FromJson<TmpList>(textAsset.text);
        _cityList = tmpList.list;

        _listView.dataSource = this;
        _listView.selector = delegate (int index) {
            var city = _cityList[index];
            StartCoroutine("OnSend", "https://weather.tsukumijima.net/api/forecast?city=" + city.cityId);
        };
        _listView.ReloadData();
    }

    IEnumerator OnSend(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        // 結果が戻ってくるまで待機
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Weather w = JsonUtility.FromJson<Weather>(request.downloadHandler.text);
            result = w;
            SceneManager.LoadScene("WeatherScene");
        }
    }

    public object Data(int index) {
        return _cityList[index];
    }

    public int dataCount { 
        get { return _cityList.Length; }
    }
}
