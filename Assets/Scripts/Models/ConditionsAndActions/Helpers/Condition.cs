using Assets.Scripts.Models.ConditionsAndActions.Helpers.Components;
using Assets.Scripts.Models.ConditionsAndActions.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers
{
    /// <summary>
    /// Представляет отдельный статус персонажа
    /// </summary>
    public class Condition : IPropertyChanged
    {
        private string Name;

        private bool Status;

        /// <summary>
        /// Получить название статуса
        /// </summary>
        public string GetName
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Получить\Задать статус состояния.
        /// </summary>
        public bool StatusChanged
        {
            get
            {
                return Status;
            }

            set
            {
                StatusChangedEvent.Invoke(new ConditionArgs(Name, value));
                Status = value;
            }
        }

        /// <summary>
        /// Событие возникает при изменении Состояния
        /// </summary>
        public event StatusProperty StatusChangedEvent;

        public Condition(string Name, bool Status)
        {
            this.Name = Name;
            this.Status = Status;
        }
    }
}
