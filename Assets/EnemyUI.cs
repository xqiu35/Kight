using UnityEngine;

// Add a UI Socket transform to your enemy
// Attach this script to the socket
// Link to a canvas prefab
namespace RPG.Characters
{
	public class EnemyUI : MonoBehaviour
	{

		// Works around Unity 5.5's lack of nested prefabs
		[Tooltip("The UI canvas prefab")]
		[SerializeField]
		GameObject enemyCanvasPrefab = null;

		RectTransform t;

		// Use this for initialization 
		void Start()
		{
			Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity,transform);
			t = GetComponentInChildren<RectTransform> ();
		}

		void Update()
		{
			t.rotation = Quaternion.LookRotation (Camera.main.transform.forward);
		}
	}
}