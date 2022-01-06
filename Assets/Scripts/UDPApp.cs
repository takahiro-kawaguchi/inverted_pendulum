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
    public int recievePort;
    public int sendPort;
    public string sendAddress;
    UdpClient udpClient;
    UdpClient sendUdpClient;
    Thread recieveThread;
    IPEndPoint receiveEP;
    IPEndPoint sendEP;
    public Action<string> RecieveAction;
    public Func<string> SendAction;
    bool started = false;

    // Start is called before the first frame update
    public void UDPStart()
    {
        if (!(recieveThread is null))
        {
            recieveThread.Abort();
        }
        if (sendAddress != "")
        {
            receiveEP = new IPEndPoint(IPAddress.Any, recievePort);
            sendEP = new IPEndPoint(IPAddress.Parse(sendAddress), sendPort);
            udpClient = new UdpClient(receiveEP);
            sendUdpClient = new UdpClient();
            recieveThread = new Thread(new ThreadStart(ThreadRecieve));
            recieveThread.Start();
            started = true;
        }
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
        if (started)
        {
            recieveThread.Abort();
        }
    }

}
