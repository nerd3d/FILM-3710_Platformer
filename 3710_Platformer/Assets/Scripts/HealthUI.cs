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
            for (int i = 0; i < numChildren; i++)
            {
                switch(updatedHealth)
                {
                    case 3:
                        transform.GetChild(5).gameObject.SetActive(true);
                        transform.GetChild(4).gameObject.SetActive(true);
                        transform.GetChild(3).gameObject.SetActive(true);
                        break;
                    case 2:
                        transform.GetChild(5).gameObject.SetActive(false);
                        transform.GetChild(4).gameObject.SetActive(true);
                        transform.GetChild(3).gameObject.SetActive(true);
                        break;
                    case 1:
                        transform.GetChild(5).gameObject.SetActive(false);
                        transform.GetChild(4).gameObject.SetActive(false);
                        transform.GetChild(3).gameObject.SetActive(true);
                        break;
                    case 0:
                        transform.GetChild(5).gameObject.SetActive(false);
                        transform.GetChild(4).gameObject.SetActive(false);
                        transform.GetChild(3).gameObject.SetActive(false);
                        break;
                }
            }      
        } 
	}
}
