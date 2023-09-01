
using System.Collections.Generic;
using UnityEngine;
using ColliderDataCollection;
using CollideJudgement;

/// --------------------------------------------------
/// #CollisionProcessing.cs
/// �쐬��:�g�c�Y��
/// 
/// �R���C�_�[�̏Փˌ��m��A�߂荞�܂Ȃ����ߏ������s���X�N���v�g
/// --------------------------------------------------

namespace CollisionSystem
{
    public static class CollisionProcessing
    {
        /// <summary>
        /// �e�̓����蔻��Ƃ̏Փˏ������s��
        /// </summary>
        public static List<OriginalCollider> BulletCollision(OriginalCollider collisionCollider)
        {
            // �Փ˂��Ă���R���C�_�[��List
            List<OriginalCollider> collidingObjects = new List<OriginalCollider>();

            // �e�̓����蔻������R���C�_�[���ׂĂ��擾
            ColliderDatas.ColliderLists bulletColliders = ColliderDatas.GetBulletColliderList();

            // �Փ˂��Ă��邩�����m���āA�Փ˂����I�u�W�F�N�g���i�[����
            collidingObjects = CollisionCheck(collisionCollider, bulletColliders);

            // �Փ˂����I�u�W�F�N�g��Ԃ�
            return collidingObjects;
        }


