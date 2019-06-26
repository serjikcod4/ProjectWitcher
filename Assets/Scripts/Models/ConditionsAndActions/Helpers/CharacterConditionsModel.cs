using EnemySpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers
{
    /// <summary>
    /// Содержит данные о характеристиках статусов персонажа
    /// </summary>
    public struct CharacterConditionsSpecifications
    {
        public float BleedingDamage { get; private set; }
        public float BleedingTime { get; private set; }
        public float PoisonDamage { get; private set; }
        public float PoisonTime { get; private set; }
        public float WeaknessDamageReduce { get; private set; }
        public float WeaknessTime { get; private set; }
        public float KnokedDownTime { get; private set; }
        public float BlindingTime { get; private set; }
        public float ImmobilizingTime { get; private set; }
        public float SlowingTime { get; private set; }
        public float SlowSpeed { get; private set; }

        /// <summary>
        /// Создает модель данных о характеристиках персонажа
        /// </summary>
        /// <param name="Specifications">Все данные о персонаже</param>
        public CharacterConditionsSpecifications(EnemySpecifications Specifications)
        {
            BleedingDamage = Specifications.BleedingDamage;
            BleedingTime = Specifications.BleedingTime;
            PoisonDamage = Specifications.PoisonDamage;
            PoisonTime = Specifications.PoisonTime;
            WeaknessDamageReduce = Specifications.WeaknessDamageReduce;
            WeaknessTime = Specifications.WeaknessTime;
            KnokedDownTime = Specifications.KnokedDownTime;
            BlindingTime = Specifications.BlindingTime;
            ImmobilizingTime = Specifications.ImmobilizingTime;
            SlowingTime = Specifications.SlowingTime;
            SlowSpeed = Specifications.SlowSpeed;
        }
    }
}
