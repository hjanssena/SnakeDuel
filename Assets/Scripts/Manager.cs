using System.Collections;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    UDPSender sender;
    UDPListener listener;

    void Start()
    {
        sender = new UDPSender("127.0.0.1", 4445);
        StartCoroutine(StartListener());
    }

    void Update()
    {
        sender.SendMessage("Bolis");
    }

    IEnumerator StartListener()
    {
        listener = new UDPListener(4444);
        listener.ListenForMessagesAsync();
        yield return null;
    }
}
