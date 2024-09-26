using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private GameObject buttonHolder;
    [SerializeField] private TextMeshProUGUI playersCountText;
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private TMP_InputField enterCode;

    private NetworkVariable<int> playersNum = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        hostButton.onClick.AddListener(async () =>
        {
            string relayCode = await Relay.Instance.CreateRelay();
            codeText.text = relayCode;
            buttonHolder.SetActive(false);
            playersCountText.gameObject.SetActive(true);
        });
        clientButton.onClick.AddListener(async () =>
        {
            codeText.text = await Relay.Instance.JoinRelay(enterCode.text);
            buttonHolder.SetActive(false);
            playersCountText.gameObject.SetActive(true);
        });
    }

    private void FixedUpdate()
    {
        playersCountText.text = "Players: " + playersNum.Value.ToString();
        if (!IsServer) return; 
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