        /// <summary>
        /// ���ړI�ȓ����蔻��Ƃ̏Փˏ������s��
        /// </summary>
        /// <param name="collisionCollider"> �I�u�W�F�N�g�̃R���C�_�[ </param>
        /// <param name="collisionTransform"> �I�u�W�F�N�g��Transform </param>
        /// <param name="movementDistance"> �ړ��� </param>
        public static List<OriginalCollider> PhysicsCollision(OriginalCollider collisionCollider, Transform collisionTransform, Vector3 movementDistance)
        {
            // �Փˌ��m���s��Sphere�R���C�_�[
            OriginalSphereCollider collisionSphereCollider = collisionCollider.GetComponent<OriginalSphereCollider>();
            
            // �Փ˂��Ă���R���C�_�[��List
            List<OriginalCollider> collidingColliders = new List<OriginalCollider>();

            // �e�̓����蔻������R���C�_�[���ׂĂ��擾
            ColliderDatas.ColliderLists physicsColliders = ColliderDatas.GetPhysicsColliderList();

            // �R���C�_�[�̌��ݍ��W
            Vector3 currentCollisionPosition = collisionTransform.position;
            Vector3 destinationCollisionPosition = collisionTransform.position + movementDistance;

            // �ړ��\����W�Ɉړ����鏈��
            collisionTransform.position = destinationCollisionPosition;

            // �Փ˂��Ă��邩�����m���āA�Փ˂����I�u�W�F�N�g���i�[����
            collidingColliders = CollisionCheck(collisionCollider, physicsColliders);

            // �I�u�W�F�N�g���Փ˂��Ă��Ȃ���Έȍ~�̏������s��Ȃ�
            if (collidingColliders.Count == 0)
            {
                // �Փ˂����I�u�W�F�N�g��Ԃ�
                return collidingColliders;
            }

            ///-- �Փ˂����Ƃ��ɁA�߂荞�܂Ȃ��悤�ɂ��鏈�� --///

            // �������Ă���R���C�_�[���J��Ԃ�
            for (int i = 0; i < collidingColliders.Count; i++)
            {
                // �Փ˂����R���C�_�[�̌`��ɉ���������
                switch (collidingColliders[i]._colliderShape)
                {
                    // ��(Sphere)�̂Ƃ�
                    case OriginalCollider._typeOfColliderShape.Sphere:

                        // �ړ��O�̈ʒu�ɖ߂�(���u���A���P��)
                        collidingColliders[i].transform.position = currentCollisionPosition;

                        break;

                    // ������(Box)�̂Ƃ�
                    case OriginalCollider._typeOfColliderShape.Box:

                        // �Փ˂����R���C�_�[�̍��W���i�[����
                        Vector3 collidingColliderPosition = collidingColliders[i].transform.position;
 
                        OriginalBoxCollider collidingBoxCollider = collidingColliders[i].GetComponent<OriginalBoxCollider>();

                        // �Փ˂����R���C�_�[�̒��_�̍��W
                        Vector3 collidingColliderMinPosition = collidingColliderPosition - collidingBoxCollider._colliderSize / 2;
                        Vector3 collidingColliderMaxPosition = collidingColliderPosition + collidingBoxCollider._colliderSize / 2;


                        ///-- �ړ������ɉ����ăR���C�_�[�̂߂荞�񂾋��������߂� --///

                        // �߂荞��ł��鋗��
                        Vector3 SinkIntoDistance = new Vector3(0, 0, 0);

                        // �R���C�_�[�̂߂荞�񂾋��������߂鏈��
                        // x���̐������Ɉړ����A�R���C�_�[�ɖ��܂��Ă���Ƃ�
                        if (movementDistance.x > 0 && currentCollisionPosition.x + collisionSphereCollider._colliderRadius >= collidingColliderMinPosition.x)
                        {
                            SinkIntoDistance.x = Mathf.Abs(currentCollisionPosition.x + collisionSphereCollider._colliderRadius - collidingColliderMinPosition.x);
                        }
                        // x���̕������Ɉړ����A�R���C�_�[�ɖ��܂��Ă���Ƃ�
                        else if (movementDistance.x < 0 && currentCollisionPosition.x - collisionSphereCollider._colliderRadius <= collidingColliderMaxPosition.x)
                        {
                            SinkIntoDistance.x = Mathf.Abs(collidingColliderMaxPosition.x - currentCollisionPosition.x - collisionSphereCollider._colliderRadius);
                        }
                        // y���̐������Ɉړ����A�R���C�_�[�ɖ��܂��Ă���Ƃ�
                        if (movementDistance.y > 0 && currentCollisionPosition.y + collisionSphereCollider._colliderRadius >= collidingColliderMinPosition.y)
                        {
                            SinkIntoDistance.y = Mathf.Abs(currentCollisionPosition.y + collisionSphereCollider._colliderRadius - collidingColliderMinPosition.y);
                        }
                        // y���̕������Ɉړ����A�R���C�_�[�ɖ��܂��Ă���Ƃ�
                        else if (movementDistance.y < 0 && currentCollisionPosition.y - collisionSphereCollider._colliderRadius <= collidingColliderMaxPosition.y)
                        {
                            SinkIntoDistance.y = Mathf.Abs(collidingColliderMaxPosition.y - currentCollisionPosition.y - collisionSphereCollider._colliderRadius);
                        }
                        // z���̐������Ɉړ����A�R���C�_�[�ɖ��܂��Ă���Ƃ�
                        if (movementDistance.z > 0 && currentCollisionPosition.z + collisionSphereCollider._colliderRadius >= collidingColliderMinPosition.z)
                        {
                            SinkIntoDistance.z = Mathf.Abs(currentCollisionPosition.z + collisionSphereCollider._colliderRadius - collidingColliderMinPosition.z);
                        }
                        // z���̕������Ɉړ����A�R���C�_�[�ɖ��܂��Ă���Ƃ�
                        else if (movementDistance.z < 0 && currentCollisionPosition.z - collisionSphereCollider._colliderRadius <= collidingColliderMaxPosition.z)
                        {
                            SinkIntoDistance.z = Mathf.Abs(collidingColliderMaxPosition.z - currentCollisionPosition.z - collisionSphereCollider._colliderRadius);
                        }


                        ///-- �߂荞�܂Ȃ����W�ɏC�����鏈�����s�� --///

                        // x�����ł��߂荞��ł��Ȃ��Ƃ�
                        if (SinkIntoDistance.x < SinkIntoDistance.y && SinkIntoDistance.x < SinkIntoDistance.z)
                        {
                            // x���̐������Ɉړ����Ă�����A�R���C�_�[�̍ŏ����W�ɂ߂荞�܂Ȃ��悤�ɏC��
                            if (movementDistance.x > 0)
                            {
                                destinationCollisionPosition.x = collidingColliderMinPosition.x - collisionSphereCollider._colliderRadius;
                            }
                            // x���̕������Ɉړ����Ă�����A�R���C�_�[�̍ő���W�ɂ߂荞�܂Ȃ��悤�ɏC��
                            else if (movementDistance.x < 0)
                            {
                                destinationCollisionPosition.x = collidingColliderMaxPosition.x + collisionSphereCollider._colliderRadius;
                            }
                        }
                        // y�����ł��߂荞��ł��Ȃ��Ƃ�
                        else if (SinkIntoDistance.y < SinkIntoDistance.z)
                        {
                            // y���̐������Ɉړ����Ă�����A�R���C�_�[�̍ŏ����W�ɂ߂荞�܂Ȃ��悤�ɏC��
                            if (movementDistance.y > 0)
                            {
                                destinationCollisionPosition.y = collidingColliderMinPosition.y - collisionSphereCollider._colliderRadius;
                            }
                            // y���̕������Ɉړ����Ă�����A�R���C�_�[�̍ő���W�ɂ߂荞�܂Ȃ��悤�ɏC��
                            else if (movementDistance.y < 0)
                            {
                                destinationCollisionPosition.y = collidingColliderMaxPosition.y + collisionSphereCollider._colliderRadius;
                            }
                        }
                        // z�����ł��߂荞��ł��Ȃ��Ƃ�
                        else
                        {
                            // z���̐������Ɉړ����Ă�����A�R���C�_�[�̍ŏ����W�ɂ߂荞�܂Ȃ��悤�ɏC��
                            if (movementDistance.z > 0)
                            {
                                destinationCollisionPosition.z = collidingColliderMinPosition.z - collisionSphereCollider._colliderRadius;
                            }
                            // z���̕������Ɉړ����Ă�����A�R���C�_�[�̍ő���W�ɂ߂荞�܂Ȃ��悤�ɏC��
                            else if (movementDistance.z < 0)
                            {
                                destinationCollisionPosition.z = collidingColliderMaxPosition.z + collisionSphereCollider._colliderRadius;
                            }
                        }

                        // �R���C�_�[�̍��W���X�V����
                        collisionTransform.position = destinationCollisionPosition;

                        break;
                }
            }
            // �Փ˂����I�u�W�F�N�g��Ԃ�
            return collidingColliders;
        }


