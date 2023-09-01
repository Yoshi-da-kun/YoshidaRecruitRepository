
using UnityEngine;

/// --------------------------------------------------
/// #OriginalBoxCollider.cs
/// �쐬��:�g�c�Y��
/// 
/// ������(Box)�̃R���C�_�[�̃f�[�^�Ə��������X�N���v�g
/// --------------------------------------------------

public class OriginalBoxCollider : OriginalCollider
{
    // ���̃R���C�_�[��Transform 
    public Transform _thisTransform { get; private set; } = default;

    [field: SerializeField, Header("�����蔻��̑傫��")]
    public Vector3 _colliderSize { get; private set; }


    private void Awake()
    {
        // ���g�̃R���C�_�[�̌`����i�[����
        _colliderShape = _typeOfColliderShape.Box;

        _thisTransform = this.transform;
    }
}
