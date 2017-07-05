using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Event;
using RPG.Utils;

namespace RPG.Characters
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	public class PlayerMovement : MonoBehaviour
	{
		// ******************************** Config ****************************
		[Tooltip("Tuning this value to remove glitch")]
		public float stopVelocity = 2.5f;
		public float minMoveDist = 1f;
		public float moveSpeed = 3.5f;
		public float moveAnimSpeed = 1f;
		public float stoppingDist = 0.1f;
		public ClickMoveConfig clickMoveConfig;

		// ******************************** Objs ****************************
		MouseEvent mouseEvent = null;
		NavMeshAgent navMeshAgent = null;
		Animator anim = null;
		Rigidbody rigibody = null;
		Player player = null;

		// ******************************** Paras ****************************
		Vector3 targetPos;
		float stopDist;
		bool canMove = false;
		bool enemyClicked = false;
		public bool isRunning{get{ return !navMeshAgent.isStopped;} set{ navMeshAgent.isStopped = !value;}}

		// ******************************** Observers ****************************
		public delegate void OnMovementStop();
		public event OnMovementStop onMovementStop;

		// ******************************** Unity Calls ****************************
		void Start()
		{
			targetPos = transform.position;
			mouseEvent = FindObjectOfType<MouseEvent> ();
			navMeshAgent = GetComponent<NavMeshAgent> ();
			anim = GetComponent<Animator> ();
			rigibody = GetComponent<Rigidbody> ();
			player = GetComponent<Player> ();

			navMeshAgent.speed = moveSpeed;
			navMeshAgent.stoppingDistance = stoppingDist;
			navMeshAgent.destination = transform.position;
			anim.speed = moveAnimSpeed;

			mouseEvent.onMouseOverWalkable += OnMouseOverWalkable;
			mouseEvent.onMouseOverEnemy += OnMouseOverEnemy;
			rigibody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}

		void Update()
		{
			RemoveGlitch();
			UpdateMovement (targetPos);
		}

		// ******************************** Callbacks ********************************************
		void OnMouseOverWalkable(Vector3 destination)
		{
			if (Input.GetMouseButtonDown ((int)clickMoveConfig))
			{
				targetPos = destination;
				if ((targetPos - transform.position).magnitude > minMoveDist)
				{
					canMove = true;
					enemyClicked = false;
					stopDist = stoppingDist;
				}
				else {
					canMove = false;
				}
			}
		}
			
		void OnMouseOverEnemy(Enemy enemy)
		{
			if (Input.GetMouseButtonDown((int)clickMoveConfig))
			{
				targetPos = enemy.gameObject.transform.position;
				if (!player.isTargetInRange (targetPos))
				{
					canMove = true;
					enemyClicked = true;
					stopDist = player.getCurrentAttackRange ();
				} else {
					canMove = false;
				}
			}
		}

		// ******************************** AI Move ********************************************
		void UpdateMovement(Vector3 destination)
		{
			if (canMove)
			{
				isRunning = true;
				navMeshAgent.stoppingDistance = stopDist;
				navMeshAgent.destination = destination;
				anim.SetBool (CharacterAnimatorPara.RUN, isRunning);
				canMove = false;
			}
		}

		// TODO make this get called again
		/*void ProcessDirectMovement()
		{
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			// calculate camera relative direction to move:
			Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
			Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

		}*/


		// ******************************** UpdateAnimation ********************************************
		void RemoveGlitch()
		{
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && isRunning)
			{
				if (!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) <= stopVelocity)
				{
					isRunning = false;
					canMove = false;
					anim.SetBool (CharacterAnimatorPara.RUN, isRunning);
					onMovementStop ();
				}
			}
		}

		public void Stop()
		{
			navMeshAgent.isStopped = true;
			anim.SetBool (CharacterAnimatorPara.RUN, false);
		}

		// ******************************** Draw ********************************************
		void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (transform.position, targetPos);
			Gizmos.DrawSphere (targetPos, 0.2f);

			if (navMeshAgent == null)
			{
				return;
			}
			Vector3 reductionVector = (targetPos - transform.position).normalized * navMeshAgent.stoppingDistance;
			Vector3 stopPosition = targetPos - reductionVector;
			Gizmos.DrawSphere (stopPosition, 0.2f);
		}
	}
}
