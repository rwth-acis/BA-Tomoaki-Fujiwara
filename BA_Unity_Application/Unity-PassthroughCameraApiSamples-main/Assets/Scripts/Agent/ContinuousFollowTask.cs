using UnityEngine;
using i5.VirtualAgents.AgentTasks;
using i5.VirtualAgents.ScheduleBasedExecution;


namespace i5.VirtualAgents.Examples {
    public class ContinuousFollowTask : AgentBaseTask {
        private Transform target; // target to chase
        private float stopDistance; // Distance to stop
        private Agent agent; // Agent

        public ContinuousFollowTask(Transform target, float stopDistance) {
            this.target = target;
            this.stopDistance = stopDistance;
        }

        public override void StartExecution(Agent executingAgent) {
            base.StartExecution(executingAgent);
            agent = executingAgent;
        }

        public override TaskState EvaluateTaskState() {
            if (target == null) {
                // ターゲットが存在しない場合、タスクを失敗として終了
                StopAsFailed();
                return TaskState.Failure;
            }

            float distance = Vector3.Distance(agent.transform.position, target.position);

            if (distance > stopDistance) {
                // 停止距離を超えている場合、ターゲットに向かって移動
                agent.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(target.position);
            } else {
                // 停止距離内にいる場合、移動を停止
                agent.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();

                // ターゲットの方向を向く（y軸回転のみ）
                Vector3 directionToTarget = (target.position - agent.transform.position).normalized;
                directionToTarget.y = 0; // y軸の影響を排除して水平回転のみを考慮
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                agent.transform.rotation = lookRotation; // y軸回転のみ適用
            }

            // タスクは終了せず、継続的に実行
            return TaskState.Running;
        }

        public override void Abort() {
            base.Abort();
            StopAsAborted();
        }
    }
}
