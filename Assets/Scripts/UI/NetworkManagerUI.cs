using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private TMPro.TMP_InputField ipField;
    [SerializeField] private TMPro.TMP_InputField portField;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipField.text, 
                (ushort)int.Parse(portField.text)); 
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipField.text,
                (ushort)int.Parse(portField.text));
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipField.text,
                (ushort)int.Parse(portField.text));
            NetworkManager.Singleton.StartClient();
        });
    }
}
