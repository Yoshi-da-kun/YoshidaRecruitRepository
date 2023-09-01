
using System.Collections.Generic;
using UnityEngine;

/// --------------------------------------------------
/// #PuyoPresentation.cs
/// �쐬��:�g�c�Y��
/// 
/// �Ղ�̉��o���s���X�N���v�g�ł�
/// --------------------------------------------------

public class PuyoPresentation : MonoBehaviour
{
    [SerializeField]
    private SummarizeScriptableObjects _summarizeScriptableObjects;

    // �e�p�����[�^���܂Ƃ߂��X�N���v�g
    private PresentationParameter _presentationParameter;
    private FieldParameter _fieldParameter;


    // �ݒu���o���s���Ղ�
    private List<Transform> _installPuyosTransforms = new List<Transform>();

    // �Ղ�ݒu�̉��o���Ԃ��v������
    private List<float> _installPresentationElapsedTimes = new List<float>();

    // �Ղ�ݒu�̉��o���̃t���O
    public bool _duringInstallPresentation { get; private set; } = default;

    // �Ղ悪�ݒu���o���̕��ׂ�������Ԃ��������t���O
    private bool _isPuyoFlatScale = default;


    // �����鉉�o���s���Ղ�
    private List<SpriteRenderer> _eracePuyosRenderer = new List<SpriteRenderer>();

    // �Ղ悪�����鉉�o���̃t���O
    public bool _duringEracePresentation { get; private set; } = default;

    // �Ղ悪�\������Ă��邩�������t���O
    private bool _isPuyoDisplaying = default;

    // �Ղ悪�����鉉�o���Ԃ��v������
    private float _eracePresentationElapsedTime = default;


    private void Start()
    {
        // �e�p�����[�^���擾
        _presentationParameter = _summarizeScriptableObjects._prensentationParameter;
        _fieldParameter = _summarizeScriptableObjects._fieldParameter;
    }


    private void FixedUpdate()
    {
        // �Ղ��ݒu�����Ƃ��̉��o���s��
        PuyoInstallPresentationProcess();

        // �Ղ悪�����鎞�̉��o���s��
        PuyoEracePresentationProcess();
    }


    /// <summary>
    /// �Ղ��ݒu�����Ƃ��̉��o
    /// </summary>
    private void PuyoInstallPresentationProcess()
    {
        // �Ղ��ݒu���鉉�o������Ȃ���Ώ������Ȃ�
        if (!_duringInstallPresentation)
        {
            return;
        }

        // �Ղ��ݒu�����Ƃ��̉��o���I�����邩�𔻒肷��
        for (int i = 0; i < _installPuyosTransforms.Count; i++)
        {
            // �Ղ��ݒu�����Ƃ��̉��o���Ԃ��v������
            _installPresentationElapsedTimes[i] += Time.fixedDeltaTime;

            // �Ղ��ݒu�����Ƃ��̉��o���Ԃ��I�����ԂɒB������I������
            if (_installPresentationElapsedTimes[i] >= _presentationParameter._installPresentationTime)
            {
                // �Ղ�̑傫�������ɖ߂�
                _installPuyosTransforms[i].localScale = _fieldParameter._scaleOfOneMass;

                // �폜�\���Transform�𖖔��̗v�f�ƌ�������
                Transform fromEndPointerTransform = _installPuyosTransforms[_installPuyosTransforms.Count - 1];
                _installPuyosTransforms[_installPuyosTransforms.Count - 1] = _installPuyosTransforms[i];
                _installPuyosTransforms[i] = fromEndPointerTransform;

                // �폜�\��̌v�����Ԃ𖖔��̗v�f�ƌ�������
                float fromEndPointerTime = _installPresentationElapsedTimes[_installPuyosTransforms.Count - 1];
                _installPresentationElapsedTimes[_installPuyosTransforms.Count - 1] = _installPresentationElapsedTimes[i];
                _installPresentationElapsedTimes[i] = fromEndPointerTime;

                // �����̗v�f���폜����
                _installPuyosTransforms.RemoveAt(_installPuyosTransforms.Count - 1);
                _installPresentationElapsedTimes.RemoveAt(_installPuyosTransforms.Count - 1);

                // ��������v�f�̃|�C���^����O�ɖ߂�(������x�����v�f�ԍ����������邽��)
                i--;

                // �ݒu���o���s���Ă���Ղ悪�ЂƂ��Ȃ���Ή��o�������I������
                if (_installPuyosTransforms.Count == 0)
                {
                    _duringInstallPresentation = false;

                    return;
                }

                continue;
            }
        }

        ///-- �ݒu���̉��o���s�� --///

        // �Ղ悪���ׂ�������Ԃ̂Ƃ�
        if (_isPuyoFlatScale)
        {
            // �Ղ���c�ɒ����傫���ɕς���
            for (int i = 0; i < _installPuyosTransforms.Count; i++)
            {
                _installPuyosTransforms[i].localScale = _fieldParameter._scaleOfOneMass;
            }
        }
        // �Ղ悪�c�ɒ�����Ԃ̂Ƃ�
        else
        {
            // �Ղ�𕽂ׂ������傫���ɕς���
            for (int i = 0; i < _installPuyosTransforms.Count; i++)
            {
                _installPuyosTransforms[i].localScale = _fieldParameter._scaleOfOneMass;
            }
        }
    }


