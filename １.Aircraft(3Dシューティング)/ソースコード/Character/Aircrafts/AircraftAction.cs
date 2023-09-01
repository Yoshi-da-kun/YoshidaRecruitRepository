
using UnityEngine;
using CollisionSystem;

/// --------------------------------------------------
/// #AircraftAction.cs
/// �쐬��:�g�c�Y��
/// 
/// �@�̂̈ړ������̋������s���֐����܂Ƃ߂��X�N���v�g
/// --------------------------------------------------

public class AircraftAction : MonoBehaviour
{
    [SerializeField, Label("�@�̂̃p�����[�^")]
    private AircraftParameter _aircraftParameter = default;

    // �@�̂�Transform�ƃR���C�_�[
    private Transform _aircraftTransform = default;
    private OriginalCollider _aircraftCollider = default;

    // �@�̂̐��ʕ����̃x�N�g��
    private Vector3Int AIRCRAFT_FRONT_DIRECTION { get { return new Vector3Int(0, 0, 1); } }

    // �@�̂̍ő働�[���p�x
    public int _maxRollAngle { get { return 90; } }
    public int _maxPitchAngle { get { return 70; } }

    // ���݂̈ړ����x
    public float _currentMovementSpeed { get; private set; } = default;

    // ���݂̋@�̂̎p��(�p�x)
    private Vector3 _currentAircraftAngle = default;
    public Vector3 _aircraftAngleGetter { get { return _currentAircraftAngle; } }

    // �@�̂̃��[���ƃs�b�`�̊�ƂȂ�p�x
    private float _baseRoll = default;


    private void Start()
    {
        // �@�̂�Transform�ƃR���C�_�[���擾
        _aircraftTransform = this.GetComponent<Transform>();
        _aircraftCollider = this.GetComponent<OriginalCollider>();

        // �@�̂̏����p�����i�[
        _currentAircraftAngle = _aircraftTransform.eulerAngles;
        _currentAircraftAngle.x = 0;
        _baseRoll = _aircraftTransform.eulerAngles.z;

        // �@�̂̏������x���i�[����
        _currentMovementSpeed = _aircraftParameter._initialMovementSpeed;
    }


    /// <summary>                                                   
    /// �@�̂̈ړ��Ɛ�������邽�߂̏���
    /// </summary>
    public void AircraftMoving(float targetMovementSpeed, Vector2 turningInputVolume)
    {
        // ���͒l�̍��v�l�����߂�
        float sumInputVolume = Mathf.Abs(turningInputVolume.x) + Mathf.Abs(turningInputVolume.y);

        // ���͒l�̍��v��1�ȏ�܂���-1�����ɂȂ�Ȃ��悤�ɓ��͒l���C������
        if (sumInputVolume > 1)
        {
            // ���v�l���P�ɂȂ�悤�Ɋ��������߂ē��͒l�Ɋi�[����
            turningInputVolume = turningInputVolume / sumInputVolume;
        }

        // �@�̂̐�����s������
        AircraftAngleControl(turningInputVolume);

        // �@�̂̈ړ����s������
        AircraftMovementControl(targetMovementSpeed);
    }


