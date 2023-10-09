using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float timeToLive = 3.0f;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        
        if(timeToLive <= 0)
		{
            Destroy(this.gameObject);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("GROUND"))
        {
            Destroy(this.gameObject);
        }
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.layer == LayerMask.NameToLayer("GROUND"))
        {
            Destroy(this.gameObject);
        }

        if (collision.collider.CompareTag("GROUND"))
        {
            Destroy(this.gameObject);
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
        if( collision.gameObject.layer == LayerMask.NameToLayer("GROUND"))
		{
            Destroy(this.gameObject);
		}

		if (collision.collider.CompareTag("GROUND"))
		{
            Destroy(this.gameObject);
        }
	}

}


