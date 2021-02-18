using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GpsStudyTest : MonoBehaviour
{

    public Text[] txt;

    // Use this for initialization
    void Start()
    {

        StartCoroutine(StartGPS());

    }

    // 유니티에서 위치는 다음과 같이 가져올 수 있다.
    IEnumerator GetGPS()
    {

        // 사용자 위치 권한 체크
        if (!Input.location.isEnabledByUser)
            yield break;

        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            // 초기화 대기
            yield return new WaitForSeconds(1);
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //실패
            yield break;
        }
        else
        {
            txt[0].text = "위도" + Input.location.lastData.latitude;
            txt[1].text = "경도" + Input.location.lastData.longitude;
            txt[2].text = "고도" + Input.location.lastData.altitude;

            Debug.Log("위도" + Input.location.lastData.latitude);
            Debug.Log("경도" + Input.location.lastData.longitude);
            Debug.Log("고도" + Input.location.lastData.altitude);
        }

        Input.location.Stop();

    }

    IEnumerator StartGPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(GetGPS());
        }
    }


}

/*
    위도와 경도에 대해서는 따로 설명할 수 없으니 직접 공부해야 할 것이다. 난이도가 있는 도전 과제지만
    해결한다면 정말로 자신만의 포켓몬 고를 만들 수 있을 것이다.
*/