    /// <summary>
    /// �Ղ悪�����鎞�̉��o
    /// </summary>
    private void PuyoEracePresentationProcess()
    {
        // �Ղ悪�����鉉�o������Ȃ���Ώ������Ȃ�
        if (!_duringEracePresentation)
        {
            return;
        }
        
        // �Ղ悪�\������Ă���Ȃ�Ղ�̕\������߂�
        if (_isPuyoDisplaying)
        {
            // �Ղ�̕\������������
            for (int i = 0; i < _eracePuyosRenderer.Count; i++)
            {
                _eracePuyosRenderer[i].enabled = false;
            }

            // �Ղ�̕\���t���O���\���̏�Ԃɂ���
            _isPuyoDisplaying = false;
        }
        // �Ղ悪�\������Ă��Ȃ��Ȃ�Ղ��\������
        else
        {
            // �Ղ��\�����鏈��
            for (int i = 0; i < _eracePuyosRenderer.Count; i++)
            {
                _eracePuyosRenderer[i].enabled = true;
            }
            
            // �Ղ�̕\���t���O���Z�b�g����
            _isPuyoDisplaying = true;
        }

        // �Ղ悪������Ƃ��̉��o���Ԃ��v������
        _eracePresentationElapsedTime += Time.fixedDeltaTime;

        // �Ղ悪������Ƃ��̉��o���Ԃ��I�����ԂɒB������I������
        if (_eracePresentationElapsedTime >= _presentationParameter._eracePresentationTime)
        {
            // �Ղ悪������Ƃ��̉��o���I������
            _duringEracePresentation = false;

            // �Ղ��\����Ԃɂ���
            for (int i = 0; i < _eracePuyosRenderer.Count; i++)
            {
                _eracePuyosRenderer[i].enabled = true;
            }

            // ���o�̌v�����Ԃ�������
            _eracePresentationElapsedTime = 0;

            // �Ղ��SpriteRenderer�̔z�������������
            _eracePuyosRenderer = new List<SpriteRenderer>();
        }
    }


    /// <summary>
    /// �Ղ悪�ݒu�����Ƃ��̉��o
    /// </summary>
    public void PuyoInstallPresentationStart(List<Transform> installPuyos)
    {
        // �ݒu�����Ղ��Transform�Ɖ��o�v�����Ԃ��i�[����
        for (int i = 0; i < installPuyos.Count; i++)
        {
            _installPuyosTransforms.Add(installPuyos[i]);
            _installPresentationElapsedTimes.Add(0);
        }
        
        _duringInstallPresentation = true;
    }


    /// <summary>
    /// �Ղ悪�ݒu�����Ƃ��̉��o
    /// </summary>
    public void PuyoEracePresentationStart(List<Transform> eracePuyos)
    {
        // �Ղ��SpriteRenderer���擾���A�i�[����
        for (int i = 0; i < eracePuyos.Count; i++)
        {
            _eracePuyosRenderer.Add(eracePuyos[i].gameObject.GetComponent<SpriteRenderer>());
        }
        
        // �Ղ悪������Ƃ��̉��o���J�n����
        _duringEracePresentation = true;

        // �Ղ�\���t���O���Z�b�g����
        _isPuyoDisplaying = true;
    }
}