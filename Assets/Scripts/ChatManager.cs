using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public GameObject textPrefab; // 텍스트 프리팹
    public ScrollRect scrollRect; // 스크롤 뷰의 ScrollRect 오브젝트
    public RectTransform content; // 스크롤 뷰의 Content 오브젝트
    public TMP_InputField chatInputField; // 채팅 입력창

    private ChatClient chatClient;
    private string chatAppId = "4d4974fd-a026-41c7-b892-892f860f3007";
    private string chatAppVersion = "1.0";
    private string userName = "null";
    private string roomName = "null";
    private string chatChannel = "global"; // 전역 채널 이름
    void Start()
    {
        roomName = DataManager.instance.inputRoomName.text;
        userName = DataManager.instance.inputNickName.text;
        chatChannel = roomName + "_General";

        // 채팅 입력창에 이벤트 리스너 등록
        chatInputField.onSubmit.AddListener(SendChatMessage);
        Connect();
    }
    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInputField.text);
            // 대화 입력창이 포커스 되어있지 않을 때 Enter키를 누르면
            if (chatInputField.isFocused == false)
                chatInputField.ActivateInputField(); // 입력창에 초점 맞추기
        }
    }
    public void SendChatMessage(string message)
    {
        SendMessage(message);
        chatInputField.text = ""; // 입력창 초기화
    }
    void AddMessage(string message)
    {
        GameObject newText = Instantiate(textPrefab, content);
        newText.GetComponent<TMP_Text>().text = message;

        // 새로 생성된 텍스트 오브젝트의 크기를 Content 오브젝트의 크기에 맞게 조절합니다.
        content.sizeDelta += new Vector2(0, newText.GetComponent<RectTransform>().rect.height);

        // 스크롤뷰를 최하단으로 이동합니다.
        content.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;

        // Content의 높이가 ScrollRect의 높이보다 크면, 스크롤바를 맨 아래로 내립니다.
        if (content.sizeDelta.y > scrollRect.GetComponent<RectTransform>().rect.height)
        {
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
    private void UpdateChatUIForRoom(string sender, string message)
    {
        AddMessage(sender + ": " + message);
    }
    public void Connect()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(chatAppId, chatAppVersion, new AuthenticationValues(userName));
    }
    public void OnConnected()
    {
        Debug.Log("Connected to chat server.");
        chatClient.Subscribe(new string[] { chatChannel });
    }
    public void OnDisconnected()
    {
        Debug.Log("Disconnected from chat server.");
    }
    public void OnChatStateChange(ChatState state) { }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        int msgCount = messages.Length;

        if (channelName.StartsWith(roomName)) // 채널 이름이 방 이름으로 시작하는지 확인
        {
            for (int i = 0; i < msgCount; i++)
            {
                // 특정 방에 대한 채팅 UI를 받은 메시지로 업데이트합니다.
                UpdateChatUIForRoom(senders[i], messages[i].ToString());
            }
        }
    }
    public void OnPrivateMessage(string sender, object message, string channelName) { }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to channel: " + channels[0]);
    }
    public void OnUnsubscribed(string[] channels) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
    public new void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        if (chatClient != null && chatClient.CanChat)
        {
            chatClient.PublishMessage(chatChannel, message);
        }

        chatInputField.text = "";
    }
    public void OnSendButtonClick()
    {
        SendChatMessage(chatInputField.text);
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        // Handle debug messages here
        // For example, you can log the debug messages to the Unity console
        Debug.Log("Chat debug return: " + message);
    }
}
