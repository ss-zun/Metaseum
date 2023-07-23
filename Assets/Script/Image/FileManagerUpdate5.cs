using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;
using UnityEditor;


public class FileManagerUpdate5 : MonoBehaviour
{
    public Image img;
    Renderer Img2;
    string imgString;
    string imgnum = "image5";
    byte[] imgByte;

    public static string useruid;


    void Start()
    {
        Img2 = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                 StartCoroutine (LoadImage(useruid, imgnum));
                if (hit.collider.gameObject.tag == "image5")
                {
                    print(hit.collider.gameObject.name + "충돌");
                    OpenFileBrowser();
                }
            }

        }
    }

    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();

        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *jtif, *.png | *jpg, *.jpeg, *jpe, *jtif, *png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            imgByte = File.ReadAllBytes(path);
            imgString = Convert.ToBase64String(imgByte);
        });

        Debug.Log(imgString.Length);

        StartCoroutine (SaveImage(useruid, imgString, imgnum));
        StartCoroutine (LoadImage(useruid, imgnum));
    }
    

    IEnumerator SaveImage(string useruid, string imgString, string imgnum)
    {
        string imgsaveURL = "http://metasium.dothome.co.kr/imgsave.php";

        WWWForm form = new WWWForm ();

        form.AddField ("UserUidPost", useruid);
		form.AddField ("ImagePost", imgString);
        form.AddField ("FilePost",imgnum);

        UnityWebRequest www = UnityWebRequest.Post(imgsaveURL, form);

        yield return www.SendWebRequest();

        Debug.Log(www.downloadHandler.text);
    }

    IEnumerator LoadImage(string useruid, string imgnum)
    {
        string imgloadURL = "http://metasium.dothome.co.kr/imgload.php";

        WWWForm form = new WWWForm ();
        form.AddField ("UserUidPost", useruid);
        form.AddField ("FilePost",imgnum);

        UnityWebRequest www = UnityWebRequest.Post(imgloadURL,form);

        yield return www.SendWebRequest();

        string imgstr = www.downloadHandler.text;
        byte[] imgbytes = Convert.FromBase64String(imgstr);

        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imgbytes);

        // 텍스처를 3D 모델에 할당
        
        Renderer img = gameObject.GetComponent<Renderer>();
        img.material.mainTexture = texture;

    }
    
}