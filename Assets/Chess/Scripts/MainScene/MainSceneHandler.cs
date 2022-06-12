using System.Collections;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainSceneHandler : MonoBehaviour {
    [SerializeField] private TMP_InputField nameInputField, roomNameInputField;
    [SerializeField] private Button joinRoomButton, startGameButton;
    [SerializeField] private TextMeshProUGUI statusText;
    private const string ConnectingMessage = "Connecting to server...";
    private const string ConnectedMessage = "<color=\"green\">Connected to server</color>";
    private const string JoiningRoom = "Joining room...";
    private const string OnePlayerMissing = "Need 1 more player";
    private const string InvalidName = "<color=\"red\">Invalid player name</color>";
    private const string InvalidRoomName = "<color=\"red\">Invalid room name</color>";
    private const string OnReady = "<color=\"green\">Please start the game</color>";
    private const string WaitForStartGame = "<color=\"green\">Waiting for host to start game</color>";

    private void Awake() {
        Application.runInBackground = true;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.logLevel = PhotonLogLevel.Informational;

#if UNITY_EDITOR
        nameInputField.text = "Avinash";
        roomNameInputField.text = "room_xyz";
#else
        nameInputField.text = "Jeba";
        roomNameInputField.text = "room_xyz";
#endif
    }

    private void Start() {
        PlayerPrefs.DeleteAll();
        SetUIInteractable(false);
        startGameButton.gameObject.SetActive(false);
        joinRoomButton.onClick.AddListener(JoinRoom);

        statusText.text = ConnectingMessage;
        StartCoroutine(ConnectToServer());
    }

    private IEnumerator ConnectToServer() {
        PhotonNetwork.ConnectUsingSettings(Application.version);
        yield return new WaitUntil(() => PhotonNetwork.connected);
        statusText.text = ConnectedMessage;
        SetUIInteractable(true);
    }

    private void JoinRoom() {
        var playerName = nameInputField.text;
        var roomName = roomNameInputField.text;

        if (playerName.Trim().Equals("")) {
            statusText.text = InvalidName;
            return;
        }

        if (roomName.Trim().Equals("")) {
            statusText.text = InvalidRoomName;
            return;
        }

        SetUIInteractable(false);
        statusText.text = JoiningRoom;
        StartCoroutine(CreateOrJoinRoom(playerName, roomName));
    }

    private IEnumerator CreateOrJoinRoom(string playerName, string roomName) {
        PhotonNetwork.playerName = playerName;
        var roomOptions = new RoomOptions {MaxPlayers = 2};
        var typedLobby = new TypedLobby(roomName, LobbyType.Default);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
        yield return new WaitUntil(() => PhotonNetwork.inRoom);

        if (PhotonNetwork.isMasterClient) {
            statusText.text = OnePlayerMissing;
        }

        //Wait for second player.
        yield return new WaitUntil(() => PhotonNetwork.room.PlayerCount == 2);

        if (PhotonNetwork.isMasterClient) {
            //Start Game
            statusText.text = OnReady;
            startGameButton.gameObject.SetActive(true);
            startGameButton.onClick.AddListener(StartGame);
        } else {
            statusText.text = WaitForStartGame;
        }
    }

    private void StartGame() {
        PhotonNetwork.LoadLevel(1);
    }

    private void SetUIInteractable(bool value) {
        nameInputField.interactable = value;
        roomNameInputField.interactable = value;
        joinRoomButton.interactable = value;
    }

    private void OnApplicationQuit() {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
    }
}