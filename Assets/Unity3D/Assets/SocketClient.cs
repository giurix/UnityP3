using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;

public class SocketClient : MonoBehaviour {

    // Use this for initialization

    public GameObject hero;
    public float xPos = 10.0f;
    public float yPos = 10.0f;

	Thread receiveThread;
	UdpClient client;
	public int port;

	//info

	public string lastReceivedUDPPacket = "";
	public string allReceivedUDPPackets = "";

	void Start () {
        init();
	}

	private void init(){
		print ("UPDSend.init()");

		port = 5065;

		print ("Sending to 127.0.0.1 : " + port);

		receiveThread = new Thread (new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start ();


	}

	public void ReceiveData(){
		client = new UdpClient (port);
		while (true) {
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
				byte[] data = client.Receive(ref anyIP);
                
				string text = Encoding.UTF8.GetString(data);
                string[] texts = text.Split(',');
                string textX = texts[0];
                string textY = texts[1];
                print (">> " + text);
				lastReceivedUDPPacket=text;
				allReceivedUDPPackets=allReceivedUDPPackets+text;
				xPos = float.Parse(textX, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                yPos = float.Parse(textY, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            }
            catch(Exception e){
				print (e.ToString());
			}
		}
	}

	public string getLatestUDPPacket(){
		allReceivedUDPPackets = "";
		return lastReceivedUDPPacket;
	}
	
	// Update is called once per frame
	void Update () {
        
    }

	void OnApplicationQuit(){
		if (receiveThread != null) {
			receiveThread.Abort();
			Debug.Log(receiveThread.IsAlive); //must be false
		}
	}
}
