
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #ColliderDatas.cs
/// �쐬��:�g�c�Y��
/// 
/// �S�ẴR���C�_�[�����X�g�ɂ��X�N���v�g
/// �e���\�b�h�Ń��X�g�ɒǉ��A�폜��Ǘ����s��
/// --------------------------------------------------

namespace ColliderDataCollection
{
    public static class ColliderDatas
    {
        private const int OUT_OF_RANGE_LIST_NUMBER = -1;


        // �e�ɑ΂��铖���蔻��������R���C�_�[�̃��X�g
        private static List<OriginalSphereCollider> _bulletSphereColliders = new List<OriginalSphereCollider>();
        // ���ړI�ȏՓ˔���������R���C�_�[�̃��X�g
        private static List<OriginalSphereCollider> _physicsSphereColliders = new List<OriginalSphereCollider>();

        // �e�ɑ΂��铖���蔻����������̃R���C�_�[�̃��X�g
        private static List<OriginalBoxCollider> _bulletBoxColliders = new List<OriginalBoxCollider>();
        // ���ړI�ȏՓ˔�����������̃R���C�_�[�̃��X�g
        private static List<OriginalBoxCollider> _physicsBoxColliders = new List<OriginalBoxCollider>();


        #region �R���C�_�[�����X�g�ɒǉ����鏈��


        /// <summary>
        /// �e�ɑ΂��铖���蔻�������Collider���i�[���鏈��
        /// </summary>
        public static int AddBulletColliders(OriginalCollider colliderToAdd)
        {
            // �e�R���C�_�[�̌`��ɉ������������s��
            switch (colliderToAdd._colliderShape)
            {
                // ��(Sphere)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // SphereCollieder��List�ɒǉ����鏈��
                    (List<OriginalSphereCollider>, int) addedSphereList = AddSphereCollider(_bulletSphereColliders, colliderToAdd);

                    // SphereCollider�ǉ���̃��X�g���i�[
                    _bulletSphereColliders = addedSphereList.Item1;

                    // SphereCollider���i�[���ꂽ�v�f�ԍ���Ԃ�
                    return addedSphereList.Item2;


                // ������(Box)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Box:

                    // BoxCollieder��List�ɒǉ����A����List��Collider�̊i�[�ԍ����擾����
                    (List<OriginalBoxCollider>, int) addedBoxList = AddBoxCollider(_bulletBoxColliders, colliderToAdd);

                    // BoxCollider�ǉ���̃��X�g���i�[
                    _bulletBoxColliders = addedBoxList.Item1;

                    // BoxCollider���i�[���ꂽ�v�f�ԍ���Ԃ�
                    return addedBoxList.Item2;
            }

            // ��O�̏���(��΂ɗ��Ȃ�)
            Debug.LogError("�e��ColliderList�ɃR���C�_�[���ǉ��ł��܂���ł���");

            return OUT_OF_RANGE_LIST_NUMBER;
        }


        /// <summary>
        /// ���ړI�ȏՓ˔��������Collider���i�[���鏈��
        /// </summary>
        public static int AddPhysicsColliders(OriginalCollider colliderToAdd)
        {
            // �e�R���C�_�[�̌`��ɉ������������s��
            switch (colliderToAdd._colliderShape)
            {
                // ��(Sphere)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // SphereCollieder��List�ɒǉ����鏈��
                    (List<OriginalSphereCollider>, int) addedSphereList = AddSphereCollider(_physicsSphereColliders, colliderToAdd);

                    // SphereCollider�ǉ���̃��X�g���i�[
                    _physicsSphereColliders = addedSphereList.Item1;

                    // SphereCollider���i�[���ꂽ�v�f�ԍ���Ԃ�
                    return addedSphereList.Item2;


                // ������(Box)�̃R���C�_�[�Ƃ�(������)
                case OriginalCollider._typeOfColliderShape.Box:

                    // BoxCollieder��List�ɒǉ����A����List��Collider�̊i�[�ԍ����擾����
                    (List<OriginalBoxCollider>, int) addedBoxList = AddBoxCollider(_physicsBoxColliders, colliderToAdd);

                    // BoxCollider�ǉ���̃��X�g���i�[
                    _physicsBoxColliders = addedBoxList.Item1;

                    // BoxCollider���i�[���ꂽ�v�f�ԍ���Ԃ�
                    return addedBoxList.Item2;
            }

            // ��O�̏���(��΂ɗ��Ȃ�)
            Debug.LogError("���ړI�ȏՓ˔���p��ColliderList�ɃR���C�_�[���ǉ��ł��܂���ł���");

            return OUT_OF_RANGE_LIST_NUMBER;
        }


