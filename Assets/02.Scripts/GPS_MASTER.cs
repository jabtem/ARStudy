using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

/*
    GPS좌표간 거리계산으로 SVS 학원에 도착하면 노티가 뜨는 GPS.

    (참고) 구글맵을 이용하면 경위도 좌표 구함
*/

public class GPS_MASTER : MonoBehaviour {
    //public ViewHandler v;
    public Text debugText;
 
    LocationInfo myGPSLocation;
    float fiveSecondCounter = 0.0f;
 
    public string LocationName;
 
    double MyLatitude, MyLongtitude;
    DistUnit unit;
 
    /**
    * 구글을 통한 학원 GPS 
    * Latitude - 경도, Longtitude - 위도
    */
    public double TargetLatitude, TargetLongtitude; // 37.507839, 127.039864
 
 
 
    IEnumerator Start () {
 
        debugText.text += "GPS Script 시작\n";
 
#if UNITY_ANROID
         Input.compass.enabled = true;  // 나침반 기능 활성화
#endif
 
        return InitializeGPSServices ();
    }
 
    IEnumerator InitializeGPSServices () {
        // 우선 사용자가 위치 서비스를 활성화 안했다면...(수락 거부)
        if (!Input.location.isEnabledByUser) {
            debugText.text += "사용자에 의해서 GPS 차단\n";
            yield break;
        }

        // querying location 전에 서비스 시작.
        Input.location.Start (0.1f, 0.1f);
 
        // 서비스 초기화까지 대기...
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds (1);
            maxWait--;
        }
 
        // 20초안에 서비스가 초기화되지 않았다면 아웃~(안전상...)
        if (maxWait < 1) {
            debugText.text += "타임 아웃\n";
            yield break;
        }
    }
 
    void Update () {
        // 프레임을 걸어준다...
        fiveSecondCounter += Time.deltaTime;
        if (fiveSecondCounter > 1.0) {
            UpdateGPS ();
            fiveSecondCounter = 0.0f;
        }
    }
 
    int updateCnt = 0;
 
    void UpdateGPS () {
        // 연결 실패시...
        if (Input.location.status == LocationServiceStatus.Failed) {
            debugText.text += "디바이스 위치 추적 불가\n";
            // 멈추고
            Input.location.Stop ();
            // 다시 시작
            Start ();
        }
        else {
            updateCnt++;
            debugText.text = "update count : " + updateCnt + "\n" + GetUpdatedGPSstring ();
        }
    }
 
    string GetUpdatedGPSstring () {
        myGPSLocation = Input.location.lastData;

        // Math.Round() => 값을 가장 가까운 정수나 지정된 소수 자릿수로 반올림.
        MyLongtitude = Math.Round (myGPSLocation.longitude, 6); // 위도
        MyLatitude = Math.Round (myGPSLocation.latitude, 6);    // 경도
 
        double DistanceToMeter;
        string storeRange;
 
        //두 점간의 거리 : 이것은 사용자정의 함수임... 
        DistanceToMeter = Distance (MyLatitude, MyLongtitude, TargetLatitude, TargetLongtitude, DistUnit.meter);
        //DistanceToMeter = distance (37.507775, 127.039675, 37.507660, 127.039530, "meter"); 

        // 20미터 이내 거리체크
        if (DistanceToMeter < 20) {// 건물의 높낮이 등 환경적인 요소로 인해 오차가 발생 할 수 있음.
            storeRange = "근처학원 O";

            //if (!v.flowAHandler.NotiOpenChk)
            //    v.flowAHandler.OpenNoti ();
            Application.Quit();
        }
        else {
            storeRange = "근처학원 X";
        }
 
        return "\n현재위치 :\n" +
                            "경도 - " + Math.Round (MyLatitude, 6) + "\n" +
                            "위도 - " + Math.Round (MyLongtitude, 6) +
 
            "\n\n" + "목표위치 : " + LocationName + "\n" +
                            "경도 - " + TargetLatitude + "\n" +
                            "위도 - " + TargetLongtitude +
 
            "\n\n목표와의거리 : 약 " + DistanceToMeter + "M" + "\n" +
                            "-------------------------------\n\n" +
 
                            storeRange;
    }
 
 
 
 
    /**
     * 두 지점간의 거리 계산
     *
     * @param lat1 지점 1 위도
     * @param lon1 지점 1 경도
     * @param lat2 지점 2 위도
     * @param lon2 지점 2 경도
     * @param unit 거리 표출단위
     * @return
     */
    // 공식화 된것이니 일단 받아드리자...
    static double Distance (double lat1, double lon1, double lat2, double lon2, DistUnit unit) {
 
        double theta = lon1 - lon2;
        // Math.Sin(), Math.Cos => sin,cos 삼각함수 구함, 일단 수학적인 함수들은 라디안 값을 받아들임..연산이 빨름...
        // Deg2rad, Rad2deg 사용자 정의 함수임.
        double dist = Math.Sin (Deg2rad (lat1)) * Math.Sin (Deg2rad (lat2)) + Math.Cos (Deg2rad (lat1)) * Math.Cos (Deg2rad (lat2)) * Math.Cos (Deg2rad (theta));

        // Math.Acos => 역 cos 함수
        dist = Math.Acos (dist);
        dist = Rad2deg (dist);
        dist = dist * 60 * 1.1515;
        // dist = dist * 1.609344 * 1000; //미터로변환

        // 1 km => 0.6214 mi(마일), 1 mi(마일) => 1.6093 km ㅋㅋ
        if (unit == DistUnit.kilometer) {
            dist = dist * 1.609344;
        }
        else if (unit == DistUnit.meter) {
            dist = dist * 1609.344;
        }
 
        return (dist);
    }

    // decimal degrees => radians 변환
    static double Deg2rad (double deg) {
        return (deg * Math.PI / 180.0);
    }
 
    // radians => decimal degrees 변환
    static double Rad2deg (double rad) {
        return (rad * 180 / Math.PI);
    }
 
}
 
enum DistUnit{
    kilometer,
    meter
}