    /// <summary>
    /// �@�̂̐���Ɋւ��鏈��
    /// </summary>
    private void AircraftAngleControl(Vector2 turningInputVolume)
    {
        ///-- ���[���𐧌䂷�鏈�� --///
        
        // ���͒l�ɉ������ő働�[���p�x�����߂�
        float inputMaxRollAngle = _baseRoll - turningInputVolume.x * _maxRollAngle;

        // ���݃��[�������͒l�̃��[�����傫���Ƃ�
        if (_currentAircraftAngle.z > inputMaxRollAngle)
        {
            // ���݃��[���p�����炷
            _currentAircraftAngle.z -= _aircraftParameter._rollingSpeed;

            // ���݃��[�������͒l�̃��[���𒴂��Ȃ��悤�ɂ��鏈��
            if (_currentAircraftAngle.z < inputMaxRollAngle)
            {
                _currentAircraftAngle.z = inputMaxRollAngle;
            }
        }
        // ���݃��[�������͒l�̃��[����菬�����Ƃ�
        else
        {
            // ���݃��[���𑝂₷
            _currentAircraftAngle.z += _aircraftParameter._rollingSpeed;

            // ���݃��[�������͒l�̃��[���𒴂��Ȃ��悤�ɂ��鏈��
            if (_currentAircraftAngle.z > inputMaxRollAngle)
            {
                _currentAircraftAngle.z = inputMaxRollAngle;
            }
        }


        ///-- �s�b�`�ƃ��[�����߂�̂Ɏg���l�̌v�Z --///

        // ���͂ɑ΂�������
        Vector2 turningVolume = turningInputVolume * _aircraftParameter._turningSpeed;

        // ���p�̊p�x
        sbyte ninetyDegrees = 90;

        // �@�̂̎p���������ȂƂ���0, �����Ȏ���-1�`1�Ƃ��āA�X��(���[��)�̓x����������
        float rollMagnitude = _currentAircraftAngle.z / ninetyDegrees;
        float rollRemainderMagnitude = 1 - Mathf.Abs(rollMagnitude);

        // ���[���̓x���������������Ƃ��ɂ�����̃��[���̓x����(rollRemainderMagnitude)�����ɂ���
        if (rollMagnitude < 0)
        {
            rollRemainderMagnitude = -rollRemainderMagnitude;
        }


        ///-- �c������(�㉺�ړ�)�̃s�b�`�𐧌䂷�� --///

        // �c���̓��͂ɉ������s�b�`�̉�]�ʕ������Z����
        _currentAircraftAngle.y += rollMagnitude * -turningVolume.y;
        _currentAircraftAngle.x -= Mathf.Abs(rollRemainderMagnitude) * turningVolume.y;

        // ���݃s�b�`���ő�s�b�`�𒴂��Ȃ��悤�ɂ��鏈��
        if (_currentAircraftAngle.x > _maxPitchAngle)
        {
            _currentAircraftAngle.x = _maxPitchAngle;
        }
        // ���݃s�b�`���ŏ��s�b�`�𖢖��ɂȂ�Ȃ��悤�ɂ��鏈��
        else if (_currentAircraftAngle.x < -_maxPitchAngle)
        {
            _currentAircraftAngle.x = -_maxPitchAngle;
        }


        ///-- ��������(���E�ړ�)�̃s�b�`�ƃ��[�𐧌䂷�鏈�� --///

        // ���[���̌X���ɉ������ő�{��
        float a = (2 - Mathf.Abs(rollMagnitude - rollRemainderMagnitude));

        // ���[���̌X���ɉ������A�s�b�`�ƃ��[�̉�]�ʂ����߂�B
        float horizontalPitchVolume = rollMagnitude * turningVolume.x * a;
        float horizontalYawVolume = rollRemainderMagnitude * turningVolume.x * a;

        // �����̓��͂ɉ������s�b�`�����߂�
        _currentAircraftAngle.y += rollMagnitude * horizontalPitchVolume;
        _currentAircraftAngle.x -= Mathf.Abs(rollRemainderMagnitude) * Mathf.Abs(horizontalPitchVolume);

        // �����̓��͂ɉ��������[�����߂�
        _currentAircraftAngle.y += rollRemainderMagnitude * horizontalYawVolume;
        _currentAircraftAngle.x += Mathf.Abs(rollMagnitude) * Mathf.Abs(horizontalYawVolume);


        // ���߂����[���A�s�b�`�A���[�̉�]���s��
        _aircraftTransform.rotation = Quaternion.Euler(_currentAircraftAngle);
    }


    /// <summary>
    /// �@�̂̈ړ��Ɋւ��鏈��
    /// </summary>
    private void AircraftMovementControl(float targetMovementSpeed)
    {
        ///-- �������x��p���āA���݈ړ����x��ڕW���x�ɋ߂Â��鏈�� --///

        // ���ݑ��x���ڕW���x���傫���Ƃ��̏���
        if (_currentMovementSpeed > targetMovementSpeed)
        {
            // ���ݑ��x���猸���x������
            _currentMovementSpeed -= _aircraftParameter._movementDeceleration;

            // ���ݑ��x���ڕW���x��菬�����Ȃ�����A�ڕW���x�ɂ���
            if (_currentMovementSpeed < targetMovementSpeed)
            {
                _currentMovementSpeed = targetMovementSpeed;
            }
        }
        // ���ݑ��x���ڕW���x��菬�����Ƃ��̏���
        else if (_currentMovementSpeed < targetMovementSpeed)
        {
            // ���ݑ��x��������x�𑫂�
            _currentMovementSpeed += _aircraftParameter._movementDeceleration;

            // ���ݑ��x���ڕW���x�𒴂�����A�ڕW���x�ɂ���
            if (_currentMovementSpeed > targetMovementSpeed)
            {
                _currentMovementSpeed = targetMovementSpeed;
            }
        }

        // �@�̂̈ړ��ʂ����߂鏈��
        Vector3 movementVolume = transform.rotation * AIRCRAFT_FRONT_DIRECTION;

        // �@�̂̈ړ����s��
        CollisionProcessing.PhysicsCollision(_aircraftCollider, _aircraftTransform, movementVolume * _currentMovementSpeed);
    }                      
}