        /// <summary>
        /// ���R���C�_�[�����X�g�ɒǉ����A���̃��X�g�Ɗi�[�����v�f�ԍ���Ԃ�
        /// </summary>
        /// <param name="colliderList"> �R���C�_�[�̃��X�g </param>
        /// <param name="colliderToAdd"> ���X�g�ɒǉ�����R���C�_�[ </param>
        private static (List<OriginalSphereCollider>, int) AddSphereCollider(List<OriginalSphereCollider> colliderList, OriginalCollider colliderToAdd)
        {
            // SphereCollider�̃C���X�^���X���擾����
            OriginalSphereCollider sphereToAdd = colliderToAdd.GetComponent<OriginalSphereCollider>();

            // List����null��T�����A���̗v�f�ԍ����擾����
            for (int i = 0; i < colliderList.Count; i++)
            {
                // List���̌�����null��Collider���i�[���鏈��
                if (colliderList[i] == null)
                {
                    colliderList[i] = sphereToAdd;

                    // Collider���i�[�����v�f�ԍ���Ԃ�
                    return (colliderList, i);
                }
            }
            // �z��̖�����Collider���i�[����
            colliderList.Add(sphereToAdd);

            // �z��̖����̔ԍ���Ԃ�
            return (colliderList, colliderList.Count - 1);
        }


        /// <summary>
        /// ������(Box)�R���C�_�[�����X�g�ɒǉ����A���̃��X�g�Ɗi�[�����v�f�ԍ���Ԃ�
        /// </summary>
        /// <param name="colliderList"> �R���C�_�[�̃��X�g </param>
        /// <param name="colliderToAdd"> ���X�g�ɒǉ�����R���C�_�[ </param>
        private static (List<OriginalBoxCollider>, int) AddBoxCollider(List<OriginalBoxCollider> colliderList, OriginalCollider colliderToAdd)
        {
            // BoxCollider�̃C���X�^���X���擾����
            OriginalBoxCollider boxToAdd = colliderToAdd.GetComponent<OriginalBoxCollider>();

            // List����null��T�����A���̗v�f�ԍ����擾����
            for (int i = 0; i < colliderList.Count; i++)
            {
                // List���̌�����null��Collider���i�[���鏈��
                if (colliderList[i] == null)
                {
                    colliderList[i] = boxToAdd;

                    // Collider���i�[�����v�f�ԍ���Ԃ�
                    return (colliderList, i);
                }
            }
            // �z��̖�����Collider���i�[����
            colliderList.Add(boxToAdd);

            // �z��̖����̔ԍ���Ԃ�
            return (colliderList, colliderList.Count - 1);
        }


        #endregion �R���C�_�[�����X�g�ɒǉ����鏈��


        #region �R���C�_�[�����X�g����폜���鏈��


        /// <summary>
        /// �e�ɑ΂��铖���蔻�������Collider��List����������鏈��
        /// </summary>
        public static void RemoveBulletColliders(OriginalCollider removeCollider, int removeColliderIndex)
        {
            // �e�R���C�_�[�̌`��ɉ��������X�g����A�R���C�_�[����������
            switch (removeCollider._colliderShape)
            {
                // ��(Sphere)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // �R���C�_�[��List�����������
                    _bulletSphereColliders[removeColliderIndex] = null;

                    break;

                // ������(Box)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Box:

                    // �R���C�_�[��List�����������
                    _bulletBoxColliders[removeColliderIndex] = null;

                    break;
            }
        }


        /// <summary>
        /// ���ړI�ȏՓ˔��������Collider��List����������鏈��
        /// </summary>
        public static void RemovePhysicsColliders(OriginalCollider removeCollider, int removeColliderIndex)
        {
            // �e�R���C�_�[�̌`��ɉ��������X�g����A�R���C�_�[����������
            switch (removeCollider._colliderShape)
            {
                // ��(Sphere)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Sphere:

                    // �R���C�_�[��List�����������
                    _physicsSphereColliders[removeColliderIndex] = null;

                    break;

                // ������(Box)�̃R���C�_�[�Ƃ�
                case OriginalCollider._typeOfColliderShape.Box:

                    // �R���C�_�[��List�����������
                    _physicsBoxColliders[removeColliderIndex] = null;

                    break;
            }
        }


        #endregion �R���C�_�[�����X�g����폜���鏈��


        /// <summary>
        /// �R���C�_�[�̎�ނŃ��X�g���܂Ƃ߂邱�Ƃ��ł���\����
        /// </summary>
        public struct ColliderLists
        {
            public List<OriginalSphereCollider> sphereList;
            public List<OriginalBoxCollider> boxList;
        }


        /// <summary>
        /// �e�̓����蔻�������Collider��List�Ԃ����\�b�h
        /// </summary>
        public static ColliderLists GetBulletColliderList()
        {
            ColliderLists returnList;

            // �e�`�̃R���C�_�[���i�[
            returnList.sphereList = _bulletSphereColliders;
            returnList.boxList = _bulletBoxColliders;

            return returnList;
        }


        /// <summary>
        /// ���ړI�ȏՓ˔��������Collider��List�Ԃ����\�b�h
        /// </summary>
        public static ColliderLists GetPhysicsColliderList()
        {
            ColliderLists returnList;

            // �e�`�̃R���C�_�[���i�[
            returnList.sphereList = _physicsSphereColliders;
            returnList.boxList = _physicsBoxColliders;

            return returnList;
        }


        /// <summary>
        /// �S�ẴR���C�_�[�̃��X�g�̏�����
        /// </summary>
        public static void AllColliderListClear()
        {
            _bulletSphereColliders = new List<OriginalSphereCollider>();
            _physicsSphereColliders = new List<OriginalSphereCollider>();;
            _bulletBoxColliders = new List<OriginalBoxCollider>();
            _physicsBoxColliders = new List<OriginalBoxCollider>();
        }
    }
}