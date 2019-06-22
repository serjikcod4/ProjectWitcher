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
    public abstract class BaseConditions: ScriptableObject
    {
        /// <summary>
        /// Коллекция содержащая все доступные персонажу статусы
        /// </summary>
        public ConditionsCollection Conditions { get; private set; }

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
        
        #region Параметры состояний

        [Header("Состояния наносящие урон здоровью")]
        [Tooltip("Урон от Кровотечение")]
        [SerializeField] private float _BleedingDamage = 0f;
        public float BleedingDamage { get { return _BleedingDamage; } }

        [Tooltip("Время Кровотечение")]
        [SerializeField] private float _BleedingTime = 0f;
        public float BleedingTime { get { return _BleedingTime; } }

        [Tooltip("Урон от Отравления")]
        [SerializeField] private float _PoisonDamage = 0f;
        public float PoisonDamage { get { return _PoisonDamage; } }

        [Tooltip("Время Отравления")]
        [SerializeField] private float _PoisonTime = 0f;
        public float PoisonTime { get { return _PoisonTime; } }

        [Header("Состояния уменьшающие урон")]
        [Tooltip("Процент уменьшения физического урон от Ослабления")]
        [SerializeField, Range(0f, 1f)] float _WeaknessDamageReduce = 0f;
        public float WeaknessDamageReduce { get { return _WeaknessDamageReduce; } }

        [Tooltip("Время ослабления")]
        [SerializeField] float _WeaknessTime = 0f;
        public float WeaknessTime { get { return _WeaknessTime; } }

        [Header("Состояния контроля")]
        [Tooltip("Время Нокдауна")]
        [SerializeField] float _KnokedDownTime = 0f;
        public float KnokedDownTime { get { return _KnokedDownTime; } }

        [Tooltip("Время Ослепления")]
        [SerializeField] float _BlindingTime = 0f;
        public float BlindingTime { get { return _BlindingTime; } }

        [Tooltip("Время Иммобилизации")]
        [SerializeField] float _ImmobilizingTime = 0f;
        public float ImmobilizingTime { get { return _ImmobilizingTime; } }

        [Tooltip("Время Замедления")]
        [SerializeField] float _SlowingTime = 0f;
        public float SlowingTime { get { return _SlowingTime; } }

        #endregion

        #region Таймеры

        protected internal float BleedingTimer = 0f;
        private float PoisonTimer = 0f;
        private float WeaknessTimer = 0f;
        private float KnokedDownTimer = 0f;
        private float BlindingTimer = 0f;
        private float ImmobilizingTimer = 0f;
        private float SlowingTimer = 0f;

        #endregion

        /// <summary>
        /// Добавляем состояния в коллекцию
        /// </summary>
        public virtual void SetBaseConditions()
        {
            #region Добавляем все состояния.
            
            Conditions = new ConditionsCollection()
            {
                new Condition("Alive", true),
                new Condition("Slowed", false),
                new Condition("Immobilized", false),
                new Condition("Blinded", false),
                new Condition("Bleeding", false),
                new Condition("KnokedDown", false),
                new Condition("Stunned", false),
                new Condition("Poisoned", false),
                new Condition("Weak", false),

            };

            Conditions.SetPropertyEventMethod(ConditionStatusHasChanged);

            #endregion
        }
        
        #region Логика Состояний

        public virtual void Slowing(ref EnemySpecifications spec, float deltaTime)
        {

        }

        public virtual void Immobilizing(ref EnemySpecifications spec, float deltaTime)
        {

        }

        public virtual void Blinding(ref EnemySpecifications spec, float deltaTime)
        {

        }

        public virtual void Bleed(ref EnemySpecifications spec, float deltaTime)
        {

        }
        
        public virtual void KnokingDown(ref EnemySpecifications spec, float deltaTime)
        {

        }

        public virtual void Stuning(ref EnemySpecifications spec, float deltaTime)
        {

        }

        public virtual void Poison(ref EnemySpecifications spec, float deltaTime)
        {

        }

        public virtual void Weak(ref EnemySpecifications spec, float deltaTime)
        {

        }

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
                    return;
                }

                else
                {
                    switch (Args.ConditionName)
                    {
                        case "Slowed":
                            ConditionsUpdateEvent += Slowing;
                            break;

                        case "Immobilized":
                            ConditionsUpdateEvent += Immobilizing;
                            break;

                        case "Blinded":
                            ConditionsUpdateEvent += Blinding;
                            break;

                        case "Bleeding":
                            ConditionsUpdateEvent += Bleed;
                            break;

                        case "KnokedDown":
                            ConditionsUpdateEvent += KnokingDown;
                            break;

                        case "Stunned":
                            ConditionsUpdateEvent += Stuning;
                            break;

                        case "Poisoned":
                            ConditionsUpdateEvent += Poison;
                            break;

                        case "Weak":
                            ConditionsUpdateEvent += Weak;
                            break;
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
        public void ConditionsUpdateStart(ref EnemySpecifications spec, float deltaTime)
        {
            ConditionsUpdateEvent?.Invoke(ref spec, deltaTime);
        }

        #endregion
    }
}
