using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExhibitController : MonoBehaviour
{
    public float walkSpeed = 20f; // 걸음속도
    public float horizontalSpeed = 100f; // 수평 회전 속도

    public CinemachineVirtualCamera virtualCamera;

    private bool isGround = true;           // 캐릭터가 땅에 있는지 확인할 변수
    private Rigidbody myRigid;
    Vector3 moveVec;
    float _moveDirX;
    float _moveDirZ;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>(); // Rigidbody 속성 가지고 있는 컴포넌트 로드
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirX = Input.GetAxisRaw("Horizontal"); // 좌우 키
        _moveDirZ = Input.GetAxisRaw("Vertical"); // 상하 키
        VerticalMove(); // 캐릭터 움직임
        Rotate(); // 캐릭터 좌우회전
    }

    void OnCollisionEnter(Collision collision)
    {
        // 부딪힌 물체의 태그가 "Ground"라면
        if (collision.gameObject.CompareTag("Ground"))
        {
            // isGround를 true로 변경
            isGround = true;
        }
    }
    void VerticalMove()
    {
        Vector3 _moveVertical = transform.forward * _moveDirZ; // 상하 좌표
        moveVec = _moveVertical.normalized;

        Vector3 _velocity = moveVec * walkSpeed; // 속도

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); // 속도에 따른 위치이동
    }

    void Rotate() // 좌우 캐릭터 회전 - 키보드
    {
        float horizontal = _moveDirX * horizontalSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, Vector3.up, horizontal);
    }
}
