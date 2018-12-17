using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTower : MonoBehaviour {



    private GameObject[] openspots;
    public GameObject monsterPrefab;
    private GameObject monster;
    private GameManagerBehavior gameManager;
    public SocketClient mySocket;


    void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        openspots = GameObject.FindGameObjectsWithTag("Openspot");
    }
	
	// Update is called once per frame
	void Update ()
    {

    
            if (gameObject.transform.localPosition.x -25 <= mySocket.xPos && gameObject.transform.localPosition.x + 25 >= mySocket.xPos &&
                gameObject.transform.localPosition.y - 25 <= mySocket.yPos && gameObject.transform.localPosition.y + 25 >= mySocket.yPos)
            {
                if (CanPlaceMonster())
                {

                    //3
                    monster = Instantiate(monsterPrefab, gameObject.transform.localPosition, Quaternion.identity);
                    //4

                    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                    audioSource.PlayOneShot(audioSource.clip);

                    gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
                    Destroy(gameObject);

                }
            }
          
            
           
        
    }
    private bool CanPlaceMonster()
    {
        int cost = monsterPrefab.GetComponent<MonsterData>().levels[0].cost;
        return monster == null && gameManager.Gold >= cost;
    }
    private void OnMouseDown()
    {
        if (CanPlaceMonster())
        {

            //3
            monster = Instantiate(monsterPrefab, gameObject.transform.localPosition, Quaternion.identity);
            //4

            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(audioSource.clip);

            gameManager.Gold -= monster.GetComponent<MonsterData>().CurrentLevel.cost;
           // Destroy(gameObject);

        }
    }

}
