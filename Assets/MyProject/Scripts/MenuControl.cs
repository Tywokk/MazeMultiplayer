using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private TMP_Text ipAddressText;
    [SerializeField] private string lobbySceneName = "GameLobby";

    public void StartGame()
    {
        var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (utpTransport)
        {
            utpTransport.SetConnectionData(Sanitaze(ipAddressText.text), 7777);
        }
        if (NetworkManager.Singleton.StartHost())
        {
            SceneTransitionHandler.sceneTransitionHandler.RegisterCallbacks();
            SceneTransitionHandler.sceneTransitionHandler.SwitchScene(lobbySceneName);
        }
        else
        {
            Debug.LogError("Failed to start host");
        }
    }

    public void JoinGame()
    {
        var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (utpTransport)
        {
            utpTransport.SetConnectionData(Sanitaze(ipAddressText.text), 7777);
        }
        if (!NetworkManager.Singleton.StartClient())
        {
            Debug.LogError("Failed to start client");
        }
    }

    static string Sanitaze(string dirtyString)
    {
        return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
    }
}
