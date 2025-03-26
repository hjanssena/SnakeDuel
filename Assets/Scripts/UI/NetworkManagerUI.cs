using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private InputField ipLabel;
    [SerializeField] private InputField portLabel;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipLabel.text, 
                (ushort)int.Parse(portLabel.text)); 
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipLabel.text,
                (ushort)int.Parse(portLabel.text));
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipLabel.text,
                (ushort)int.Parse(portLabel.text));
            NetworkManager.Singleton.StartClient();
        });
    }
}
