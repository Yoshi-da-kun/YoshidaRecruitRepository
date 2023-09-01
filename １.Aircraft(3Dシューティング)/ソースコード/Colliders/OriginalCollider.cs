
using UnityEngine;
using ColliderDataCollection;

/// --------------------------------------------------
/// #OriginalCollider.cs
/// �쐬��:�g�c�Y��
/// 
/// ����̃R���C�_�[�̃f�[�^�Ə��������X�N���v�g
/// --------------------------------------------------

public class OriginalCollider : MonoBehaviour
{
    // �R���C�_�[�̌`�������enum
    public enum _typeOfColliderShape
    {
        Sphere,
        Box
    }

    // ���̃R���C�_�[�̌`��
    public _typeOfColliderShape _colliderShape { get; protected set; } = default;


    [SerializeField, Header("�e�̓����蔻��(�`�F�b�N��ON)")]
    private bool _isBulletCollision = false;

    [SerializeField, Header("���ړI�ȏՓ˔���(�`�F�b�N��ON)")]
    private bool _isPhysicsCollision = false;

    // �e���育�Ƃ�List�Ɋi�[����Ă���v�f�ԍ�
    private int _bulletColliderStoredIndex = default;
    private int _physicsColliderStoredIndex = default;


    /// <summary>
    /// �I�u�W�F�N�g�L�������ɁA�R���C�_�[�Ǘ��X�N���v�g�Ɏ��g���i�[����
    /// </summary>
    private void OnEnable()
    {
        // �e�̓����蔻�肪ON�̂Ƃ�
        if (_isBulletCollision)
        {
            _bulletColliderStoredIndex = ColliderDatas.AddBulletColliders(this);
        }
        // ���ڏՓ˔��肪ON�̂Ƃ�
        if (_isPhysicsCollision)
        {
            _physicsColliderStoredIndex = ColliderDatas.AddPhysicsColliders(this);
        }
    }


    /// <summary>
    /// �I�u�W�F�N�g���������ɁA�R���C�_�[�Ǘ��X�N���v�g���玩�g���폜����
    /// </summary>
    private void OnDisable()
    {
        // �e�̓����蔻�肪ON�̂Ƃ�
        if (_isBulletCollision)
        {
            ColliderDatas.RemoveBulletColliders(this, _bulletColliderStoredIndex);
        }
        // �����Փ˔��肪ON�̂Ƃ�
        if (_isPhysicsCollision)
        {
            ColliderDatas.RemovePhysicsColliders(this, _physicsColliderStoredIndex);
        }
    }
}
