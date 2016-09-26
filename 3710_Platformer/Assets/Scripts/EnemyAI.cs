using UnityEngine;
using System.Collections;
using Prime31;

public class EnemyAI : MonoBehaviour {

    private AnimationController2D _animator;
    private int currentHealth = 0;

    public int health = 1;
    public float moveSpeed = 1;


    // Use this for initialization
    void Start ()
    {
        _animator = gameObject.GetComponent<AnimationController2D>();
        currentHealth = health;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
