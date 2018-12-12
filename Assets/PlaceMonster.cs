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
public class PlaceMonster : MonoBehaviour
{
    public ArrayList contours;
    public RawImage rawImage;
    private WebCamTexture webCamTexture;
    private Texture2D tex;
    private Mat mat, greyMat;
    public GameObject monsterPrefab;
    private GameObject monster;
    private GameManagerBehavior gameManager;
    private GameObject[] openspots;
    
    
    

    // Use this for initialization
    void Start()
    {
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        webCamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        webCamTexture.Play();
        tex = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
        mat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC4);
        greyMat = new Mat(webCamTexture.height, webCamTexture.width, MatType.CV_8UC1);
    }

    // Update is called once per frame
    void Update()
    {
        if (webCamTexture.didUpdateThisFrame && webCamTexture.isPlaying)
        {

            CamUpdate();

        }

    }
    void CamUpdate()
    {
        openspots = GameObject.FindGameObjectsWithTag("Openspot");
        CvUtil.GetWebCamMat(webCamTexture, ref mat);
        Cv2.CvtColor(mat, greyMat, ColorConversionCodes.RGBA2GRAY);
        var thresh = Cv2.Threshold(greyMat, greyMat, 100, 255, ThresholdTypes.Binary);
        var detectorParams = new SimpleBlobDetector.Params
        {
            FilterByArea = true,
            MinArea = 20, // 10 pixels squared
            MaxArea = 200,

        };
        var simpleBlobDetector = SimpleBlobDetector.Create(detectorParams);
        var keyPoints = simpleBlobDetector.Detect(greyMat);





     //   for (int j = 0; j <= keyPoints.Length; j++)
            
        //{
        
            for (int i = 0; i < openspots.Length; i++)
            {
                 
                
                     if (monster == null)
                     {
                        //3
                         monster = Instantiate(monsterPrefab, openspots[i].transform.localPosition, Quaternion.identity);
                        //4

                        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                        audioSource.PlayOneShot(audioSource.clip);

                        gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;


               }
            
        }


        CvConvert.MatToTexture2D(mat, ref tex);
    }

    private bool CanPlaceMonster()
    { 
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }

    //1
   /* void OnMouseUp()
    {
        CvUtil.GetWebCamMat(webCamTexture, ref mat);
        Cv2.CvtColor(mat, greyMat, ColorConversionCodes.RGBA2BGR);
        var thresh = Cv2.Threshold(greyMat, greyMat, 100, 255, ThresholdTypes.Binary);
        var detectorParams = new SimpleBlobDetector.Params
        {
            FilterByColor = true,
            BlobColor = 130,
        };
        var simpleBlobDetector = SimpleBlobDetector.Create(detectorParams);
        var keyPoints = simpleBlobDetector.Detect(greyMat);

        CvConvert.MatToTexture2D(mat, ref tex);
        rawImage.texture = tex;

        if (CanPlaceMonster())
        {
            //3
            monster = (GameObject) Instantiate(monsterPrefab, transform.position, Quaternion.identity);
            //4
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
        }
       
    }*/



}