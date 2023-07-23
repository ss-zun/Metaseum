using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using UnityEngine.SceneManagement;

public class DB_LoginManager : MonoBehaviour // 일반 유저의 로그인, 회원가입 시 사용되는 스크립트 DB_LoginManager
{

    public InputField Id_Input, Password_Input; // 로그인 시 사용되는 inputfield
    public InputField Register_Id, Register_pw, Register_email; // 회원가입 시 사용되는 inputfield
    public GameObject LoginError,RegisterWindow, RegisterFailed, RegisterSuccessed; // 각종 창들
 
    public void LoginButton() // 로그인 버튼 클릭 시 로그인 함수 호출
    {
        StartCoroutine (LoginToDB (Id_Input.text, Password_Input.text));
    }

    void Update () // 엔터를 누를 때도 로그인 함수 호출
	{
		if (Input.GetKeyDown (KeyCode.KeypadEnter))
			StartCoroutine (LoginToDB (Id_Input.text, Password_Input.text));
	}
    
	IEnumerator LoginToDB(string user_Id, string user_pw) //로그인 php로 ID, PW 파라미터를 보내는 함수
	{
        string LoginURL = "http://metasium.dothome.co.kr/login.php";  //로그인 php url
		WWWForm form = new WWWForm ();
		form.AddField ("user_IdPost", user_Id);
		form.AddField ("user_pwPost", user_pw);

		WWW www = new WWW (LoginURL, form);  // form에 id, pw 저장해서 php로 파라미터 송신
		yield return www;

        if (www.text=="login success") // 로그인 php로부터 로그인 성공이 return 된다면
        {
            Debug.Log("로그인 성공");

            // 캐릭터 선택 씬으로 이동
            SceneManager.LoadScene("Select");
        }
        else //만약 로그인에 실패하면 로그인 에러 창을 전시
        {
            LoginError.SetActive(true);
        }
	}

    public void RegisterButton() // 회원가입 버튼 클릭 시
    {
        if (Register_Id.text=="") // id, pw, email이 빈 칸이라면 회원가입 실패 창 전시
        {
            Debug.Log("아이디를 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        if (Register_pw.text=="")
        {
            Debug.Log("비밀번호를 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        if (Register_email.text=="")
        {
            Debug.Log("이메일을 입력하세요!");
            RegisterFailed.SetActive(true);
            return;
        }
        // 모든 칸이 채워져 있다면 회원가입 함수 호출
        StartCoroutine(RegisterToDB(Register_Id.text, Register_pw.text, Register_email.text)); 
    }
    
    IEnumerator RegisterToDB(string Register_Id, string Register_pw, string Register_email) 
	{  // 회원가입 php로 ID, PW, Email 파라미터를 보내는 함수
        string RegisterURL = "http://metasium.dothome.co.kr/register.php"; // 회원가입 php url
		WWWForm form = new WWWForm ();
		form.AddField ("register_IdPost", Register_Id);
		form.AddField ("register_pwPost", Register_pw);
        form.AddField ("register_emailPost", Register_email);

		WWW www = new WWW (RegisterURL, form); // form에 ID, PW, Email 저장해서 php로 파라미터 송신
		yield return www;

        if (www.text=="save fail") // 회원가입 php에서 회원가입에 실패한다면 회원가입 실패 창 전시
            RegisterFailed.SetActive(true);
        else if (www.text=="save success") // 회원가입 php에서 회원가입에 성공한다면 회원가입 성공 창 전시
            RegisterSuccessed.SetActive(true);

		Debug.Log (www.text);
	}

    public void OpenError() //로그인 실패 창 띄우기
    {
        LoginError.SetActive(true);
    }
    public void OpenRegister() //회원가입 창 띄우기
    {
        RegisterWindow.SetActive(true);
    }

}
