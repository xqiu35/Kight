using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using UnityEngine.EventSystems;
using RPG.Utils;
using Merlin.FastHighlight;

namespace RPG.Event
{
	public class MouseEvent : MonoBehaviour {

		[SerializeField] Texture2D walkCursor = null;
		[SerializeField] Texture2D enemyCursor = null;
		[SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

		public delegate void OnMouseOverEnemy(Enemy enemy);
		public event OnMouseOverEnemy onMouseOverEnemy;

		public delegate void OnMouseOverTerrain(Vector3 destination);
		public event OnMouseOverTerrain onMouseOverWalkable;


		float maxRaycastDepth = 100f; // Hard coded value
		Rect currentScrenRect;
		GameObject highlightedObj = null;



		void Update()
		{
			currentScrenRect = new Rect(0, 0, Screen.width, Screen.height);

			// Check if pointer is over an interactable UI element
			if (EventSystem.current.IsPointerOverGameObject())
			{
				// Impliment UI interaction
			}
			else
			{
				PerformRaycasts();
			}
		}

		void PerformRaycasts()
		{
			if (currentScrenRect.Contains(Input.mousePosition))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				// Specify layer priorities below, order matters
				if (RaycastForEnemy(ray)) { return; }
				if (RaycastForWalkable(ray)) { return; }
			}
		}

		bool RaycastForEnemy(Ray ray)
		{
			RaycastHit hitInfo;
			Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
			if (hitInfo.collider == null) {
				return false;
			}
			GameObject gameObjectHit = hitInfo.collider.gameObject;
			Enemy enemyHit = gameObjectHit.GetComponent<Enemy>();
			if (enemyHit && !enemyHit.IsDead)
			{
				if (highlightedObj != gameObjectHit)
				{
					Dehighlight (highlightedObj);
					Highlight (gameObjectHit);
				}
				Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverEnemy(enemyHit);
				return true;
			}
			return false;
		}

		private bool RaycastForWalkable(Ray ray)
		{
			RaycastHit hitInfo;
			LayerMask potentiallyWalkableLayer = 1 << EnvParas.WALKABLE_LAYER;
			bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
			if (potentiallyWalkableHit)
			{
				Dehighlight (highlightedObj);
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverWalkable(hitInfo.point);
				return true;
			}
			return false;
		}

		private void Highlight(GameObject obj)
		{
			Highlighter highlighter = obj.GetComponent<Highlighter> ();
			if (highlighter == null) {
				print ("No highlighter found in " + obj.name);
				return;
			}
			highlightedObj = obj;
			highlighter.HighlightColor = Color.red;
			highlighter.HighlightThickness = 0.20f;
		}

		private void Dehighlight(GameObject obj)
		{
			if (highlightedObj == null) {
				return;
			}

			Highlighter highlighter = obj.GetComponent<Highlighter> ();
			highlighter.HighlightThickness = 0f;
			highlightedObj = null;
		}
	}
}
