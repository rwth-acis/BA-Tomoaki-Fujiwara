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
                // �^�[�Q�b�g�����݂��Ȃ��ꍇ�A�^�X�N�����s�Ƃ��ďI��
                StopAsFailed();
                return TaskState.Failure;
            }

            float distance = Vector3.Distance(agent.transform.position, target.position);

            if (distance > stopDistance) {
                // ��~�����𒴂��Ă���ꍇ�A�^�[�Q�b�g�Ɍ������Ĉړ�
                agent.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(target.position);
            } else {
                // ��~�������ɂ���ꍇ�A�ړ����~
                agent.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();

                // �^�[�Q�b�g�̕����������iy����]�̂݁j
                Vector3 directionToTarget = (target.position - agent.transform.position).normalized;
                directionToTarget.y = 0; // y���̉e����r�����Đ�����]�݂̂��l��
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                agent.transform.rotation = lookRotation; // y����]�̂ݓK�p
            }

            // �^�X�N�͏I�������A�p���I�Ɏ��s
            return TaskState.Running;
        }

        public override void Abort() {
            base.Abort();
            StopAsAborted();
        }
    }
}
