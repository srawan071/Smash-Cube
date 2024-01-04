using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class PlayercubeHolder : MonoBehaviour
{
    public GameObject playerCube;
    public Cube _playerCube;
    public GameObject Target;

   
    private float _tempPos;
    private bool _shoot;
    [SerializeField]
   private cubeInsantiater cubeInstantiater;

    private PlayerControls _playerControls;
    private RigidbodyInterpolation _rigidIntrapolation;
   
    [SerializeField]
    private float _dragSpeed;
    private float _Xclamp;
    [SerializeField]
    private ShopData _shopData;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Player.Enable();
        _playerControls.Player.Press.performed += OnPressed;
        _playerControls.Player.Press.canceled += OnReleased;
        _playerControls.Player.Delta.performed += OnDrag;
      
    }

    private IEnumerator Start()
    {
        yield return null;
       _dragSpeed = 270/(float)Screen.width;
        
    }
    private void OnPressed(InputAction.CallbackContext context)
    {
        StartCoroutine(UICheck());

       
    }

    private void OnDrag(InputAction.CallbackContext context)
    {
        if (_shoot&& _playerCube != null)
        {
            _tempPos += _playerControls.Player.Delta.ReadValue<Vector2>().x * Time.fixedDeltaTime * _dragSpeed;
            _tempPos = Mathf.Clamp(_tempPos, -_Xclamp, _Xclamp);


            _playerCube.rb.position = new Vector3(_tempPos, _playerCube.transform.position.y, _playerCube.transform.position.z);
           
            Target.transform.position = new Vector3(_playerCube.transform.position.x, 1, 1);
        }

       
    }

    private void OnReleased(InputAction.CallbackContext context)
    {
       
        if (_shoot && _playerCube != null)
        {
           
            _playerCube.rb.isKinematic = false;
            _playerCube.rb.interpolation = RigidbodyInterpolation.None;

            _playerCube.outcheck();
          
            _playerCube.rb.velocity = Vector3.forward * 18;
            cubeInstantiater.InstantiateSwipeCube(_tempPos);
            if(!_playerCube.bomb)
            cubeInstantiater.GroundCubes.Add(_playerCube);
            _playerCube.transform.SetParent(cubeInstantiater.transform);


            _playerCube.trail.EnableTrail();

            Target.SetActive(false);
            _playerCube = null;
            _shoot = false;
            Sounds.PlaySoundSource(5);
        }
      

    }

  
    private void OnDisable()
    {

        _playerControls.Player.Press.performed -= OnPressed;
        _playerControls.Player.Press.canceled -= OnReleased;
        _playerControls.Player.Delta.performed -= OnDrag;
        _playerControls.Player.Disable();
    }

    private IEnumerator UICheck()
    {
        yield return new WaitForEndOfFrame();
        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.singleton.isPaused = true;
        }
        else
            GameManager.singleton.isPaused = false;
        if (!_shoot && !GameManager.singleton.isPaused && _playerCube != null)
        {
           // _playerCube.rb.position = new Vector3(_tempPos, _playerCube.transform.position.y, _playerCube.transform.position.z);
            _shoot = true;
            Target.SetActive(true);
            Target.transform.position = new Vector3(_playerCube.transform.position.x, 1, 1);

            if (_shopData.SkinSide == 0)
            {
                _Xclamp = 1.65f;

            }
            else
            {
                _Xclamp = 1.5f;
            }


        }
    }
}
    


