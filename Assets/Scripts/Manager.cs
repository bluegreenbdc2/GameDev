using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public int noCoinsCollected;
    public int noCoins;
    public int playerHealth;
    [SerializeField]
    TextMeshProUGUI cointmp;
    [SerializeField]
    TextMeshProUGUI healthtmp;
    // Start is called before the first frame update
    void Start()
    {
        noCoins = GameObject.Find("Collectibles").transform.childCount;
        noCoinsCollected = 0;
        playerHealth = 8;

        cointmp.text = "Coins: " + noCoinsCollected + "/" + noCoins;
        healthtmp.text = "Health: " + playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        cointmp.text = "Coins: " + noCoinsCollected + "/" + noCoins;
        healthtmp.text = "Health: " + playerHealth;

        if (noCoinsCollected == noCoins)
        {
            SceneManager.LoadScene("YouWin", LoadSceneMode.Single);
        }
        if (playerHealth < 1)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        /*if (Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);

        }*/
    }
}
