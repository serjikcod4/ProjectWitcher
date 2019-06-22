using EnemySpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.ConditionsAndActions
{
    [CreateAssetMenu]
    public class EnemyConditions: BaseConditions
    {
        /// <summary>
        /// Кровотечение
        /// </summary>
        /// <param name="HealthPoints"></param>
        /// <param name="deltaTime"></param>
        public override void Bleed(ref EnemySpecifications spec, float deltaTime)
        {
            BleedingTimer += deltaTime;
            spec.HP -= BleedingDamage;

            Debug.Log("Enemy Health:" +spec.HP);

            if (BleedingTimer >= BleedingTime)
            {
                //Меняем статус состояния, сбрасываем таймер, отписываемся от метода в событии апдейта статусов.
                Conditions.ChangeConditionStatus("Bleeding", false);
                BleedingTimer = 0;
                ConditionsUpdateEvent -= Bleed;
            }
        }
    }
}
