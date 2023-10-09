using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEventHandler : MonoBehaviour
{
	void Death()
	{
		Destroy(this.transform.parent.gameObject);
	}
}
