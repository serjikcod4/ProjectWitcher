﻿using UnityEngine;
using UnityEditor;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.ConditionsAndActions.Helpers;

namespace EnemySpace
{
    public class EnemyFightController
    {
        /// <summary>
        /// Делегат для перехода в состояние погони
        /// </summary>
        /// <param name="unitName"></param>
        public delegate void AttackToChase(string unitName);
        public static event AttackToChase AttackToChaseEvent;
        /// <summary>
        /// Локальные копии задействованных компонентов
        /// </summary>
        EnemyMove move; //Локальная копия класса движения движения
        Transform enemyTransform; //Локальная копия трансформа врага
        MeshRenderer gun; //МешРендерер стрелкового оружия
        MeshRenderer knife; //МешРендерер оружия ближнего боя
        Transform gunBarrelEnd; //Трансформ точки стрельбы
        AudioSource gunShotSound; //Звук стрельбы
        Ray shootRay; //Луч для проверки попадания по объекту
        RaycastHit hit; //Точка попадания луча
        LineRenderer shootLine; //Визуальное отображение выстрела

        /// <summary>
        /// Локальные переменные, хранящие в себе состояния и значения параметров
        /// </summary>
        float priorityDistance; //Приоритетная дистанция атаки - выбирается автоматически при создании объекта, исходя из его типа
        float alternativeDistance; //Альтернативная дистанция атаки - для альтернативного режима атаки
        float switchDistance; //Дистанция на которой происходит переключение режимов атаки
        bool switchMode = true; //режим атаки (true - приоритетный, false - альтернативный)
        float currentAttackDistance; //текущая дистанция атаки
        float runSpeed; //Скорость бега
        float boostSpeed; //Скорость рывка - используется для быстрого сокращения/разрыва дистанции
        float timer; //Основной задейсвованный таймер (Скорость стрельбы)
        float reactionTimer; //таймер принятия решения врагом
        float reaction = 0.2f; //время принятия решения (между выбором цели и выстрелом)
        bool choseAim = false; //Флаг состояния выбора цели
        Vector3 reactionAim; //точка, в которую будет произведен выстрел
        float rangeDamage; //Дистанционный урон
        float rangeAccuracy; //Дистанционная меткость
        float shootSpeed; //Скорость стрельбы
        float meleeDamage; //Урон ближнего боя
        float hitSpeed; //Скорость ближнего боя
        float effectsDisplayTime = 0.1f; //Время действия визуальных эффектов
        int meleeHitCount = 0; //счетчик нанесенных ударов ближнего боя - для спец способности
        bool specialAbility = false; //флаг включения\отключения спец способности
        float hitChance; //Шанс попасть при стрельбе
        float missChance; //Шанс промахнуться при стрельбе

        /// <summary>
        /// Вспомогательные переменные для отслеживания меткости стрельбы
        /// </summary>
        int shotCount = 1; 
        int hitCount = 1;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="move"></param>
        /// <param name="enemyTransform"></param>
        /// <param name="gun"></param>
        /// <param name="knife"></param>
        /// <param name="gunBarrelEnd"></param>
        /// <param name="shootLine"></param>
        /// <param name="priorityDistance"></param>
        /// <param name="alternativeDistance"></param>
        /// <param name="runSpeed"></param>
        /// <param name="rangeDamage"></param>
        /// <param name="rangeAccuracy"></param>
        /// <param name="shootSpeed"></param>
        /// <param name="meleeDamage"></param>
        /// <param name="hitSpeed"></param>
        /// <param name="gunShotSound"></param>
        public EnemyFightController(EnemyMove move, Transform enemyTransform, MeshRenderer gun, MeshRenderer knife, Transform gunBarrelEnd, LineRenderer shootLine, float priorityDistance, float alternativeDistance, float rangeDamage, float rangeAccuracy, float shootSpeed, float meleeDamage, float hitSpeed, AudioSource gunShotSound)
        {
            this.move = move;
            this.enemyTransform = enemyTransform;
            this.shootLine = shootLine;
            this.priorityDistance = priorityDistance;
            this.alternativeDistance = alternativeDistance;
            switchDistance = Mathf.Abs(priorityDistance - alternativeDistance) / 2;
            currentAttackDistance = priorityDistance;
            this.rangeDamage = rangeDamage;
            this.rangeAccuracy = rangeAccuracy;
            this.shootSpeed = shootSpeed;
            this.gun = gun;
            this.knife = knife;
            this.gunBarrelEnd = gunBarrelEnd;
            this.gunShotSound = gunShotSound;
            this.meleeDamage = meleeDamage;
            this.hitSpeed = hitSpeed;
            hitChance = rangeAccuracy;
            missChance = 10 - rangeAccuracy;
        }

