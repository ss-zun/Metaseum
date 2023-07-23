


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public float walkSpeed = 20f; // 걸음속도
    public float horizontalSpeed = 100f; // 수평 회전 속도
    private float jumpForce = 5f;  // 점프하는 힘

    public TMP_Text nickNameUi; // 닉네임 UI

    private bool isGround = true;           // 캐릭터가 땅에 있는지 확인할 변수
    private Rigidbody myRigid;
    private CinemachineVirtualCamera virtualCamera;
    private Animator animator;
    private TMP_InputField chatInputField;

    Vector3 moveVec;
    float _moveDirX;
    float _moveDirZ;

    void Awake()
    {
        myRigid = GetComponent<Rigidbody>(); // Rigidbody 속성 가지고 있는 컴포넌트 로드
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        animator = GetComponent<Animator>();
        chatInputField = GameObject.FindObjectOfType<TMP_InputField>();
    }
    void Start() // 시작
    {
        // 닉네임 셋팅
        nickNameUi.text = photonView.Owner.NickName;

        // 자신의 캐릭터일 경우 시네머신 카메라를 연결
        if (photonView.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
            // 닉네임 비활성화(내 닉네임을 나만 안보이게)
            nickNameUi.gameObject.SetActive(false);
        }
    }

    void Update() // 대략 1초에 60프레임 반복
    {
        // 자신의 캐릭터(네트워크 객체)만 컨트롤
        if (photonView.IsMine)
        {
            _moveDirX = Input.GetAxisRaw("Horizontal"); // 좌우 키
            _moveDirZ = Input.GetAxisRaw("Vertical"); // 상하 키
            if (!chatInputField.isFocused)
            {
                VerticalMove(); // 캐릭터 움직임
                Rotate(); // 캐릭터 좌우회전
            }
        }
    }

    // 충돌 함수
    void OnCollisionEnter(Collision collision)
    {
        // 부딪힌 물체의 태그가 "Ground"라면
        if (collision.gameObject.CompareTag("Ground"))
        {
            // isGround를 true로 변경
            isGround = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), false);
        }
    }

    void VerticalMove()
    {
        Vector3 _moveVertical = transform.forward * _moveDirZ; // 상하 좌표
        moveVec = _moveVertical.normalized;

        Vector3 _velocity = moveVec * walkSpeed; // 속도

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // 속도에 따른 위치이동

        animator.SetBool("isWalking", moveVec != Vector3.zero); // Run 애니메이션 true
    }
    void Rotate() // 좌우 캐릭터 회전 - 키보드
    {
        float horizontal = _moveDirX * horizontalSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, Vector3.up, horizontal);
    }
}