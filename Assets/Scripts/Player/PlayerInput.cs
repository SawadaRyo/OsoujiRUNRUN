using UnityEngine;
using UniRx;

public class PlayerInput : MonoBehaviour
{
    [SerializeField, Tooltip("���̃I�u�W�F�N�g�̈ړ����x")]
    float _speed = 5f;
    [SerializeField, Tooltip("�S�~�̎擾����")]
    Transform _boxVec = null;
    [SerializeField, Tooltip("���̃I�u�W�F�N�g��RigitBody")]
    Rigidbody2D _rb = null;
    [SerializeField, Tooltip("�S�~�̎擾�͈�")]
    Vector2 _boxSize = Vector2.zero;
    [SerializeField, Tooltip("�擾����S�~�̃��C���[")]
    LayerMask _checkTrashLayer = 3;

    [Tooltip("�p�[�e�B�N���̋N���m�F")]
    BoolReactiveProperty _vaccum = new BoolReactiveProperty();

    /// <summary>
    /// �p�[�e�B�N���̋N���m�F(�ǂݎ���p)
    /// </summary>
    public IReadOnlyReactiveProperty<bool> Vaccum => _vaccum;

    void Awake()
    {
        Initialize(gameObject.GetComponent<PlayerContoller>());
    }
    public void Initialize(PlayerContoller playerContoller)
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                bool leftClick = Input.GetMouseButton(0);
                _vaccum.Value = Input.GetMouseButton(1) || Input.GetKey(KeyCode.E);

                InputMouse(playerContoller, leftClick);
                InputKey(playerContoller, moveInput);
                if (!leftClick && moveInput == Vector2.zero && !_vaccum.Value)
                {
                    _rb.velocity = Vector2.zero;
                }
            }).AddTo(this);
    }
    void InputMouse(PlayerContoller playerContoller, bool leftClick)
    {
        if (InGame.GameManager.Instance.GameState.Value == Commons.Enum.InGameState.Game)
        {
            if (leftClick || (_vaccum.Value && Input.GetMouseButton(1) == true))
            {
                Vector2 mouseTrans = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 moveVec = (mouseTrans - (Vector2)this.transform.position);

                PlayerMoveMethod(leftClick, _speed, moveVec, _rb);
                PlayerRotateMethod(moveVec);
            }
            if (_vaccum.Value && Input.GetMouseButton(1) == true && Input.GetKey(KeyCode.E) == false)
            {
                playerContoller.VacuumingGarbege(_vaccum.Value, _boxVec, _boxSize, _checkTrashLayer);
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    void InputKey(PlayerContoller playerContoller, Vector2 moveInput)
    {
        if (InGame.GameManager.Instance.GameState.Value == Commons.Enum.InGameState.Game)
        {
            if (moveInput != Vector2.zero)
            {
                PlayerMoveMethod(moveInput, _speed, _rb);
                PlayerRotateMethod(moveInput);
            }
            else if (_vaccum.Value && Input.GetKey(KeyCode.E) == true && Input.GetMouseButton(1) == false)
            {
                playerContoller.VacuumingGarbege(_vaccum.Value, _boxVec, _boxSize, _checkTrashLayer);
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// �v���C���[�̓���(�}�E�X�o�[�W����)
    /// </summary>
    public void PlayerMoveMethod(bool isInput, float speed, Vector2 moveVec, Rigidbody2D rb)
    {
        if (isInput)
        {
            if (moveVec.magnitude < 1f) return;
            rb.velocity = moveVec.normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// �v���C���[�̓���(�L�[�{�[�h�o�[�W����)
    /// </summary>
    public void PlayerMoveMethod(Vector2 moveVec, float speed, Rigidbody2D rb)
    {
        rb.velocity = moveVec.normalized * speed;
    }
    /// <summary>
    /// �v���C���[�̉�]
    /// </summary>
    void PlayerRotateMethod(Vector2 moveVec)
    {
        if (moveVec.magnitude < 1f) return;
        transform.right = moveVec;
    }
}
