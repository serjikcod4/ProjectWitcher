using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers
{
    /// <summary>
    /// Базовая модель персонажа
    /// </summary>
    public struct BaseCharacterModel
    {
        public float Health;
        public float Speed;
        public float RunSpeed;

        public BaseCharacterModel(float Health, float Speed,float RunSpeed)
        {
            this.Health = Health;
            this.Speed = Speed;
            this.RunSpeed = RunSpeed;
        }
    }
}
