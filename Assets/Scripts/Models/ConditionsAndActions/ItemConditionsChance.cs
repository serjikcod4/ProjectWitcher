using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Interfaces
{
    /// <summary>
    /// Содержит данные о статусе который может передать объект и шансах его срабатывания
    /// </summary>
    public struct CurrentCondition
    {
        public string Name { get; private set; }

        public float Chance { get; private set; }

        public CurrentCondition(string Name, float Chance)
        {
            this.Name = Name;
            this.Chance = Chance;
        }
    }

    /// <summary>
    /// Содержит данные всех статусах, которые может передать объект и шансах их срабатывания
    /// </summary>
    [CreateAssetMenu]
    public class ItemConditionsChance : ScriptableObject
    {
        private CurrentCondition[] ItemConditions = new CurrentCondition[8];

        #region Шансы статусов предмета

        [SerializeField, Range(0f, 1f)] float Slow = 0f;
        [SerializeField, Range(0f, 1f)] float Immobilized = 0f;
        [SerializeField, Range(0f, 1f)] float Blinded = 0f;
        [SerializeField, Range(0f, 1f)] float Bleeding = 0f;
        [SerializeField, Range(0f, 1f)] float KnokedDown = 0f;
        [SerializeField, Range(0f, 1f)] float Poisoned = 0f;
        [SerializeField, Range(0f, 1f)] float Weak = 0f;

        #endregion
        
        public void SetItemConditionsChance()
        {
            ItemConditions = new CurrentCondition[]
            {
                new CurrentCondition("Bleeding", Bleeding),
                new CurrentCondition("Poisoned", Poisoned),
                new CurrentCondition("Weakness", Weak),
                new CurrentCondition("KnokedDown", KnokedDown),
                new CurrentCondition("Blinded", Blinded),
                new CurrentCondition("Immobilized", Immobilized),
                new CurrentCondition("Slowed", Slow),
                
            };
        }

        /// <summary>
        /// Получить все активные статусы на объекте
        /// </summary>
        /// <returns></returns>
        public CurrentCondition[] GetCurrentItemConditions()
        {
            return (from Con in ItemConditions where Con.Chance != 0 select Con).ToArray();
        }
    }
}