
using System.Collections.Generic;
using UnityEngine;
using CollisionSystem;

/// --------------------------------------------------
/// #ExplosionBulletProcess.cs
/// �쐬��:�g�c�Y��
/// 
/// �@�̂̒e�̏������s���X�N���v�g
/// --------------------------------------------------

public class ExplosionBulletProcess : MonoBehaviour 
{
    [SerializeField, Label("�e�̃p�����[�^")]
    private BulletParameter _bulletParameter;

    // ���̒e�̃R���C�_�[��Transform
    private OriginalCollider _thisCollider = default;
    private Transform _bulletTransform = default;

    // �e�̈ړ�����
    private Vector3 _bulletMoveDirection = default;

    // �e�����˂���Ă���̌v������
    private float _shotedElapsedTime = 0;

    // ���𗬂��X�N���v�g
    private SoundController _soundController;

    // �e�����������Ƃ��̃G�t�F�N�g
    private Transform _hitEffectTransform = default;


    private void Start()
    {
        // �R���C�_�[��Transform���擾����
        _thisCollider = this.GetComponent<OriginalCollider>();
        _bulletTransform = this.GetComponent<Transform>();
    }


    /// <summary>
    /// �e�̈ړ��A�U���������s��
    /// </summary>
    private void FixedUpdate()
    {
        // �v�����Ԃ̉��Z���s��
        _shotedElapsedTime += Time.fixedDeltaTime;

        // �e�̈ړ����s��
        _bulletTransform.position += _bulletParameter._bulletSpeed * _bulletMoveDirection;

        // �����Փ˂����I�u�W�F�N�g�̃��X�g���i�[����
        List<OriginalCollider> collidingObjects = CollisionProcessing.BulletCollision(_thisCollider);

        // �e�������蔻��ɂ������Ă��Ȃ��Ƃ�
        if (collidingObjects.Count == 0)
        {
            // �e�����˂���Ă����莞�Ԃ��o�߂�����A�e������
            if (_shotedElapsedTime >= _bulletParameter._bulletBreakTime)
            {
                breakBullet();
            }

            return;
        }

        // �e�����������I�u�W�F�N�g�ɑ̗͂��������Ƃ��A�̗͂����炷����
        for (int i = 0; i < collidingObjects.Count; i++)
        {
            // �̗͂��i�[����Ă���X�N���v�g���擾
            CharacterBase collideCharacter = collidingObjects[i].GetComponent<CharacterBase>();

            // �̗͂������Ă��Ȃ��Ȃ玟�̗v�f��
            if (!collideCharacter)
            {
                continue;
            }
            // �e�����������L�����N�^�[�Ƀ_���[�W��^����
            collideCharacter.TakesDamage(_bulletParameter._bulletPower);
        }

        // �G�t�F�N�g���Đ�����
        _hitEffectTransform.position = _bulletTransform.position;
        _hitEffectTransform.gameObject.SetActive(true);

        // �e�����������Ƃ��̉����Đ�����
        _soundController.PlaySeSound(_bulletParameter._hitSound);

        // �e�����������Ƃ��ɒe������
        breakBullet();
    }


    /// <summary>
    /// �e���󂳂ꂽ(�����ꂽ)�Ƃ�
    /// </summary>
    private void breakBullet()
    {
        // �e�����˂���Ă���̌v������
        _shotedElapsedTime = 0;

        // �e������
        this.gameObject.SetActive(false);
    }


    /// <summary>
    /// �e�𔭎˂���
    /// </summary>
    public void ExplosionBulletShot(Vector3 shotDirection, SoundController soundController)
    {
        // �G�t�F�N�g����������Ă��Ȃ��Ȃ�
        if (!_hitEffectTransform)
        {
            // �e�����������Ƃ��̃G�t�F�N�g�𐶐�
            _hitEffectTransform = Instantiate(_bulletParameter._hitEffect, this.transform.position, Quaternion.identity).transform;
        }

        // �G�t�F�N�g�I�u�W�F�N�g�𖳌���
        _hitEffectTransform.gameObject.SetActive(false);

        // �e�̈ړ��������i�[
        _bulletMoveDirection = shotDirection;

        // ���ʉ��𗬂��X�N���v�g���擾
        _soundController = soundController;

        // ���ˎ��̉����o��
        _soundController.PlaySeSound(_bulletParameter._shotSound);
    }
}
