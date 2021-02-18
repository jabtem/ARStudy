using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamScript : MonoBehaviour
{

    public GameObject WebcamPlane;
    public Button FireButton;
    public GameObject target;

    // Use this for initialization
    void Start()
    {
        if (Application.isMobilePlatform)
        {
            GameObject cameraParent = new GameObject("camParent");
            cameraParent.transform.position = this.transform.position;
            this.transform.parent = cameraParent.transform;

            // 안드로이드 모발일에서 카메라가 (0, 270, 90)으로 변한다.
            // 따라서, 현재 카메라의 부모의 회전값을 x : 90, y : 90 으로 한다. 
            //cameraParent.transform.Rotate(Vector3.right, 90);
            cameraParent.transform.rotation = Quaternion.Euler(90f, 90f, 0f);

            // 좀 복잡하게 해서 미안~
            WebcamPlane.transform.rotation = Quaternion.Euler(180, 90, 180);
            WebcamPlane.transform.parent.rotation = Quaternion.Euler(90, 180, 0);
            WebcamPlane.transform.localScale = new Vector3(1, 1, 0.575f);
        }
        else
        {
            GameObject cameraParent = new GameObject("camParent");
            cameraParent.transform.position = this.transform.position;
            this.transform.parent = cameraParent.transform;
            cameraParent.transform.Rotate(Vector3.forward, 0f);
        }

        Input.gyro.enabled = true;

        //WebCamTexture webCameraTexture = new WebCamTexture();

        WebCamDevice[] temp_devices = WebCamTexture.devices;
        foreach (WebCamDevice w in temp_devices)
        {
            print(w.name);
        }

        // 난 노트북이라 캠이 두개~ 노트북 캠 사용시 인덱스 0
        // 모바일 사용시 인덱스 0은 후방, 1은 전방 카메라
        WebCamTexture webCameraTexture = new WebCamTexture(temp_devices[0].name);

        WebcamPlane.GetComponent<MeshRenderer>().material.mainTexture = webCameraTexture;
        webCameraTexture.Play();

        FireButton.onClick.AddListener(OnButtonDown);

    }

    void OnButtonDown()
    {
        GameObject bullet = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = Camera.main.transform.rotation;
        bullet.transform.position = Camera.main.transform.position;
        rb.AddForce(Camera.main.transform.forward * 500f);
        Destroy(bullet, 3);

        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion cameraRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        this.transform.localRotation = cameraRotation;
    }
}
