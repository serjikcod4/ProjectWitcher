using System;
<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> origin/master
using UnityEngine;

public class MovementController : MonoBehaviour
{
<<<<<<< HEAD
    private CharacterController _controller;
    
    //Камера игрока
    [SerializeField] private Transform Camera;

    [SerializeField] private float speed = 5f;
=======
    protected CharacterController _controller;
    
    //Камера игрока
    [SerializeField] private Transform mainCamera;

    [SerializeField] protected float jumpSpeed = 8.0f;

    [SerializeField] protected float speed = 5f;
>>>>>>> origin/master

    [SerializeField] private float runSpeed = 10f;
    
    [SerializeField] private float rotateSpeed = 15f;
    
    [SerializeField] private float mouseSensivity = 4f;
    
    [SerializeField] private KeyCode runButton = KeyCode.LeftShift;

<<<<<<< HEAD
    [SerializeField] private float gravity = -9.81f;
=======
    [SerializeField] private float walkStaminaDrain = 0.1f;

    [SerializeField] private float runStaminaDrain = 0.3f;

    [SerializeField] protected float gravity = -9.81f;
>>>>>>> origin/master
    
    private bool _isRunning = false;
    private bool _isGrounded = false;
    private bool _isStanding = true;
    
<<<<<<< HEAD
    //Mock for unit data model
    private Unit unit = new Unit();
    
=======
>>>>>>> origin/master
    //Вектор движения персонажа.
    private Vector3 Movement = Vector3.zero;

    void Start()
    {
<<<<<<< HEAD
=======
        mainCamera = Camera.main.transform;
>>>>>>> origin/master
       _controller =  gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        //Нулевой вектор.
        Movement = Vector3.zero;

<<<<<<< HEAD
        _isRunning = Input.GetKey(runButton) && unit.StaminaChekForRun();
=======
        _isRunning = Input.GetKey(runButton) && Unit.InstanceUnit.StaminaChekForRun();
>>>>>>> origin/master
        
        // Check to see if the A or D key are being pressed
        var x = Input.GetAxis("Horizontal") * (_isRunning ? runSpeed : speed);

        // Check to see if the W or S key is being pressed.  
        var z = Input.GetAxis("Vertical") * (_isRunning ? runSpeed : speed);

        if (Math.Abs(z) > float.Epsilon)
            // Mock for Stamina Drain
            if (_isRunning)
            {
<<<<<<< HEAD
                unit.DrainStamina();
=======
                Unit.InstanceUnit.DrainStamina();
>>>>>>> origin/master
            }

        //Если были нажаты клавиши то:
        if (z != 0 || x != 0)
        {
            CharacterMovement(Movement, x, z);
        }

<<<<<<< HEAD
        //Если персонаж стоит на поверхности, то имитируем силу тяжести.
=======
        //Если персонаж находится в воздухе, то имитируем силу тяжести.
>>>>>>> origin/master
        if (!_controller.isGrounded)
        {
            _controller.SimpleMove(_controller.velocity + Vector3.down * gravity * Time.deltaTime);
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
<<<<<<< HEAD
        Quaternion TempCameraRotation = Camera.rotation;

        //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
        Camera.eulerAngles = new Vector3(0, Camera.eulerAngles.y, 0);

        //Переводим локальные координаты вектора движения игрока в глобальные.
        Movement = Camera.TransformDirection(Movement);

        //Возвращаем поворот камеры.
        Camera.rotation = TempCameraRotation;
=======
        Quaternion TempCameraRotation = mainCamera.rotation;

        //Задаем угол Эулера для камеры как координату оси Y, z и x оставляем 0.
        mainCamera.eulerAngles = new Vector3(0, mainCamera.eulerAngles.y, 0);

        //Переводим локальные координаты вектора движения игрока в глобальные.
        Movement = mainCamera.TransformDirection(Movement);

        //Возвращаем поворот камеры.
        mainCamera.rotation = TempCameraRotation;
>>>>>>> origin/master

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
