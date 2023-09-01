
using UnityEngine;

/// --------------------------------------------------------
/// #PlayerActions.cs
/// 
/// �v���C���[�̓�����s���X�N���v�g
/// --------------------------------------------------------

public class PlayerActions : MonoBehaviour
{
    [SerializeField, Label("�v���C���[�̃p�����[�^")]
    private PlayerParameter _playerParameter;

    [SerializeField, Label("�t�B�[���h�̃p�����[�^")]
    private FieldParameter _fieldParameter;

    // ���̃L����(�v���C���[)��Transform��Rigidbody
    private Transform _playerTransform = default;
    private Rigidbody2D _playerRigidbody = default;

    // �v���C���[�̓����蔻��̔��a
    private float _playerRadius = default;

    // �}�b�v��̈ړ�����Ɖ����̍��W
    private float _upperLimitOfMovement = default, _underLimitOfMovement = default;

    // �v���C���[�̌����Ă������
    private Vector2 _playerFacingDirection = default;

    // ���̂��󂷗p��Ray��Hit����Collider
    private Collider2D _breakingCollider = default;

    // ���̂��󂷊Ԋu�̌v������
    private float _breakIntervalElapsedTime;


    // �ړ��p�̓��͂��i�[����ϐ�
    private float horizontalInput = default;
    private float verticalInput = default;

    // �󂹂�I�u�W�F�N�g�̃^�O
    private const string BREAKABLE_OBJECTS_TAG = "BreakableObject";


    private void Start()
    {
        // �L�����N�^�[��Transform��Rigidbody���擾
        _playerTransform = this.GetComponent<Transform>();
        _playerRigidbody = this.GetComponent<Rigidbody2D>();

        // �L�����N�^�[�̔��a���擾
        _playerRadius = this.GetComponent<CircleCollider2D>().radius * _playerTransform.localScale.z;

        // �}�b�v�̏�[�Ɖ��[�̍��W���擾
        float topEdgeOfField = _fieldParameter._fieldGenerateStartPosition.y + _fieldParameter._oneBlockSize.y * _fieldParameter._fieldColumnSize;
        float bottomEdgeOfField = _fieldParameter._fieldGenerateStartPosition.y;

        // �L�����N�^�[�̈ړ�����Ɖ��������߂�
        _upperLimitOfMovement = topEdgeOfField - _playerRadius;
        _underLimitOfMovement = bottomEdgeOfField + _playerRadius;
    }


    private void Update()
    {
        // ���͂��󂯎�鏈��
        RecieveInput();

        // �j��̃A�N�V�������s��
        PlayerBreakAction();
    }


    /// <summary>
    /// �R���g���[���ƃL�[�{�[�h�̓��͂��󂯎��
    /// </summary>
    private void RecieveInput()
    {
        // �ړ��̓��͂��󂯎��
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }



    private void FixedUpdate()
    {
        // �v���C���[�̈ړ����s��
        PlayerMoveProcess();


    }


    /// <summary>
    /// �v���C���[�̈ړ����s������
    /// </summary>
    private void PlayerMoveProcess()
    {
        // �ړ��p�̓��͂���Ă��Ȃ���Ώ������Ȃ�
        if (horizontalInput == 0 && verticalInput == 0)
        {
            return;
        }

        // �v���C���[�̈ړ��\��ꏊ
        Vector3 playerMoveDestinationPos = _playerTransform.position + new Vector3(horizontalInput, verticalInput, 0) * _playerParameter._moveSpeed;

        // �v���C���[�̌����Ă���������i�[����
        _playerFacingDirection = new Vector2(horizontalInput, verticalInput);

        // �v���C���[���t�B�[���h�̉��̒[�𒴂����Ƃ��A�ړ�������̒[�ɂ���
        if (playerMoveDestinationPos.y < _underLimitOfMovement)
        {
            playerMoveDestinationPos.y = _underLimitOfMovement;
        }
        // �v���C���[���t�B�[���h�̏�̒[�𒴂����Ƃ��A�ړ������̒[�ɂ���
        else if (playerMoveDestinationPos.y > _upperLimitOfMovement)
        {
            playerMoveDestinationPos.y = _upperLimitOfMovement;
        }

        // �v���C���[�̈ړ����s��
        _playerRigidbody.MovePosition(playerMoveDestinationPos);
    }


