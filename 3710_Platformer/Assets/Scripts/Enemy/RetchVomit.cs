using UnityEngine;
using System.Collections;

public class RetchVomit : MonoBehaviour
{
    public float speed = 4;
    private float endPoint;
    private float startPoint;
    private bool isVomit = true;
	// Use this for initialization
	void Start ()
    {
        speed *= Time.deltaTime;
        startPoint = GetComponentInParent<Transform>().position.x;
        endPoint = GetComponentInParent<Retch>().range;
        transform.SetParent(null);
        if (startPoint > this.transform.position.x)//facing left
        {
            endPoint = startPoint - endPoint;
            Vector3 invertScale = transform.localScale;
            invertScale.x *= -1;
            transform.localScale = invertScale;
            speed *= -1;
        }
        else
        {
            endPoint = startPoint + endPoint;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(speed<0)//headed left
        {
            if (transform.position.x < endPoint)
            {
                Destroy(this.gameObject);
            }
        }
        else//headed right
        {
            if (transform.position.x > endPoint)
            {
                Destroy(this.gameObject);
            }
        }
        //move
        Vector3 newPosition = transform.position;
        newPosition.x += speed;
        transform.position = newPosition;
	}
    void OnTriggerStay2D(Collider2D col)
    {
        if (isVomit)//restrict following code to enemy on trigger collission
        {
            if (col.tag == "Player" || col.tag == "PlayerClub") // damage effect
            {
                Destroy(this.gameObject);
            }
        }
    }
}