        /// <summary>
        /// �Փ˂��Ă��邩�����m���āA�Փ˂����I�u�W�F�N�g��Ԃ�
        /// </summary>
        /// <param name="collisionCollider"> �Փ˂��Ă��邩�����m����R���C�_�[ </param>
        /// <param name="recieveColliderList"> ��Փˑ��̃R���C�_�[�̃��X�g </param>
        private static List<OriginalCollider> CollisionCheck(OriginalCollider collisionCollider, ColliderDatas.ColliderLists recieveColliderList)
        {
            // �Փ˂��Ă���R���C�_�[��List
            List<OriginalCollider> collidingColliders = new List<OriginalCollider>();

            // �e�R���C�_�[�̌`��ɉ������������s��
            switch (collisionCollider._colliderShape)
            {
                // �Փˌ��m����R���C�_�[����(Sphere)�̃R���C�_�[�̂Ƃ�
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // �Փˌ��m���鋅(Sphere)�R���C�_�[���擾����
                    OriginalSphereCollider collisionSphereCollider = collisionCollider.GetComponent<OriginalSphereCollider>();

                    // ������(Box)�R���C�_�[�ƏՓ˂��Ă��邩�����m����
                    for (int i = 0; i < recieveColliderList.boxList.Count; i++)
                    {
                        // ��Փˑ��̃R���C�_�[��List��null�Ȃ玟�̗v�f��
                        if (!recieveColliderList.boxList[i])
                        {
                            continue;
                        }

                        // �Փ˂����I�u�W�F�N�g���擾
                        OriginalCollider collidingCollider = CollideJudgementFunctions.SphereToBoxCollision(collisionSphereCollider, recieveColliderList.boxList[i]);

                        // �Փ˂��Ă���I�u�W�F�N�g���Ȃ������玟�̗v�f��
                        if (!collidingCollider)
                        {
                            continue;
                        }

                        // �Փ˂����I�u�W�F�N�g���i�[����
                        collidingColliders.Add(collidingCollider);
                    }

                    // ��(Sphere)�R���C�_�[�ƏՓ˂��Ă��邩�����m����
                    for (int i = 0; i < recieveColliderList.sphereList.Count; i++)
                    {
                        // ��Փˑ��̃R���C�_�[��List��null�Ȃ玟�̗v�f��
                        if (!recieveColliderList.sphereList[i])
                        {
                            continue;
                        }

                        // �Փ˂����I�u�W�F�N�g���擾
                        OriginalCollider collidingCollider = CollideJudgementFunctions.SphereToSphereCollision(collisionSphereCollider, recieveColliderList.sphereList[i]);

                        // �Փ˂��Ă���I�u�W�F�N�g���Ȃ������玟�̗v�f��
                        if (!collidingCollider)
                        {
                            continue;
                        }

                        // �Փ˂����I�u�W�F�N�g���i�[����
                        collidingColliders.Add(collidingCollider);
                    }

                    break;


                // ������(Sphere)�̃R���C�_�[�̂Ƃ�(������:Box�`��̂��̂𓮂����Ă��Ȃ�����)
                //case OriginalCollider._typeOfColliderShape.Box:
                //
                //    break;
            }

            // �Փ˂����I�u�W�F�N�g��Ԃ�
            return collidingColliders;
        }
    }
}
