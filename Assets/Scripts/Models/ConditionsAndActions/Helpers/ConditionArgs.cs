using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers
{
    /// <summary>
    /// Представляет данные для события IPropertyChanged.StatusChanged
    /// </summary>
    public class ConditionArgs:EventArgs
    {
        private string Name;
        private bool Status;

        /// <summary>
        /// Возвращает название изменного статуса
        /// </summary>
        public string ConditionName
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Возвращает значение измененного статуса
        /// </summary>
        public bool ConditionStatus
        {
            get
            {
                return Status;
            }
        }

        /// <summary>
        /// Создает новый экземпляр класса ConditionArgs
        /// </summary>
        /// <param name="Name">Название измененного статуса</param>
        /// <param name="Status">Текущее значение измененного статуса</param>
        public ConditionArgs(string Name, bool Status)
        {
            this.Name = Name;
            this.Status = Status;
        }
    }
}
