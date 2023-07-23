using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;
using UnityEditor;
public class FileView5 : MonoBehaviour
{
    public Image img;
    Renderer Img2;
    string imgString;
    string imgnum = "image5";
    byte[] imgByte;
    int timer=0;
    public static string useruid = DataManager.instance.inputRoomName.text;


    void Start()
    {
        Img2 = gameObject.GetComponent<Renderer>();
        StartCoroutine (LoadImage(useruid, imgnum));
    }

    // Update is called once per frame
    void Update()
    {
        timer++;
        if (timer==60)
        {
            StartCoroutine (LoadImage(useruid, imgnum)); // 60프레임마다 이미지를 새로고침(불러오기)
            timer=0;
        }   
    }

    IEnumerator LoadImage(string useruid, string imgnum)
    { // 이미지 불러오기 php로 uid, 액자 번호 파라미터를 보내는 함수
        string imgloadURL = "http://metasium.dothome.co.kr/imgload.php"; // 이미지 불러오기 php url
        WWWForm form = new WWWForm ();
        form.AddField ("UserUidPost", useruid);
        form.AddField ("FilePost",imgnum);

        UnityWebRequest www = UnityWebRequest.Post(imgloadURL,form);
        // form에 uid, 액자 번호 저장해서 php로 파라미터 송신
        yield return www.SendWebRequest();

        string imgstr = www.downloadHandler.text; 
        byte[] imgbytes = Convert.FromBase64String(imgstr);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imgbytes);
        /*
         php로부터 전달받은 string은 기존의 이미지가 base64 인코딩 된 값
        => 이를 다시 이미지화 하여 2d texture로 저장
        */

        Renderer img = gameObject.GetComponent<Renderer>();
        img.material.mainTexture = texture; // 텍스처를 3D 모델에 할당
    }
}
