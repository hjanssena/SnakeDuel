using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class UDPListener
{
    private UdpClient udpListener;
    private IPEndPoint remoteEndPoint;

    public UDPListener(int port)
    {
        udpListener = new UdpClient(port);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
        Debug.Log("UDP Listener started on port " + port);
    }

    public async void ListenForMessagesAsync()
    {
        while (true)
        {
            UdpReceiveResult result = await udpListener.ReceiveAsync();
            string receivedText = Encoding.ASCII.GetString(result.Buffer);
            Debug.Log("Received: " + receivedText);
        }
    }

    void OnApplicationQuit()
    {
        udpListener.Close();
    }
}