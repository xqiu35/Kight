﻿using System.Collections;
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
		bool isRunning = false;
		Vector3 clickedPoint;
		public bool isStopped{get{ return navMeshAgent.isStopped;} set{ navMeshAgent.isStopped = value;}}

		// ******************************** Unity Calls ****************************
		void Start()
		{
			clickedPoint = transform.position;
			mouseEvent = FindObjectOfType<MouseEvent> ();
			navMeshAgent = GetComponent<NavMeshAgent> ();
			anim = GetComponent<Animator> ();
			rigibody = GetComponent<Rigidbody> ();
			player = GetComponent<Player> ();

			navMeshAgent.speed = moveSpeed;
			navMeshAgent.stoppingDistance = stoppingDist;
			anim.speed = moveAnimSpeed;

			mouseEvent.onMouseOverWalkable += OnMouseOverWalkable;
			mouseEvent.onMouseOverEnemy += OnMouseOverEnemy;
			rigibody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}

		void Update()
		{
			UpdateAnimation();
		}

		// ******************************** Callbacks ********************************************
		void OnMouseOverWalkable(Vector3 destination)
		{
			if (Input.GetMouseButtonDown ((int)clickMoveConfig))
			{
				clickedPoint = destination;
				if ((clickedPoint - transform.position).magnitude > minMoveDist) {
					isRunning = true;
					MoveTo (stoppingDist, destination);
					CharacterAnimatorPara.setWalk (anim, isRunning);
				}

			}
		}
			
		void OnMouseOverEnemy(Enemy enemy)
		{
			if (Input.GetMouseButtonDown((int)clickMoveConfig))
			{
				clickedPoint = enemy.gameObject.transform.position;
				if (!player.isTargetInRange (clickedPoint)) {
					isRunning = true;
					float stopDistance = player.getCurrentAttackRange ();
					CharacterAnimatorPara.setWalk (anim, isRunning);
					MoveTo (stopDistance, clickedPoint);
				}
			}
		}

		// ******************************** AI Move ********************************************
		void MoveTo(float stopDistance, Vector3 destination)
		{
			navMeshAgent.stoppingDistance = stopDistance;
			navMeshAgent.destination = destination;
			navMeshAgent.isStopped = false;
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
		void UpdateAnimation()
		{
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
				if (!navMeshAgent.hasPath || Mathf.Abs (navMeshAgent.velocity.sqrMagnitude) <= stopVelocity)
				{
					navMeshAgent.isStopped = true;
				}
			}
			
			if (navMeshAgent.isStopped)
			{
				isRunning = false;
			}
			else
			{
				isRunning = true;
			}

			isStopped = navMeshAgent.isStopped;
			anim.SetBool (CharacterAnimatorPara.RUN, isRunning);

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
			Gizmos.DrawLine (transform.position, clickedPoint);
			Gizmos.DrawSphere (clickedPoint, 0.2f);

			if (navMeshAgent == null) {
				return;
			}
			Vector3 reductionVector = (clickedPoint - transform.position).normalized * navMeshAgent.stoppingDistance;
			Vector3 stopPosition = clickedPoint - reductionVector;
			Gizmos.DrawSphere (stopPosition, 0.2f);
		}
	}
}