    /// <summary>
    /// �v���C���[�̔j��A�N�V�������s������
    /// </summary>
    private void PlayerBreakAction()
    {
        // ray�̉���(�f�o�b�O)���s��
        DebugPlayerActionRay();


        // Ray�̔��ˈʒu�����߂�
        Vector2 rayStartPos = _playerFacingDirection * _playerRadius;
        rayStartPos += new Vector2(_playerTransform.position.x, _playerTransform.position.y);

        // �v���C���[�𒆐S�Ɍ����Ă��������Ray���o��
        RaycastHit2D facingDirectionRayHit = Physics2D.Raycast(rayStartPos, _playerFacingDirection, _playerParameter._breakingDistance);

        // Ray���������Ă��Ȃ��Ƃ��A�������I������
        if (!facingDirectionRayHit)
        {
            return;
        }

        // Ray���󂹂�I�u�W�F�N�g�ɂ��������Ƃ��̏���
        if (facingDirectionRayHit.collider.CompareTag(BREAKABLE_OBJECTS_TAG))
        {
            // �j��Ώۂ̃I�u�W�F�N�g���ς�����Ƃ�
            if (_breakingCollider != facingDirectionRayHit.collider)
            {
                // �j��Ώۂ̃R���C�_�[���i�[����
                _breakingCollider = facingDirectionRayHit.collider;

                // �j��Ԋu�̌v�����Ԃ�������
                _breakIntervalElapsedTime = 0;

                return;
            }
            // �j��Ώۂ̃I�u�W�F�N�g���ς��Ȃ��Ƃ�
            else
            {            
                // �j��Ԋu���Ԃ̌v��
                _breakIntervalElapsedTime += Time.deltaTime;
            }

            // �j��Ԋu���Ԃ��o�߂��Ă��Ȃ���Ώ������I������
            if (_breakIntervalElapsedTime < _playerParameter._breakIntervalTime)
            {
                return;
            }

            // �I�u�W�F�N�g��HP���i�[����X�N���v�g�擾
            BreakableObjects breakableObjects = facingDirectionRayHit.collider.GetComponent<BreakableObjects>();

            // �I�u�W�F�N�g��HP������������
            breakableObjects.DamagedProcess(_playerParameter._attackVolume);
        }
        else
        {
            // �j�󒆂̃I�u�W�F�N�g����ɂ���
            _breakingCollider = null;
        }
    }


    /// <summary>
    /// �v���C���[����o��Ray���������鏈��(�f�o�b�O)
    /// </summary>
    private void DebugPlayerActionRay()
    {
        // Ray�̔��ˈʒu�����߂�
        Vector2 rayStartPos = _playerFacingDirection * _playerRadius;
        rayStartPos += new Vector2(_playerTransform.position.x, _playerTransform.position.y);

        // �C������{�������߂邽�߂̏���
        Vector2 correctionDivisor = _playerFacingDirection;

        if (_playerFacingDirection.x < 0)
        {
            correctionDivisor.x = -_playerFacingDirection.x;
        }
        if (_playerFacingDirection.y < 0)
        {
            correctionDivisor.y = -_playerFacingDirection.y;
        }
        // �v���C���[�̌����Ă�������̒l�̍��v���P�ɂȂ�悤�ɏC������{��
        float directionCorrectionMultiplier = 1 / (correctionDivisor.x + correctionDivisor.y);

        // �����Ă�������̒l���C������
        _playerFacingDirection = new Vector2(_playerFacingDirection.x, _playerFacingDirection.y) * directionCorrectionMultiplier;

        Debug.DrawRay(rayStartPos, _playerFacingDirection * 3, Color.red);
    }
}
