using System.Collections;
using i5.VirtualAgents.ScheduleBasedExecution;
using System.Collections.Generic;
using i5.VirtualAgents.AgentTasks;
using UnityEngine;

namespace i5.VirtualAgents.Examples {
    public class ModifyAgentNavigationController : SampleScheduleController {
        /// <summary>
        /// List of waypoints which the agent should visit in order.
        /// </summary>
        [Tooltip("List of waypoints which the agent should visit in order.")]
        public List<Transform> waypoints;

        /// <summary>
        /// Waypoint with a high priority
        /// </summary>
        [Tooltip("Waypoint with a high priority")]
        public Transform highPrioWaypoint;
        [SerializeField] private GameObject chaseTarget;

        //[SerializeField] private Transform target2;

        public UnityEngine.AI.NavMeshAgent navMeshAgent;

        private AgentBaseTask removeTask;

        private AgentBaseTask followTask;

        // Moving goal
        public Transform waypoint;


        // PickUp and Drop Item
        public GameObject pickUpItem1;
        public Transform dropItemPoint;


        /// <summary>
		/// The time in seconds the agent should aim at the target.
		/// </summary>
		[Tooltip("The time in seconds the agent should aim at the target.")]
        [SerializeField] private int aimAtTime = 5;
        // Pointing target
        public GameObject pointingTarget;

        // Rotate to target
        //public Transform rotateTarget;


        // User Position
        public GameObject user;

        protected override void Start() {
            base.Start();
            /*
            // add walking tasks for each waypoint
            // here, we use the TaskActions shortcut but we could also just create a new
            // AgentMovementTask and schedule it using agent.ScheduleTask.
            for (int i = 0; i < waypoints.Count-1; i++)
            {
                taskSystem.Tasks.GoTo(waypoints[i].position);
            }
            // this task will never be executed, as we remove it before it is started
            removeTask = taskSystem.Tasks.GoTo(waypoints[^1].position);

            // example for a different priority:
            // this waypoint is added last but has the highest priority,
            // so the agent will walk to it first
            taskSystem.Tasks.GoTo(highPrioWaypoint, Vector3.zero, 5);

            // we can retroactively remove tasks from the queue
            // tasks that are already running will be aborted
            // to remove all tasks one can use taskSystem.Clear()
            taskSystem.RemoveTask(removeTask);
            */

            //taskSystem.Tasks.GoTo(chaseTarget,new Vector3(0.2f,0.2f,0.2f),0,true);
            //  Ǐ] ^ X N  X P W   [  
            followTask = new ContinuousFollowTask(chaseTarget.transform, stopDistance: 2.0f);
            //taskSystem.ScheduleTask(followTask, priority: 0);

        }

        public void testfunction() {
            removeTask = taskSystem.Tasks.GoTo(new Vector3(0, 0, 0));
        }

        public void GoToWaypoint() {
            // Go to the specified waypoint
            taskSystem.Tasks.GoTo(waypoint.position);
            //taskSystem.Tasks.GoTo(waypoint.position);
        }

        public void stopthetask() {
            taskSystem.RemoveTask(removeTask);
        }

        public void GoPickUpItem() {
            // Go to the item and pick it up
            taskSystem.Tasks.GoToAndPickUp(pickUpItem1);
        }

        public void DropPickUpedItem()
        {
            // Go to the item and pick it up
            taskSystem.Tasks.GoToAndDropItem(dropItemPoint);
        }

        
        public void CarryItemToUser() {

            taskSystem.Tasks.GoToAndPickUp(pickUpItem1);

            float distanceInFront = 0.8f;
            Vector3 dropPosition = user.transform.position + user.transform.forward * distanceInFront;

            taskSystem.Tasks.GoToAndDropItem(dropPosition, pickUpItem1);
        }

        /*
        /// <summary>
        /// This is the function for function calling by LLMs.
        /// </summary>
        /// <param name="target"></param>
        public void CarryTargetItemToUser(GameObject target)
        {

            taskSystem.Tasks.GoToAndPickUp(target);

            float distanceInFront = 0.8f;
            Vector3 dropPosition = user.transform.position + user.transform.forward * distanceInFront;

            taskSystem.Tasks.GoToAndDropItem(dropPosition, pickUpItem1);
        }
        */
        /// <summary>
        /// This is the function for function calling by LLMs.
        /// </summary>
        /// <param name="target"></param>
        public void CarryTargetItemToUser(GameObject target)
        {

            taskSystem.Tasks.GoTo(target.transform.position);


            CustomAgentPickUpTask pickUpTask = new CustomAgentPickUpTask(target);
            pickUpTask.maxPickupDistance = 2.0f; 
            taskSystem.ScheduleTask(pickUpTask);


            float distanceInFront = 0.8f;
            Vector3 dropPosition = user.transform.position + user.transform.forward * distanceInFront;
            taskSystem.Tasks.GoToAndDropItem(dropPosition);
        }

        

        /// <summary>
        /// This is the function for function calling by LLMs.
        /// </summary>
        /// <param name="pointingTargetPosition"></param>
        public void PointVirtualAgentArmToTarget(GameObject pointingTarget) {
            // Rotate the agent to the specified target
            AgentRotationTask rotationTarget = new AgentRotationTask(pointingTarget);
            taskSystem.ScheduleTask(rotationTarget, 0, "Base Layer");
            // Point the arm to the specified target
            AgentAnimationTask pointingLeft = new AgentAnimationTask("PointingLeft", aimAtTime, "", "Left Arm", pointingTarget);
            taskSystem.ScheduleTask(pointingLeft, 0, "Left Arm");
        }

        /// <summary>  This is test function
        public void PointToTarget() {
            RotateToTarget();
            // Point to the specified target
            AgentAnimationTask pointingLeft = new AgentAnimationTask("PointingLeft", aimAtTime, "", "Left Arm", pointingTarget);
            taskSystem.ScheduleTask(pointingLeft, 0, "Left Arm");
        }

        /// <summary>
        /// This is test function
        /// </summary>
        public void RotateToTarget() {
            // Rotate to the specified target
            AgentRotationTask rotationTarget = new AgentRotationTask(pointingTarget);
            //AgentRotationTask rotationTarget = new AgentRotationTask(rotateTarget);
            taskSystem.ScheduleTask(rotationTarget, 0, "Base Layer");
        }

        public void StartFollowing() {
            //var agent = GetComponent<Agent>();

            
            if (navMeshAgent != null && !navMeshAgent.enabled) {
                navMeshAgent.enabled = true;
                //Debug.Log("NavMeshAgent enabled.");
            }

            //followTask = new ContinuousFollowTask(chaseTarget.transform, stopDistance: 2.0f);
            taskSystem.ScheduleTask(followTask, priority: 0);
        }

        public void StopFollowing() {
            taskSystem.RemoveTask(followTask);

        }

        public void FollowUserSetting(bool flag) {
            if (flag) {
                taskSystem.ScheduleTask(followTask);
            } else {
                taskSystem.RemoveTask(followTask);
            }
        }
    }
}