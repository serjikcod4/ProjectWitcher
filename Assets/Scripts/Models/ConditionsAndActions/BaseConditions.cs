using Assets.Scripts.Models.ConditionsAndActions.Helpers;
using Assets.Scripts.Models.ConditionsAndActions.Helpers.Components;
using EnemySpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.ConditionsAndActions
{
    /// <summary>
    /// Содержит информацию о доступных персонажу статусах и отслеживает их выполнение
    /// </summary>
    public class BaseConditions
    {
        /// <summary>
        /// Коллекция содержащая все доступные персонажу статусы
        /// </summary>
        public ConditionsCollection Conditions { get; private set; }

        /// <summary>
        /// Содержит все характеристики состояний персонажа
        /// </summary>
        private CharacterConditionsSpecifications ConditionsSpecifications;

        /// <summary>
        /// Содержит все методы состояний
        /// </summary>
        private Dictionary<string, ConditionsUpdate> ConditionsMethods;

        /// <summary>
        /// Содержит таймеры для всех состояний
        /// </summary>
        private Dictionary<string, float> ConditionTimers;
        
        #region События

        /// <summary>
        /// Событие запускающее активные статусы
        /// </summary>
        protected internal event ConditionsUpdate ConditionsUpdateEvent;
        
        private bool ActiveConditions;

        /// <summary>
        /// Свойство показывает, если на персонаже активные статусы требующие апдейта
        /// </summary>
        public bool HasActiveConditions
        {
            get
            {
                return ActiveConditions;
            }

            private set
            {
                ActiveConditions = value;
            }
        }

        #endregion

        #region Таймеры

        private float BleedingTimer = 0f;
        private float SlowingTimer = 0f;

        #endregion

        public BaseConditions(List<string> ConditionsList, CharacterConditionsSpecifications ConditionsSpecifications)
        {
            #region Добавляем все состояния в коллекцию

            Conditions = new ConditionsCollection(ConditionsList)
            {
                new Condition("Alive", true)
            };

            Conditions.SetPropertyEventMethod(ConditionStatusHasChanged);

            #endregion

            //Получаем ссылку на характеристики состояний
            this.ConditionsSpecifications = ConditionsSpecifications;

            #region Заполняем словарь с методами состояний

            ConditionsMethods = new Dictionary<string, ConditionsUpdate>();

            ConditionsMethods.Add("Bleeding", Bleed);
            ConditionsMethods.Add("Slowed", Slowing);


            #endregion

            #region Заполняем словарь с таймерами

            ConditionTimers = new Dictionary<string, float>();

            ConditionTimers.Add("Bleeding", BleedingTimer);
            ConditionTimers.Add("Slowed", SlowingTimer);

            #endregion
        }

        #region Логика Состояний

        /// <summary>
        /// Кровотечение
        /// </summary>
        /// <param name="CharacterModel">Модель персонажа</param>
        /// <param name="deltaTime">Время</param>
        public void Bleed(ref BaseCharacterModel CharacterModel, ref EnemySpecifications enemySpecifications, float deltaTime)
        {
            ConditionTimers["Bleeding"] += deltaTime;
            CharacterModel.Health -= ConditionsSpecifications.BleedingDamage;

            Debug.Log($"!!!BLEED!!! Health:{CharacterModel.Health.ToString("0")} Condition Time Left: {(ConditionsSpecifications.BleedingTime - ConditionTimers["Bleeding"]).ToString("0.0")}");

            if(ConditionTimers["Bleeding"] >= ConditionsSpecifications.BleedingTime)
            {
                ConditionTimers["Bleeding"] = 0;
                Conditions.ChangeConditionStatus("Bleeding", false);
                ConditionsUpdateEvent -= Bleed;
            }
        }

        /// <summary>
        /// Замедление
        /// </summary>
        /// <param name="CharacterModel">Модель персонажа</param>
        /// <param name="deltaTime">Время</param>
        public void Slowing(ref BaseCharacterModel CharacterModel, ref EnemySpecifications enemySpecifications, float deltaTime)
        {
            ConditionTimers["Slowed"] += deltaTime;
            CharacterModel.Speed = ConditionsSpecifications.SlowSpeed;
            CharacterModel.RunSpeed = ConditionsSpecifications.SlowSpeed;

            Debug.Log($"!!!SLOW!!! Condition Time Left: {(ConditionsSpecifications.SlowingTime - ConditionTimers["Slowed"]).ToString("0.0")}");

            if (ConditionTimers["Slowed"] >= ConditionsSpecifications.SlowingTime)
            {
                ConditionTimers["Slowed"] = 0;
                Conditions.ChangeConditionStatus("Slowed", false);
                ConditionsUpdateEvent -= Slowing;

                CharacterModel.Speed = enemySpecifications.Speed;
                CharacterModel.RunSpeed = enemySpecifications.RunSpeed;
            }
        }

        #region TO DO 

        //public void Immobilizing(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Blinding(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}
        
        //public void KnokingDown(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Stuning(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Poison(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        //public void Weak(ref BaseCharacterModel CharacterModel, float deltaTime)
        //{

        //}

        #endregion

        #endregion

        #region Изменение статуса состояний

        /// <summary>
        /// Вызывается каждый раз при изменении статуса состояния
        /// </summary>
        /// <param name="Args">Содержит информацию об изменившемся статусе</param>
        private void ConditionStatusHasChanged(ConditionArgs Args)
        {
            if(Args.ConditionName == "Alive" & Args.ConditionStatus == false)
            {
                CharacterDeathStatus();
                return;
            }

            else
            {
                if(Args.ConditionStatus == false)
                {
                    ActiveConditionsCheck();
                    return;
                }

                else
                {
                    if(!Conditions.CurrentConditionStatus(Args.ConditionName))
                    {
                        ConditionsUpdateEvent += ConditionsMethods[Args.ConditionName];
                    }
                    else
                    {
                        ConditionTimers[Args.ConditionName] = 0;
                    }

                    ActiveConditionsCheck();
                }
            }
        }

        /// <summary>
        /// Проверяет активные статусы на персонаже
        /// </summary>
        private void ActiveConditionsCheck()
        {
            for (int i = 1; i < Conditions.Count; i++)
            {
                if(Conditions[i].StatusChanged == true)
                {
                    ActiveConditions = true;
                    return;
                }
            }
            ActiveConditions = false;
        }

        /// <summary>
        /// В случае смерти персонажа отписываем от всех методов, меняем коллекцию статусов
        /// </summary>
        private void CharacterDeathStatus()
        {
            if (ConditionsUpdateEvent != null)
            {
                foreach (var item in ConditionsUpdateEvent.GetInvocationList())
                {
                    ConditionsUpdateEvent -= item as ConditionsUpdate;
                }
            }

            Conditions = new ConditionsCollection()
            {
                new Condition("Alive", false)
            };

        }

        #endregion

        #region Запуск состояний

        /// <summary>
        /// Запускает выполнение всех активных статусов
        /// </summary>
        /// <param name="spec">Модель данных противника</param>
        /// <param name="deltaTime">Время</param>
        public void ConditionsUpdateStart(ref BaseCharacterModel CharacterModel, ref EnemySpecifications enemySpecifications, float deltaTime)
        {
            ConditionsUpdateEvent?.Invoke(ref CharacterModel, ref enemySpecifications, deltaTime);
        }

        #endregion
    }
}
