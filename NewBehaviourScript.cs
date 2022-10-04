using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class NewBehaviourScript : MonoBehaviour
{   
    public AudioClip goodSpeak;
    public AudioClip normalSpeak;
    public AudioClip badSpeak;
    private AudioSource selectAudio;
    private Dictionary<string,float> dataSet = new Dictionary<string, float>();
    private bool statusStart = false;
    private int i = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoogleSheets());
    }

    // Update is called once per frame
    void Update()
    {   
        if (i > dataSet.Count) {
            return;
        }

        if (dataSet["iter_" + i.ToString()] > 1000 & statusStart == false & i <= dataSet.Count){
            StartCoroutine(PlaySelectAudioBad());
            Debug.Log(dataSet["iter_" + i.ToString()] + " " + i.ToString());
        }

        if (dataSet["iter_" + i.ToString()] < 1000 & dataSet["iter_" + i.ToString()] > 100 & statusStart == false & i <= dataSet.Count){
            StartCoroutine(PlaySelectAudioNormal());
            Debug.Log(dataSet["iter_" + i.ToString()] + " " + i.ToString());
        }

        if (dataSet["iter_" + i.ToString()] < 100 & statusStart == false & i <= dataSet.Count){
            StartCoroutine(PlaySelectAudioGood());
            Debug.Log(dataSet["iter_" + i.ToString()] + " " + i.ToString());
        }
    }

    IEnumerator GoogleSheets()
    {   
        UnityWebRequest curentResp = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1tQY186QDGvT6w5jB9ANhIBNiu6xNn-cnaYNWLKtC2x4/values/Лист1?key=AIzaSyCEjAgiUJ-ZSmQ-w93qEoEG_36ya0Z61IU");
        yield return curentResp.SendWebRequest();
        string rawResp = curentResp.downloadHandler.text;
        var rawJson = JSON.Parse(rawResp);
        foreach (var itemRawJson in rawJson["values"])
        {
            var parseJson = JSON.Parse(itemRawJson.ToString());
            var selectRow = parseJson[0].AsStringList;
            dataSet.Add(("iter_" + selectRow[0]), float.Parse(selectRow[2]));
        }
    }

    IEnumerator PlaySelectAudioGood()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = goodSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        statusStart = false;
        i++;
    }
    IEnumerator PlaySelectAudioNormal()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = normalSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(3);
        statusStart = false;
        ++i;
    }
    IEnumerator PlaySelectAudioBad()
    {
        statusStart = true;
        selectAudio = GetComponent<AudioSource>();
        selectAudio.clip = badSpeak;
        selectAudio.Play();
        yield return new WaitForSeconds(4);
        statusStart = false;
        i++;
    }
}