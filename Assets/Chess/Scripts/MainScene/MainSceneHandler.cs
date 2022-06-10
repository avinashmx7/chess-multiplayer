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
    private const string ConnectedMessage = "Connected to server";
    private const string JoiningLobby = "Joining lobby...";
    private const string OnePlayerMissing = "Need 1 more player";
    private const string InvalidName = "Invalid player name";
    private const string InvalidRoomName = "Invalid room name";
    private const string OnReady = "Please start the game.";
    private const string WaitForStartGame = "Please wait until game starts";

    private void Awake() {
        Application.runInBackground = true;
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
        joinRoomButton.onClick.AddListener(JoinLobby);

        statusText.text = ConnectingMessage;
        StartCoroutine(ConnectToServer());
    }

    private IEnumerator ConnectToServer() {
        PhotonNetwork.ConnectUsingSettings(Application.version);
        yield return new WaitUntil(() => PhotonNetwork.connected);
        statusText.text = ConnectedMessage;
        SetUIInteractable(true);
    }

    private void JoinLobby() {
        SetUIInteractable(false);
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

        statusText.text = JoiningLobby;
        StartCoroutine(CreateOrJoinLobby(playerName, roomName));
    }

    private IEnumerator CreateOrJoinLobby(string playerName, string roomName) {
        //Creating or joining lobby
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
}