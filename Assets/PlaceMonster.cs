/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using OpenCvSharp;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using NWH;
using UnityEngine.UI;
using OpenCvSharp.ML;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;

public class PlaceMonster : MonoBehaviour 
{
    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient client;
    public SocketClient MySocket;
    // public
    // public string IP = "127.0.0.1"; default local
    public int port; // define > init

    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!

    public ArrayList contours;
    private WebCamTexture webCamTexture;
    private Texture2D tex;
    private Mat mat, greyMat;
    public GameObject monsterPrefab;
    private GameObject monster;
    private GameManagerBehavior gameManager;
    private GameObject[] openspots;

    private float xPos;
    private float yPos;
    private string textX;
    private string textY;

    // Use this for initialization
    void Start()
    {
        openspots = GameObject.FindGameObjectsWithTag("Openspot");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        //init();
    }
   /* public void init()
    {
        print("UPDSend.init()");

        port = 5065;

        print("Sending to 127.0.0.1 : " + port);

        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();


    }
    public void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                string[] texts = text.Split(',');
                string textX = texts[0];
                string textY = texts[1];
                print(">> " + text);
                lastReceivedUDPPacket = text;
                allReceivedUDPPackets = allReceivedUDPPackets + text;
                
                
                break;
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
            break;
        }
    }
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }

    public void GetX(String textX)
    {
        xPos = float.Parse(textX, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
    }

    public void GetY(String textY)
    {
        yPos = float.Parse(textY, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
    }
    */

    // Update is called once per frame
    private void UpdateSpots()
    {
        openspots = GameObject.FindGameObjectsWithTag("Openspot");
    }

    void Update()
    {


        //Vector3 nposition;
        // nposition = new Vector3(xPos, yPos);
        //GetX(textX);
        //GetY(textY);


        //for (int i = 0; i < openspots.Length; i++)
        //{
        UpdateSpots();
        if (openspots[0].transform.localPosition.x - 50 <= MySocket.xPos && openspots[0].transform.localPosition.x + 50 >= MySocket.xPos &&
            openspots[0].transform.localPosition.y - 50 <= MySocket.yPos && openspots[0].transform.localPosition.y + 50 >= MySocket.yPos)
        {

            if (CanPlaceMonster())
            {
                //3
                monster = Instantiate(monsterPrefab, openspots[0].transform.localPosition, Quaternion.identity);
                //4

                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);

                gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
                Destroy(openspots[0]);
                

            }
        }
        else if (openspots[1].transform.localPosition.x - 50 <= MySocket.xPos && openspots[1].transform.localPosition.x + 50 >= MySocket.xPos &&
            openspots[1].transform.localPosition.y - 50 <= MySocket.yPos && openspots[1].transform.localPosition.y + 50 >= MySocket.yPos)
        {
             if (CanPlaceMonster())
            {
                //3
                monster = Instantiate(monsterPrefab, openspots[1].transform.localPosition, Quaternion.identity);
                //4

                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);

                gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
                Destroy(openspots[1]);
                monster = null;

            }
        else if (openspots[2].transform.localPosition.x - 50 <= MySocket.xPos && openspots[2].transform.localPosition.x + 50 >= MySocket.xPos &&
                openspots[2].transform.localPosition.y - 50 <= MySocket.yPos && openspots[2].transform.localPosition.y + 50 >= MySocket.yPos)
            {
                if (CanPlaceMonster())
                {
                    //3
                    monster = Instantiate(monsterPrefab, openspots[2].transform.localPosition, Quaternion.identity);
                    //4

                    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                    audioSource.PlayOneShot(audioSource.clip);

                    gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
                    Destroy(openspots[2]);
                    monster = null;
                }
            }
             else
            {
                print("No tower placed");
            }

            //}
        }
    }
 

    private bool CanPlaceMonster()
    { 
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }


    /*void OnApplicationQuit()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
            Debug.Log(receiveThread.IsAlive); //must be false
        }
    } */
    
}