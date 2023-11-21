using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Atributo de la clase que permite que la variable se pueda modificar en el Editor pero no desde otro script
    [SerializeField] float _moveSpeed, _gravityModifier, _jumpPower, _runSpeed;
    bool _canDoubleJump;

    private Vector3 _moveInput;

    Transform _camTrans;
    public bool invertX;
    public bool invertY;
    public float mouseSensibility;

    CharacterController _charCon;
    Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _charCon = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        _camTrans = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        //Store y velocity
        float _yStore = _moveInput.y;

        //Movement
        //_moveInput.x = Input.GetAxisRaw("Horizontal") * _moveSpeed * Time.deltaTime;
        //_moveInput.z = Input.GetAxisRaw("Vertical") * _moveSpeed * Time.deltaTime;
        Vector3 _vertMove = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 _horMove = transform.right * Input.GetAxisRaw("Horizontal");
        _moveInput = _horMove + _vertMove;
        //Hacemos que el movimiento siempre sea de 1 incluso al ir en diagonal
        _moveInput.Normalize();

        //Running
        if (Input.GetKey(KeyCode.LeftShift))
            _moveInput *= _runSpeed; // _moveInput = _moveInput * _runSpeed;
        else
            _moveInput *= _moveSpeed;

        //Gravity
        _moveInput.y = _yStore;
        _moveInput.y += Physics.gravity.y * _gravityModifier;
        //Si estamos tocando suelo, se resetea la gravedad para no acumularla
        if(_charCon.isGrounded)
            _moveInput.y = Physics.gravity.y * _gravityModifier * Time.deltaTime;

        //Double Jump
        if (Input.GetKeyDown(KeyCode.Space) && _charCon.isGrounded)
        {
            _moveInput.y = _jumpPower;
            _canDoubleJump = true;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
        {
            _moveInput.y = _jumpPower;
            _canDoubleJump = false;
        }    

        _charCon.Move(_moveInput * Time.deltaTime);

        //Camera Movement
        Vector3 _mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensibility;
        if(invertX)
            _mouseInput.x = -_mouseInput.x;
        if(invertY)
            _mouseInput.y = -_mouseInput.y;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + _mouseInput.x, transform.rotation.eulerAngles.z);
        _camTrans.rotation = Quaternion.Euler(_camTrans.rotation.eulerAngles + new Vector3(-_mouseInput.y, 0f, 0f));

        //Animations
        _anim.SetFloat("moveSpeed", _moveInput.magnitude); //Magnitude es la longitud del vector que equivale a velocidad
        _anim.SetBool("isGround", _charCon.isGrounded);
    }
}
