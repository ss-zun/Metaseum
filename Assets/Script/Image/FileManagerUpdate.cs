using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using AnotherFileBrowser.Windows;
using Photon.Pun;
using Photon.Realtime;

public class FileManagerUpdate : MonoBehaviour
{
    string path2;

    Renderer Img2;

    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        Img2 = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "image")
                    {
                        print(hit.collider.gameObject.name + "Ãæµ¹");
                        OpenFileBrowser();
                    }
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
            StartCoroutine(LoadImage(path));
        });
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                Img2.material.mainTexture = uwrTexture;

            }
        }
    }
}