        /// <summary>
        /// Основной вызываемый из контроллера врага метод - отвечает за режим боя
        /// </summary>
        /// <param name="archrival"></param>
        /// <param name="deltaTime"></param>
        public void Fight(GameObject archrival, float deltaTime)
        {
            timer += deltaTime;
            reactionTimer += deltaTime;
            SpecialAbilityActivator();
            if (timer > effectsDisplayTime)
                DisableEffects();
            if (specialAbility)
            {
                SpecialAbility(archrival.transform.position);
            }
            else
            {
                //Рассчет расстояния до потивника
                float distance = Mathf.Sqrt(Mathf.Pow(archrival.transform.position.x - enemyTransform.position.x, 2) + Mathf.Pow(archrival.transform.position.y - enemyTransform.position.y, 2) + Mathf.Pow(archrival.transform.position.z - enemyTransform.position.z, 2));
                if (distance > currentAttackDistance)
                {
                    //Debug.Log("MoveToPlayer");
                    move.Continue();
                    move.Move(archrival.transform.position, Speed.Walk);
                    move.Rotate(RotateDirection(archrival.transform.position));
                    if (timer > 15f)
                    {
                        AttackToChaseEvent(enemyTransform.name);
                    }
                    if (switchMode)
                    {
                        gun.enabled = true;
                        knife.enabled = false;
                        //Debug.Log("MoveToPlayer");
                        move.Continue();
                        move.Move(archrival.transform.position, Speed.Run);
                        move.Rotate(RotateDirection(archrival.transform.position));
                    }
                    else
                    {
                        gun.enabled = false;
                        knife.enabled = true;
                        //Debug.Log("ThrowToPlayer");
                        move.Continue();
                        move.Move(archrival.transform.position, Speed.Throw);
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (distance >= switchDistance)
                        {
                            timer = 0f;
                            switchMode = !switchMode;
                            currentAttackDistance = priorityDistance;
                        }
                    }

                }
                else if (distance <= currentAttackDistance)
                {
                    move.Stop();
                    if (switchMode)
                    {
                        gun.enabled = true;
                        knife.enabled = false;
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (timer >= shootSpeed)
                        {
                            if (choseAim)
                            {
                                if(reactionTimer > reaction)
                                {
                                    RangeAttack(reactionAim);
                                    choseAim = false;
                                }
                            }
                            else
                            {
                                //Выбор цели и запуск таймера реакции
                                reactionTimer = 0f;
                                reactionAim = ShootDirection(archrival.transform.position, rangeAccuracy);
                                choseAim = true;
                            }
                        }
                        if (distance <= switchDistance)
                        {
                            timer = 0f;
                            switchMode = !switchMode;
                            currentAttackDistance = alternativeDistance;
                        }
                    }
                    else
                    {
                        gun.enabled = false;
                        knife.enabled = true;
                        move.Rotate(RotateDirection(archrival.transform.position));
                        if (timer >= hitSpeed)
                        {
                            MeleeAttack();
                            meleeHitCount++;
                        }
                    }
                }
            }            
        }
        /// <summary>
        /// Метод выбора направления для вращения врага
        /// </summary>
        /// <param name="archrival"></param>
        /// <returns></returns>
        private Vector3 RotateDirection(Vector3 archrival)
        {
            Vector3 currentDirection = new Vector3(archrival.x - enemyTransform.position.x, 0, archrival.z - enemyTransform.position.z);
            return currentDirection;
        }

