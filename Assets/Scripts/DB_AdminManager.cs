using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using UnityEngine.SceneManagement;

public class DB_AdminManager : MonoBehaviour
{
    // 관리자 로그인&회원가입 시 사용되는 inputfield
    public InputField Admin_Id, Admin_pw, Admin_email, Admin_code; 
    public GameObject AdminWindow,RegisterFailed,LoginError,RegisterSuccessed;
    // Start is called before the first frame update
    public void AdminLoginButton()
    {
        StartCoroutine (LoginToAdminDB (Admin_Id.text, Admin_pw.text));
    }

    public void AdminRegisterButton() //가입하기 버튼 눌렀을 때
    {
        if (Admin_Id.text=="")
        {
            Debug.Log("아이디를 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        if (Admin_pw.text=="")
        {
            Debug.Log("비밀번호를 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        if (Admin_email.text=="")
        {
            Debug.Log("이메일을 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        if (Admin_code.text=="")
        {
            Debug.Log("key-code를 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        
        StartCoroutine(AdminRegisterToDB(Admin_Id.text, Admin_pw.text, Admin_email.text,Admin_code.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAdmin() //회원가입 창 띄우기
    {
        AdminWindow.SetActive(true);
    }

    IEnumerator LoginToAdminDB(string admin_id, string admin_pw)
    {
        string LoginURL = "http://metasium.dothome.co.kr/adminlogin.php";
		WWWForm form = new WWWForm ();
		form.AddField ("admin_idPost", admin_id);
		form.AddField ("admin_pwPost", admin_pw);

		WWW www = new WWW (LoginURL, form);

		yield return www;

        Debug.Log(www.text);
		
        if (www.text=="admin login success")
        {
            Debug.Log("관리자 로그인 성공");

            // 캐릭터 선택 씬으로 이동
            FileManagerUpdate1.useruid = admin_id;
            FileManagerUpdate2.useruid = admin_id;
            FileManagerUpdate3.useruid = admin_id;
            FileManagerUpdate4.useruid = admin_id;
            FileManagerUpdate5.useruid = admin_id;
            FileManagerUpdate6.useruid = admin_id;
            SceneManager.LoadScene("ExhibitMode");
        }
        else
        {
            LoginError.SetActive(true);
        }

    }

    IEnumerator AdminRegisterToDB(string Admin_Id, string Admin_pw, string Admin_email, string Admin_code) //회원가입 php
	{
        string RegisterURL = "http://metasium.dothome.co.kr/adminregister.php";
		WWWForm form = new WWWForm ();
		form.AddField ("admin_IdPost", Admin_Id);
		form.AddField ("admin_pwPost", Admin_pw);
        form.AddField ("admin_emailPost", Admin_email);
        form.AddField ("admin_codePost", Admin_code);

		WWW www = new WWW (RegisterURL, form);

		yield return www;


        if (www.text=="admin save fail")
        {
            RegisterFailed.SetActive(true);
        }
        else if (www.text=="admin save success")
        {
            RegisterSuccessed.SetActive(true);
        }
		Debug.Log (www.text);
	}


}
