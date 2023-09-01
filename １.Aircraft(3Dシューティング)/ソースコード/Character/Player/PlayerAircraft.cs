
using UnityEngine;
using ControllerInput;

/// --------------------------------------------------
/// #PlayerAircraft.cs
/// �쐬��:�g�c�Y��
/// 
/// �v���C���[�̋@�̂𐧌䂷��X�N���v�g
/// --------------------------------------------------

public class PlayerAircraft : CharacterBase
{
    [SerializeField, Label("�@�̂̃p�����[�^")]
    private AircraftParameter _aircraftParameter = default;

    [SerializeField, Label("�v���C���[�̃p�����[�^")]
    private PlayerParameter _playerParameter = default;

    // �e��X�N���v�g
    private AircraftAction _aircraftAction = default;

    private AircraftBullet _aircraftBullet = default;

    private WarpSkill _warpSkill = default;


    // �@�̂̐��ʕ����̃x�N�g��
    private Vector3Int AIRCRAFT_FRONT_DIRECTION { get { return new Vector3Int(0, 0, 1); } }

    // �@�̂�Transform
    private Transform _aircraftTransform = default;

    // ����p�̓��͒l
    private Vector2 _turningInputVolume = default;

    // ���x�𒲐�������͂̒l
    private float _speedControlInput = default;


    private void Start()
    {
        // �q��@�֘A�̃X�N���v�g���擾
        _aircraftAction = this.GetComponent<AircraftAction>();
        _aircraftBullet = this.GetComponent<AircraftBullet>();

        // �v���C���[�֘A�̃X�N���v�g���擾
        _warpSkill = this.GetComponent<WarpSkill>();

        // �@�̂�Transform���擾
        _aircraftTransform = this.GetComponent<Transform>();
    }


    /// <summary>
    /// �v���C���[�̓��͂��Ԃ��Ƃ̏������s��
    /// </summary>
    private void Update()
    {
        // ����ʂ��i�[����
        _turningInputVolume = PlayerInput.TurningInput();

        // ���x�������͂̒l���擾����
        _speedControlInput = PlayerInput.SpeedControlInput();

        // �e���˂̓��͂��擾����
        if (PlayerInput.BulletShotInput())
        {
            // �@�̂̈ړ����������߂鏈��
            Vector3 movementDirection = transform.rotation * AIRCRAFT_FRONT_DIRECTION;

            // �e�𔭎˂���
            _aircraftBullet.AircraftMachinegunShot(movementDirection);
        }

        // ���͂ɉ����ă��[�v�X�L���̐ݒu���s��
        if (PlayerInput.TimeWarpBombInput())
        {
            _warpSkill.WarpSkillStart();
        }

        // �̗͂��Ȃ��Ȃ������L�����N�^�[������
        if (_isDead)
        {
            // �G�t�F�N�g���o��
            Instantiate(_playerParameter._breakedEffectPrefab, this.transform.position, Quaternion.identity);
            
            this.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// �@�̂̈ړ��⋓���Ɋւ��鏈�����s��
    /// </summary>
    private void FixedUpdate()
    {
        // ���[�v���̂Ƃ��̏���
        if (_warpSkill._isWarping)
        {
            _warpSkill.WarpingProcessing(_aircraftTransform);

            return;
        }

        // ���[�v�X�L���������̏���
        if (_warpSkill._inWarpSkill)
        {
            _warpSkill.WarpSkillProcessing(_aircraftTransform.position, _aircraftTransform.rotation);
        }

        // �ړ��Ɋւ��鏈�����s��
        MoveProcess();
    }


    /// <summary>
    /// �ړ��Ɋւ��鏈�����s��
    /// </summary>
    private void MoveProcess()
    {
        // ���݂̓��͂ɉ������ړ����x�̍ő�l
        float _currentMaxMovementSpeed = default;

        // �u�[�X�g���̋@�̍ő呬�x���i�[����
        if (_speedControlInput > 0)
        {
            _currentMaxMovementSpeed = _aircraftParameter._boostMovementSpeed;
        }
        // �������̋@�̂̍ő呬�x���i�[����
        else if (_speedControlInput < 0)
        {
            _currentMaxMovementSpeed = _aircraftParameter._slowMovementSpeed;
        }
        // ���݂̋@�̂̑��x���i�[����
        else
        {
            _currentMaxMovementSpeed = _aircraftAction._currentMovementSpeed;
        }

        // �@�̂̈ړ��Ɛ�����s��
        _aircraftAction.AircraftMoving(_currentMaxMovementSpeed, _turningInputVolume);
    }
}