        /// <summary>
        /// Метод расчета направления для стрельбы
        /// </summary>
        /// <param name="archrival"></param>
        /// <param name="rangeAccuracy"></param>
        /// <returns></returns>
        private Vector3 ShootDirection(Vector3 archrival, float rangeAccuracy)
        {
            if(hitChance == 0 && missChance == 0)
            {
                hitChance = rangeAccuracy;
                missChance = 10 - rangeAccuracy;
            }
            float isHit = Random.Range(-missChance, hitChance);
            Vector3 currentDirection;
            Vector3 shootDirection = new Vector3(archrival.x - enemyTransform.position.x, archrival.y - enemyTransform.position.y, archrival.z - enemyTransform.position.z);
            if (isHit >= 0 && isHit < hitChance)
            {
                int variant = Random.Range(-1, 1);
                currentDirection = new Vector3(shootDirection.x + variant, shootDirection.y, shootDirection.z + variant); ;
                //Debug.LogWarning("ShotInPlayer");
                hitChance--;
            }
            else
            {
                int miss = Random.Range(-5, -3);
                int sideMiss = Random.Range(0, 2);
                if (sideMiss == 0)
                    miss = -miss;
                currentDirection = new Vector3(shootDirection.x + miss, shootDirection.y, shootDirection.z + miss);
                //Debug.LogWarning("MissShot");
                missChance--;
            }
            return currentDirection;
        }

        /// <summary>
        /// Метод Стрельбы
        /// </summary>
        /// <param name="archrival"></param>
        private void RangeAttack(Vector3 archrival)
        {
            //Debug.Log("ShotCount: " + shotCount);
            //Debug.Log("HitCount: " + hitCount);
            timer = 0f;
            gunShotSound.Play();
            float currentDamage = rangeDamage;
            shootLine.useWorldSpace = true;
            shootLine.enabled = true;
            shootLine.SetPosition(0, gunBarrelEnd.position);
            shootLine.SetPosition(1, archrival + gunBarrelEnd.position);

            shotCount++;
            shootRay.origin = gunBarrelEnd.position;
            shootRay.direction = archrival;
            //Debug.Log(shootRay);

            if (Physics.Raycast(shootRay, out hit, 100f))
            {
                hitCount++;
                //Debug.LogError("Hit: " + hit.collider.name);
                SetDamage(hit.collider.GetComponent<IDamageable>(), currentDamage);
            }
        }

        /// <summary>
        /// Метод ближнего боя (в будущем)
        /// </summary>
        private void MeleeAttack()
        {
            timer = 0f;
        }

        /// <summary>
        /// Метод отключения визуальных эффектов
        /// </summary>
        private void DisableEffects()
        {
            shootLine.enabled = false;
            shootLine.useWorldSpace = false;
        }

        /// <summary>
        /// Метод нанесения урона цели
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="damage"></param>
        private void SetDamage(IDamageable obj, float damage)
        {
            if(obj != null)
            {
                //Debug.Log("Damage: " + damage);
                obj.TakeDamage(damage);
            }
        }

        /// <summary>
        /// Активатор спец способности
        /// </summary>
        private void SpecialAbilityActivator()
        {
            if(meleeHitCount == 3)
            {
                specialAbility = true;
            }
        }

        /// <summary>
        /// Метод выполнения специальной способности
        /// </summary>
        /// <param name="archrival"></param>
        private void SpecialAbility(Vector3 archrival)
        {
            //Debug.Log("Special");
            switchMode = true;
            currentAttackDistance = priorityDistance;
            gun.enabled = true;
            knife.enabled = false;
            Vector3 direction = RotateDirection(archrival);
            Vector3 delta = new Vector3(archrival.x - enemyTransform.position.x, archrival.y - enemyTransform.position.y, archrival.z - enemyTransform.position.z);
            Vector3 distancePointDirection = new Vector3(enemyTransform.position.x - delta.x * priorityDistance, enemyTransform.position.y - delta.y, enemyTransform.position.z - delta.z * priorityDistance);
            Vector3 distancePoint = new Vector3(distancePointDirection.x, distancePointDirection.y, distancePointDirection.z);
            move.Continue();
            move.Move(distancePoint, Speed.Throw);
            //enemyTransform.Translate(distancePoint);
            float distance = Mathf.Sqrt(Mathf.Pow(archrival.x - enemyTransform.position.x, 2) + Mathf.Pow(archrival.y - enemyTransform.position.y, 2) + Mathf.Pow(archrival.z - enemyTransform.position.z, 2));
            if (distance >= switchDistance)
            {
                specialAbility = false;
                meleeHitCount = 0;
                move.Stop();
                move.Rotate(RotateDirection(archrival));
                //Debug.LogError("FinishAbility");
            }
        }
    }
}
