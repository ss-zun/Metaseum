using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0f"; // 버전

    public GameObject[] playerPrefab; // 플레이어 프리팹
    private GameObject player;
    private string playerName;
    private string roomName;

    void Start()
    {
        player = playerPrefab[(int)DataManager.instance.currentCharacter];
        playerName = DataManager.instance.inputNickName.text;
        roomName = DataManager.instance.inputRoomName.text;

        // 유저 아이디 할당
        PhotonNetwork.NickName = playerName;

        // 같은 룸의 유저들에게 자동으로 씬을 로딩
        PhotonNetwork.AutomaticallySyncScene = true;

        // 같은 버전의 유저끼리 접속 허용
        PhotonNetwork.GameVersion = version;

        // Photon 서버에 연결합니다.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // 마스터 서버에 연결되면 룸을 생성하거나 조인합니다.
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 룸에 접속한 사용자 정보 확인
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
            // $ => String.Format() : ""안에있는걸 문자열로 반환해줘라.
        }

        // 캐릭터 출현 정보를 배열에 저장
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        // 캐릭터를 생성
        PhotonNetwork.Instantiate(player.name, points[idx].position, points[idx].rotation, 0);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
}

