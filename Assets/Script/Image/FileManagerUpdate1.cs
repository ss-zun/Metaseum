using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;
using UnityEditor;

public class FileManagerUpdate1 : MonoBehaviour
{
    public Image img;
    Renderer Img2;
    string imgString;
    string imgnum = "image1";
    byte[] imgByte;

    public static string useruid;


    void Start()
    {
        Img2 = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시 호출
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                 StartCoroutine (LoadImage(useruid, imgnum)); // 클릭 할 때마다 이미지를 새로고침(불러오기)
                if (hit.collider.gameObject.tag == "image") // 이미지에 클릭하면 파일 탐색기 함수 호출
                {
                    print(hit.collider.gameObject.name + "충돌");
                    OpenFileBrowser();
                }
            }

        }
    }

    public void OpenFileBrowser() // 파일 탐색기를 열어 이미지를 선택한 후 db에 저장하는 함수
    {
        var bp = new BrowserProperties();

        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *jtif, *.png | *jpg, *.jpeg, *jpe, *jtif, *png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            imgByte = File.ReadAllBytes(path);
            imgString = Convert.ToBase64String(imgByte);  // 선택한 이미지의 base64 인코딩 값을 저장
        });

        Debug.Log(imgString.Length);

        StartCoroutine (SaveImage(useruid, imgString, imgnum)); // 이미지 저장 함수 호출
        StartCoroutine (LoadImage(useruid, imgnum)); //이미지 불러오기 함수 호출
    }
    

    IEnumerator SaveImage(string useruid, string imgString, string imgnum)
    { // 이미지 저장 php로 uid, 이미지 base64 string, 액자 번호 파라미터를 보내는 함수
        string imgsaveURL = "http://metasium.dothome.co.kr/imgsave.php"; //이미지 저장 php url

        WWWForm form = new WWWForm ();
        form.AddField ("UserUidPost", useruid);
		form.AddField ("ImagePost", imgString);
        form.AddField ("FilePost",imgnum);

        UnityWebRequest www = UnityWebRequest.Post(imgsaveURL, form); 
        //form에 uid, imgstring, 액자 번호 저장해서 php로 파라미터 송신
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
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