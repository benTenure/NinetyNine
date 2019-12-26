using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace NinetyNine.Menus
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findOpponentPanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;

        private bool isConnecting = false;
        private const string GAME_VERSION = "0.1";
        private const int MAX_PLAYERS_PER_ROOM = 2;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void FindOpponent()
        {
            isConnecting = true;
            findOpponentPanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Searching for opponent...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = GAME_VERSION;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to Master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            waitingStatusPanel.SetActive(false);
            findOpponentPanel.SetActive(true);
            
            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("No clients are waiting for an opponent, creating new room");

            PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = MAX_PLAYERS_PER_ROOM});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Client successfully joined a room");

            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

            if (playerCount != MAX_PLAYERS_PER_ROOM)
            {
                waitingStatusText.text = "Waiting for opponent...";
                Debug.Log("Client is waiting for opponent");
            }
            else
            {
                waitingStatusText.text = "Opponent found";
                Debug.Log("Match is ready to begin");
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MAX_PLAYERS_PER_ROOM)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                Debug.Log("Match is ready to begin");
                waitingStatusText.text = "Opponent found";

                PhotonNetwork.LoadLevel("GameLobby");
            }
        }
    }
}
