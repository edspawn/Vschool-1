using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HttpConnector : MonoBehaviour
{
    //TODO move to config file
    public const string ServerUrl = "http://localhost:63866";
    //private const string ServerUrl = "http://virtual.itschool.ssau.ru";

    public const string CourseDataUrl = "/Render/UnityData";
    public const string StatUrl = "/Render/UnityStat";
    public const string SaveStatisticUrl = "/Render/UnitySave";
    public const string UnityListUrl = "/Render/UnityList";
    public const string UnitySaveRpgUrl = "/Render/UnitySaveRPG";
    public const string SaveGameAchievementUrl = "/Render/SaveGameAchievement";
    public const string GetGameAchievementsUrl = "/Render/GetGameAchievementsForUnity";

    public void Get(string url, System.Action<WWW> onSuccess)
    {
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www, onSuccess));
    }

    public void Post(string url, Dictionary<string, string> post, System.Action<WWW> onSuccess)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<String, String> postArg in post)
        {
            form.AddField(postArg.Key, postArg.Value);
        }
        WWW www = new WWW(url, form);

        StartCoroutine(WaitForRequest(www, onSuccess));
    }

    private IEnumerator WaitForRequest(WWW www, System.Action<WWW> onSuccess)
    {
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            onSuccess(www);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}
