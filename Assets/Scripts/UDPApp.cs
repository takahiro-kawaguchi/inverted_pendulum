using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Threading;
using System.Text;

public class UDPApp : MonoBehaviour
{
    public int myPort = 22222;
    public int yourPort = 8000;
    public string yourAddress = "127.0.0.1";
    UdpClient udpClient;
    UdpClient sendUdpClient;
    Thread recieveThread;
    IPEndPoint receiveEP;
    IPEndPoint sendEP;
    public Action<string> RecieveAction;
    public Func<string> SendAction;

    // Start is called before the first frame update
    public void UDPStart()
    {
        receiveEP = new IPEndPoint(IPAddress.Any, myPort);
        sendEP = new IPEndPoint(IPAddress.Parse(yourAddress), yourPort);
        udpClient = new UdpClient(receiveEP);
        sendUdpClient = new UdpClient();
        recieveThread = new Thread(new ThreadStart(ThreadRecieve));
        recieveThread.Start();
//        Debug.Log("start");

    }


    void ThreadRecieve()
    {

        while (true)
        {

         
            IPEndPoint senderEP = null;
            byte[] recieveBytes = udpClient.Receive(ref senderEP);
            string msg_recieve = Encoding.ASCII.GetString(recieveBytes);
            
            RecieveAction(msg_recieve);
            string msg_send = SendAction();
//            Debug.Log(msg_send);
            Thread.Sleep(2);
            byte[] message = Encoding.ASCII.GetBytes(msg_send);
            UDPsend(message);
        }
    }

    public void UDPsend(byte[] msg)
    {
        sendUdpClient.SendAsync(msg, msg.Length, sendEP);
    }

    void OnApplicationQuit()
    {
        recieveThread.Abort();
    }

}
