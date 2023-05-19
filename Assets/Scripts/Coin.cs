using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject CoinDeath;
    public Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        if (this.gameObject.scene.isLoaded) Instantiate(CoinDeath, transform.position, transform.rotation);
    }

    void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.transform.name.Contains("Player"))
        {
            manager.noCoinsCollected += 1;

        }
    }


}
