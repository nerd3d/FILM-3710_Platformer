using UnityEngine;
using System.Collections;

public class HealthUI : MonoBehaviour {

    public GameObject player;
    int currentHealth;
	// Use this for initialization
	void Start () {
        currentHealth = 3;
	}
	
	// Update is called once per frame
	void Update () {
        int updatedHealth = player.GetComponent<PlayerController>().getCurrentHealth();
        if (currentHealth != updatedHealth)
        {
            currentHealth = updatedHealth;

            int numChildren = transform.childCount/2;
            //hardcoded to 3 max hp
            for (int i = 0; i < numChildren; ++i)
            {
                if(i<(currentHealth))
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }      
        } 
	}
}
