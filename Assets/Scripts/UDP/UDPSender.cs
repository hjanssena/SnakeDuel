using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPSender
{
    private UdpClient udpSender;
    private IPEndPoint remoteEndPoint;

    public UDPSender(string ipAddress, int port)
    {
        udpSender = new UdpClient();
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
    }

    public void SendMessage(string message)
    {
        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
        udpSender.Send(messageBytes, messageBytes.Length, remoteEndPoint);
        Debug.Log("Message sent: " + message);
    }

    public void Close()
    {
        udpSender.Close();
    }
}