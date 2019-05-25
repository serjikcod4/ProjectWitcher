﻿using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    //Сериализованное поле для ссылки на игрока.
    [SerializeField] private GameObject Player;

    [SerializeField] private float CameraMinDistance = 5.0f;

    [SerializeField] private float CameraMaxDistance = 15f;

    [SerializeField] private float CameraZoomSpeed = 300f;

    [SerializeField] private float AxisX_MouseSensivity = 3f;

    [SerializeField] private float AxisY_MouseSensivity = 3f;

<<<<<<< HEAD
=======
    [SerializeField] public float CameraObstacleAvoidSpeed = 5;

    [SerializeField] public float CameraReturnSpeed = 10;

>>>>>>> origin/master
    //Угол вращения камеры по оси Y.
    private float RotationY = 0;

    //Угол вращения камеры по оси X.
    private float RotationX = 0;

    //Приближение камеры.
    private float Zoom;

    //Вектор расстояния между игроком и камерой.
    private Vector3 Offset;

<<<<<<< HEAD
    void Start()
    {
        //Присваимем переменной угол по оси y;
        RotationY = transform.eulerAngles.y;

        //Вычисляем расстояние между камерой и игроком.
        Offset = Player.transform.position - transform.position;
=======
    //Расстояние до камеры с препятствием.
    private Vector3 ObstacleOffset;

    //Стартовое расстояние до камеры.
    private Vector3 StartCameraDistance;

    //Флаг для наличия препятствий
    private bool CameraObstacle;

    //Маска
    public LayerMask Mask;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        //Вычисляем стартовое положение камеры.
        StartCameraDistance = Player.transform.position + (-Player.transform.forward * (CameraMinDistance + CameraMaxDistance / 2));

        //Задаем начальное расстояние.
        Offset = Player.transform.position - StartCameraDistance;
>>>>>>> origin/master
    }

    void LateUpdate()
    {
<<<<<<< HEAD
        float Zoom = Input.GetAxis("Mouse ScrollWheel");

        //Управление зумом камеры.
        if (Zoom != 0)
        {
            Offset.z -= (1 * Input.GetAxis("Mouse ScrollWheel")) * CameraZoomSpeed * Time.deltaTime;
=======
        //Получаем значения колесика мыши
        Zoom = Input.GetAxis("Mouse ScrollWheel");

        //Получаем значения движения мыши по осям X и Y
        RotationY += Input.GetAxis("Mouse X") * (AxisX_MouseSensivity * 2);
        RotationX += -Input.GetAxis("Mouse Y") * (AxisY_MouseSensivity * 2);
>>>>>>> origin/master

        //Ограничиваем движение камеры по оси X
        RotationX = Mathf.Clamp(RotationX, 0, 70);

        //Преобразуем угол Еулера в кватернион.
        Quaternion Rotation = Quaternion.Euler(RotationX, RotationY, 0);

<<<<<<< HEAD
        RotationY += Input.GetAxis("Mouse X") * (AxisX_MouseSensivity * 2);
        RotationX += -Input.GetAxis("Mouse Y") * (AxisY_MouseSensivity * 2);

        //Ограничиваем движение камеры
        RotationX = Mathf.Clamp(RotationX, 0, 70);
=======
        //Проверяем коллизию
        CollisionCheck(Player.transform.position, Rotation);

        //Двигаем камеру
        CameraMove(CameraObstacle, Rotation);

        //Камера все время повернута в сторону игрока.
        transform.LookAt(Player.transform);
    }

    /// Метод для передвижения камеры
    /// </summary>
    /// <param name="CameraObstacle">Флаг препятствия</param>
    /// <param name="Rotation">Текущее вращение камеры</param>
    private void CameraMove(bool CameraObstacle, Quaternion Rotation)
    {
        switch (CameraObstacle)
        {
            case true:

                transform.position = Vector3.Lerp(transform.position, ObstacleOffset, CameraObstacleAvoidSpeed * Time.deltaTime);
                break;

            case false:

                transform.position = Vector3.Lerp(transform.position, Player.transform.position - (Rotation * Offset), CameraReturnSpeed * Time.deltaTime);
                
                //Управляем зумом
                CameraZoom(CameraMinDistance, CameraMaxDistance);
                break;
        }
    }

    /// <summary>
    /// Метод проверки коллизий
    /// </summary>
    /// <param name="PlayerPosition">Позиция игрока</param>
    /// <param name="rotation">Вращение камеры</param>
    private void CollisionCheck(Vector3 PlayerPosition, Quaternion rotation)
    {
        RaycastHit Ray;
        
        Debug.DrawLine(PlayerPosition, (PlayerPosition - (rotation * Offset)), Color.yellow);

        //Проверяем столкновение луча с препятствием
        if (Physics.Linecast(PlayerPosition, (PlayerPosition - (rotation * Offset)), out Ray, Mask))
        {
            CameraObstacle = true;

            ObstacleOffset = Ray.point;

            Debug.DrawLine(Player.transform.position, ObstacleOffset, Color.red);

            return;
        }
>>>>>>> origin/master

        CameraObstacle = false;
    }

<<<<<<< HEAD
        //Задаем позицию камеры как Vector3 игрока минус оффсет умноженное угол вращения.
        transform.position = Player.transform.position - (rotation * Offset);

        //Камера все время повернута в сторону игрока.
        transform.LookAt(Player.transform);
=======
    /// <summary>
    /// Метод "зума" камеры
    /// </summary>
    /// <param name="MinDistance">Минимальная дистанция</param>
    /// <param name="MaxDistance">Максимальная дистанция</param>
    private void CameraZoom(float MinDistance, float MaxDistance)
    {
        //Управление зумом камеры.
        if (Zoom != 0)
        {
            Offset.z -= (1 * Input.GetAxis("Mouse ScrollWheel")) * CameraZoomSpeed * Time.deltaTime;

            //Ограничиваем приближение\отдаление камеры.
            Offset.z = Mathf.Clamp(Offset.z, MinDistance, MaxDistance);
        }
>>>>>>> origin/master
    }
}
