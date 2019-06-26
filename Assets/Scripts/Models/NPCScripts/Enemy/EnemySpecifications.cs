using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace EnemySpace
{
    [CreateAssetMenu]
    public class EnemySpecifications : ScriptableObject
    {
        #region Основные характеристики персонажа

        [SerializeField] string type = string.Empty;
        public string Type { get { return type; } }

        [SerializeField] float hp = 0;
        internal float HP { get { return hp; } set { hp = value; } }

        [SerializeField] float speed = 0;
        public float Speed { get { return speed; } }

        [SerializeField] float runSpeed = 0;
        public float RunSpeed { get { return runSpeed; } }

        [SerializeField] bool rangeAttack = false;
        public bool RangeAttack { get { return rangeAttack; } }

        [SerializeField] float rangeDamage = 0;
        public float RangeDamage { get { return rangeDamage; } }

        [SerializeField] float rangeDistance = 0;
        public float RangeDistance { get { return rangeDistance; } }

        [SerializeField] float rangeAccuracy = 0;
        public float RangeAccuracy { get { return rangeAccuracy; } }

        [SerializeField] float shootSpeed = 0;
        public float ShootSpeed { get { return shootSpeed; } }

        [SerializeField] bool meleeAttack = false;
        public bool MeleeAttack { get { return meleeAttack; } }

        [SerializeField] float meleeDamage = 0;
        public float MeleeDamage { get { return meleeDamage; } }

        [SerializeField] float hitSpeed = 0;
        public float HitSpeed { get { return hitSpeed; } }

        [SerializeField] float meleeDistance = 0;
        public float MeleeDistance { get { return meleeDistance; } }

        [SerializeField] float viewDistance = 0;
        public float ViewDistance { get { return viewDistance; } }

        [SerializeField] float patrolDistance = 0;
        public float PatrolDistance { get { return patrolDistance; } }

        [SerializeField] float chasingTime = 0;
        public float ChasingTime { get { return chasingTime; } }

        #endregion

        #region Характеристикик состояний персонажа

        [Header("Состояния наносящие урон здоровью")]
        [Tooltip("Кровотечение")]
        [SerializeField] private bool Bleeding = false;
        [Tooltip("Урон от Кровотечение")]
        [SerializeField] private float _BleedingDamage = 0f;
        public float BleedingDamage { get { return _BleedingDamage; } }

        [Tooltip("Время Кровотечение")]
        [SerializeField] private float _BleedingTime = 0f;
        public float BleedingTime { get { return _BleedingTime; } }

        [Tooltip("Отравление")]
        [SerializeField] private bool Poison = false;
        [Tooltip("Урон от Отравления")]
        [SerializeField] private float _PoisonDamage = 0f;
        public float PoisonDamage { get { return _PoisonDamage; } }

        [Tooltip("Время Отравления")]
        [SerializeField] private float _PoisonTime = 0f;
        public float PoisonTime { get { return _PoisonTime; } }

        [Header("Состояния уменьшающие урон")]
        [Tooltip("Ослабление")]
        [SerializeField] private bool Weakness = false;
        [Tooltip("Процент уменьшения физического урон от Ослабления")]
        [SerializeField, Range(0f, 1f)] float _WeaknessDamageReduce = 0f;
        public float WeaknessDamageReduce { get { return _WeaknessDamageReduce; } }

        [Tooltip("Время ослабления")]
        [SerializeField] float _WeaknessTime = 0f;
        public float WeaknessTime { get { return _WeaknessTime; } }

        [Header("Состояния контроля")]
        [Tooltip("Нокдаун")]
        [SerializeField] private bool KnokedDown = false;
        [Tooltip("Время Нокдауна")]
        [SerializeField] float _KnokedDownTime = 0f;
        public float KnokedDownTime { get { return _KnokedDownTime; } }

        [Tooltip("Нокдаун")]
        [SerializeField] private bool Blinding = false;
        [Tooltip("Время Ослепления")]
        [SerializeField] float _BlindingTime = 0f;
        public float BlindingTime { get { return _BlindingTime; } }

        [Tooltip("Иммобилизации")]
        [SerializeField] private bool Immobilizing = false;
        [Tooltip("Время Иммобилизации")]
        [SerializeField] float _ImmobilizingTime = 0f;
        public float ImmobilizingTime { get { return _ImmobilizingTime; } }

        [Tooltip("Замедления")]
        [SerializeField] private bool Slowing = false;
        [Tooltip("Скорость замедленного персонажа")]
        [SerializeField] private float _SlowSpeed = 0f;
        [Tooltip("Время Замедления")]
        [SerializeField] float _SlowingTime = 0f;
        public float SlowingTime { get { return _SlowingTime; } }
        public float SlowSpeed { get { return _SlowSpeed; } }

        #endregion

        private Dictionary<string, bool> AllConditions;

        /// <summary>
        /// Возвращает список доступных статусов для персонажа
        /// </summary>
        /// <returns></returns>
        public List<string> GetCharacterConditionsList()
        {
            AllConditions = new Dictionary<string, bool>();

            #region Добавляем все состояния в словарь

            AllConditions.Add("Bleeding", Bleeding);
            AllConditions.Add("Poisoned", Poison);
            AllConditions.Add("Weakness", Weakness);
            AllConditions.Add("KnokedDown", KnokedDown);
            AllConditions.Add("Blinded", Blinding);
            AllConditions.Add("Immobilized", Immobilizing);
            AllConditions.Add("Slowed", Slowing);

            #endregion

            return (from c in AllConditions where c.Value != false select c.Key).ToList();
        }

    }
    
}
