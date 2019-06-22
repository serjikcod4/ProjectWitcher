using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Interfaces
{
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
        [SerializeField, Range(0f, 1f)] float Stunned = 0f;
        [SerializeField, Range(0f, 1f)] float Poisoned = 0f;
        [SerializeField, Range(0f, 1f)] float Weak = 0f;

        #endregion
        
        public void SetItemConditionsChance()
        {
            ItemConditions = new CurrentCondition[]{

                new CurrentCondition("Slowed", Slow),
                new CurrentCondition("Immobilized", Immobilized),
                new CurrentCondition("Blinded", Blinded),
                new CurrentCondition("Bleeding", Bleeding),
                new CurrentCondition("KnokedDown", KnokedDown),
                new CurrentCondition("Stunned", Stunned),
                new CurrentCondition("Poisoned", Poisoned),
                new CurrentCondition("Weak", Weak),
            };
        }

        public CurrentCondition[] GetCurrentItemConditions()
        {
            return (from Con in ItemConditions where Con.Chance != 0 select Con).ToArray();
        }
    }
}