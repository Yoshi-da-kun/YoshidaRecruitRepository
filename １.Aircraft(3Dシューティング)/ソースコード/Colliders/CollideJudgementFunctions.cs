
using UnityEngine;

/// --------------------------------------------------
/// #CollideJudgementFunctions.cs
/// �쐬��:�g�c�Y��
/// 
/// �R���C�_�[�̏Փ˔�����Ƃ�֐����܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

namespace CollideJudgement
{
    public static class CollideJudgementFunctions
    {
        /// <summary>
        /// ���R���C�_�[���m���������Ă��邩�𔻒肵�āA�������Ă���I�u�W�F�N�g��Ԃ�
        /// </summary>
        public static OriginalCollider SphereToSphereCollision(OriginalSphereCollider collisionCollider, OriginalSphereCollider receiveCollider)
        {
            // �Փ˂��Ă���R���C�_�[
            OriginalCollider collidingCollider = default;

            // �R���C�_�[���m�̋��������߂�
            float colliderDistance = Vector3.Distance(collisionCollider._thisTransform.position, receiveCollider._thisTransform.position);

            // ���߂��������e���̔��a�𑫂����l��菬����(�R���C�_�[���m���Փ˂��Ă���)�Ƃ�
            if (colliderDistance <= collisionCollider._colliderRadius + receiveCollider._colliderRadius)
            {
                collidingCollider = receiveCollider.GetComponent<OriginalCollider>();
            }

            return collidingCollider;
        }


        /// <summary>
        /// ���R���C�_�[��������(Box)�R���C�_�[�ɓ������Ă��邩�𔻒肵�āA�������Ă���I�u�W�F�N�g��Ԃ�
        /// </summary>
        public static OriginalCollider SphereToBoxCollision(OriginalSphereCollider collisionCollider, OriginalBoxCollider receiveCollider)
        {
            // �Փ˂��Ă���R���C�_�[
            OriginalCollider collidingCollider = default;

            // �Փ˂��鑤�̃R���C�_�[�̒��S���W
            Vector3 collisionColliderPosition = collisionCollider._thisTransform.position;

            // �Փ˂��鑤�̃R���C�_�[�̒��S���W�ɑ΂��āA�����̃R���C�_�[�̒��ōł��߂����W
            Vector3 receiveCollideClosestPosition = collisionColliderPosition;

            // ��Փ�(Box)�R���C�_�[�̍ŏ��A�ő�l�̍��W
            Vector3 maxRcieveCollidePosition = receiveCollider._thisTransform.position + receiveCollider._colliderSize / 2;
            Vector3 minRcieveCollidePosition = receiveCollider._thisTransform.position - receiveCollider._colliderSize / 2;


            ///-- �������v�Z���邽�߂́A������(��Փ�)�R���C�_�[�̒�������W�����߂� --///
            /// ����Փ˃R���C�_�[ �� ��R���C�_

            // ��R���C�_�ŏ����W(x)��蒆�S���W(x)���������Ƃ��A��R���C�_�̍ŏ����W���i�[����
            if (collisionColliderPosition.x < minRcieveCollidePosition.x)
            {
                receiveCollideClosestPosition.x = minRcieveCollidePosition.x;
            }
            // ��R���C�_�ő���W(x)��蒆�S���W(x)���傫���Ƃ��A��R���C�_�̍ő���W���i�[����
            else if (collisionColliderPosition.x > maxRcieveCollidePosition.x)
            {
                receiveCollideClosestPosition.x = maxRcieveCollidePosition.x;
            }
            // ��R���C�_�ŏ����W(y)��蒆�S���W(y)���������Ƃ��A��R���C�_�̍ŏ����W���i�[����
            if (collisionColliderPosition.y < minRcieveCollidePosition.y)
            {
                receiveCollideClosestPosition.y = minRcieveCollidePosition.y;
            }
            // ��R���C�_�ő���W(y)��蒆�S���W(y)���傫���Ƃ��A��R���C�_�̍ő���W���i�[����
            else if (collisionColliderPosition.y > maxRcieveCollidePosition.y)
            {
                receiveCollideClosestPosition.y = maxRcieveCollidePosition.y;
            }
            // ��R���C�_�ŏ����W(z)��蒆�S���W(z)���������Ƃ��A��R���C�_�̍ŏ����W���i�[����
            if (collisionColliderPosition.z < minRcieveCollidePosition.z)
            {
                receiveCollideClosestPosition.z = minRcieveCollidePosition.z;
            }
            // ��R���C�_�ő���W(z)��蒆�S���W(z)���傫���Ƃ��A��R���C�_�̍ő���W���i�[����
            else if (collisionColliderPosition.z > maxRcieveCollidePosition.z)
            {
                receiveCollideClosestPosition.z = maxRcieveCollidePosition.z;
            }

            // �R���C�_�[���m�̋��������߂�
            float colliderDistance = Vector3.Distance(collisionColliderPosition, receiveCollideClosestPosition);

            // ���߂����������a��菬����(�R���C�_�[���m���Փ˂��Ă���)�Ƃ�
            if (colliderDistance <= collisionCollider._colliderRadius)
            {
                // �Փ˂����R���C�_�[���i�[����
                collidingCollider = receiveCollider.GetComponent<OriginalCollider>();
            }

            // �Փ˂����R���C�_�[��Ԃ�
            return collidingCollider;
        }
    }
}

