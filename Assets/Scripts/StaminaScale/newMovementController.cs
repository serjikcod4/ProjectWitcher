﻿using System;
using UnityEngine;


public class newMovementController : MonoBehaviour
{
    protected CharacterController _controller;

    //Камера игрока
    [SerializeField] private Transform mainCamera;

    [SerializeField] protected float jumpSpeed = 8.0f;

    [SerializeField] protected float speed = 5f;

    [SerializeField] private float runSpeed = 10f;

    [SerializeField] private float rotateSpeed = 15f;

    [SerializeField] private float mouseSensivity = 4f;

    [SerializeField] private KeyCode runButton = KeyCode.LeftShift;

    [SerializeField] private float walkStaminaDrain = 0.1f;

    [SerializeField] private float runStaminaDrain = 0.3f;

    [SerializeField] protected float gravity = -9.81f;

    [SerializeField] private UnitModel _unitModel;

    [SerializeField] private float x;
    [SerializeField] private float z;

    private bool _isRunning = false;
    private bool _isGrounded = false;
    private bool _isStanding = true;

    //Вектор движения персонажа.
    private Vector3 Movement = Vector3.zero;
    private void Awake()
    {
        _unitModel = GameObject.FindGameObjectWithTag("Player").GetComponent<UnitModel>();
    }
    void Start()
    {
        mainCamera = Camera.main.transform;
        _controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        //Нулевой вектор.
        Movement = Vector3.zero;

        _isRunning = Input.GetKey(runButton) & _unitModel._canRun == true;

        // Check to see if the A or D key are being pressed
        x = Input.GetAxis("Horizontal") * (_isRunning ? runSpeed : speed);

        // Check to see if the W or S key is being pressed.  
        z = Input.GetAxis("Vertical") * (_isRunning ? runSpeed : speed);


        //Если были нажаты клавиши то:
        if (z != 0 || x != 0)
        {
            CharacterMovement(Movement, x, z);
            // проверка на бег
            if (_isRunning)
            {
                Debug.Log("run");
                _unitModel.ChangeMoveState(2);
            }
            // шаг
            else
            {
                Debug.Log("walk");
                _unitModel.ChangeMoveState(1);
            }
        }
        // стоит
        else
        {
            Debug.Log("stand");
            _unitModel.ChangeMoveState(0);
        }


        //Если персонаж находится в воздухе, то имитируем силу тяжести.
        if (!_controller.isGrounded)
        {
            _controller.SimpleMove(_controller.velocity + Vector3.down * gravity * Time.deltaTime);
            //Debug.Log("jump");
            //_unitModel.ChangeMoveState(4);
        }
    }

    /// <summary>
    /// Метод движения персонажа
    /// </summary>
    /// <param name="Movement">Вектор движения</param>
    /// <param name="AxisX">Направление по оси X</param>
    /// <param name="AxisZ">Направление по оси Z</param>
    private void CharacterMovement(Vector3 Movement, float AxisX, float AxisZ)
    {
        //Добавляем скорость движения. Изменяем положение по осям x и z вектора3.
        Movement.x = AxisX;
        Movement.z = AxisZ;

        //Ограничиваем скорость движения по диагонали.
        Movement = Vector3.ClampMagnitude(Movement, runSpeed);

        //Кватернион для сохранения текущего вращения камеры.
        Quaternion TempCameraRotation = mainCamera.rotation;

        //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
        mainCamera.eulerAngles = new Vector3(0, mainCamera.eulerAngles.y, 0);

        //Переводим локальные координаты вектора движения игрока в глобальные.
        Movement = mainCamera.TransformDirection(Movement);

        //Возвращаем поворот камеры.
        mainCamera.rotation = TempCameraRotation;

        //Создаем кватернион направления движения, метод LookRotation() вычисляет кватернион который смотрит в направлении движения.
        Quaternion Direction = Quaternion.LookRotation(Movement);

        //Вращаем персонажа
        transform.rotation = Quaternion.Lerp(transform.rotation, Direction, rotateSpeed * Time.deltaTime);

        //Двигаем персонажа
        _controller.Move(Movement * Time.deltaTime);
    }

    //Проверка персонажа на Прыжок
    public bool JumpCheck()
    {
        return !_controller.isGrounded;
    }

    //Проверка персонажа на Бег
    public bool RunCheck()
    {
        return _isRunning;
    }

    //Проверка персонажа на Стояние на месте
    public bool StandCheck()
    {
        return _isStanding;
    }

}