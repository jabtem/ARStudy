using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWebCam : MonoBehaviour
{
    WebCamTexture texWeb;
    public GameObject target;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);

        if(Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone))
        {
            WebCamDevice[] tempDevices = WebCamTexture.devices;
            foreach(WebCamDevice w in tempDevices)
            {
                print(w.name);
            }
            texWeb = new WebCamTexture(tempDevices[0].name);

            texWeb.Play();
            target.GetComponent<Renderer>().material.mainTexture = texWeb;
            // 안드로이드 빌드시 주석 해제
            // 방향치라 고생했음;
#if UNITY_ANDROID
            target.transform.localRotation = Quaternion.Euler(0, 0, (Application.platform == RuntimePlatform.Android) ? target.transform.localRotation.z + 90 : 180);
#endif

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
