
using UnityEngine;

/// --------------------------------------------------
/// #CircleEnemy.cs
/// �쐬��:�g�c�Y��
/// 
/// CircleEnemy�̋������s���X�N���v�g
/// --------------------------------------------------

public class CircleEnemy : TargetManager
{
    // CircleEnemy��Transform
    private Transform _thisTransform;

    [SerializeField, Label("CircleEnemy�̃p�����[�^")]
    private CircleEnemyParameter _circleEnemyParameter;

    [SerializeField, Label("���𗬂��X�N���v�g")]
    private SoundController _soundController;

    // CircleEnemy�̐��ʕ����̃x�N�g��
    private Vector3Int AIRCRAFT_FRONT_DIRECTION { get { return new Vector3Int(0, 0, 1); } }

    [SerializeField, Header("CircleEnemy�̐���̒��S�ʒu")]
    private Transform _turningCenterTransform;

    [SerializeField, Header("CircleEnemy�̏����ʒu�𔼎�������Ԃɂ��邩")]
    private bool _startToHalfCirclePosition;


    void Start()
    {
        // CircleEnemy��Transform���擾
        _thisTransform = this.GetComponent<Transform>();

        // CircleEnemy�������ʒu�Ɉړ�����
        CiecleEnemyInitialPosition();
    }


    private void FixedUpdate()
    {
        // CircleEnemy�̉~��Ɉړ�����
        CiecleEnemyMoving();

        // �̗͂��Ȃ��Ȃ����Ƃ��A�L�����N�^�[������
        if (_isDead)
        {
            // �G�t�F�N�g���o��
            Instantiate(_circleEnemyParameter._breakedEffectPrefab, this.transform.position, Quaternion.identity);

            // �����o��
            _soundController.PlaySeSound(_circleEnemyParameter._breakedSound);

            // �I�u�W�F�N�g�𖳌�������
            this.gameObject.SetActive(false);
        }
    }

    
    /// <summary>
    /// CircleEnemy�̉~����Ɉړ�����
    /// </summary>
    private void CiecleEnemyMoving()
    {
        // ������s��
        _thisTransform.rotation *= Quaternion.Euler(0, _circleEnemyParameter._turningSpeed, 0);

        // CircleEnemy�̐��ʕ����ɑO�i����
        _thisTransform.position += _thisTransform.rotation * AIRCRAFT_FRONT_DIRECTION * _circleEnemyParameter._movingSpeed;
    }


    /// <summary>
    /// �~��Ɉړ�����CircleEnemy�������ʒu�Ɉړ�����
    /// </summary>
    private void CiecleEnemyInitialPosition()
    {
        // �����ɂ�����p�x
        int halfRoundDegrees = 180;

        // �����̈ړ��ɂ���������
        float halfRoundMoveCount = halfRoundDegrees / Mathf.Abs(_circleEnemyParameter._turningSpeed);

        // �����̒���(�~����2����1)
        float halfCircumference = halfRoundMoveCount * _circleEnemyParameter._movingSpeed;

        // ���a�����߂�
        float radius = halfCircumference / Mathf.PI;

        // ��������ɂ���Ĉړ����������ς���
        if (_circleEnemyParameter._turningSpeed > 0)
        {
            radius = -radius;
        }

        // �����ʒu�����̂܂܂ȂƂ�
        if (_startToHalfCirclePosition)
        {
            // CircleEnemy�̐���̒��S�ʒu�������̔��a���������ʒu�Ɉړ�����
            _thisTransform.position = new Vector3(_turningCenterTransform.position.x + radius, _thisTransform.position.y, _thisTransform.position.z);
        }
        // �����ʒu������������Ԃ̂Ƃ�
        else
        {
            // CircleEnemy�̐���̒��S�ʒu�������̔��a���������ʒu�Ɉړ�����
            _thisTransform.position = new Vector3(_turningCenterTransform.position.x - radius, _thisTransform.position.y, _thisTransform.position.z);

            // �����̊p�x����]����
            _thisTransform.rotation *= Quaternion.Euler(0, halfRoundDegrees, 0);
        }
        
    }
}
