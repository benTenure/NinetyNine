using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    private const string PLAYER_PREFS_NAME_KEY = "PlayerName";

    private void Start()
    {
        SetUpInputField();
    }

    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PLAYER_PREFS_NAME_KEY)) return;

        string defaultName = PlayerPrefs.GetString(PLAYER_PREFS_NAME_KEY);

        nameInputField.text = defaultName;
        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName()
    {
        string playerName = nameInputField.text;
        PhotonNetwork.NickName = playerName;
        PlayerPrefs.SetString(PLAYER_PREFS_NAME_KEY, playerName);
    }
}
