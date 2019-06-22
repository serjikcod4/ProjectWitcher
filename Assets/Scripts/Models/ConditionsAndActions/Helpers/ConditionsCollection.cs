using Assets.Scripts.Models.ConditionsAndActions.Helpers.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers
{
    public class ConditionsCollection : ObservableCollection<Condition>
    {
        /// <summary>
        /// Проверяет есть ли указанное Состояние в коллекции по названию
        /// </summary>
        /// <param name="Name">Название состояния</param>
        /// <returns></returns>
        public bool HasName(string Name)
        {
            foreach (var item in Items)
            {
                if (item.GetName == Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получает текущий статус состояния
        /// </summary>
        /// <param name="Name">Название состояния</param>
        /// <returns></returns>
        public bool CurrentConditionStatus(string Name)
        {
            foreach (var item in Items)
            {
                if (item.GetName == Name)
                {
                    return item.StatusChanged;
                }
            }
            return false;
        }

        /// <summary>
        /// Устанавливает статус состояния по его названию
        /// </summary>
        /// <param name="Name">Название состояния</param>
        /// <param name="Status">Статус состояния</param>
        public void ChangeConditionStatus(string Name, bool Status)
        {
            for (int i = 0; i < Count; i++)
            {
                if(Items[i].GetName == Name)
                {
                    Items[i].StatusChanged = Status;
                }
            }
        }

        /// <summary>
        /// Устанавливает метод для события изменения статуса состояний
        /// </summary>
        /// <param name="Method"></param>
        public void SetPropertyEventMethod(StatusProperty Method)
        {
            foreach (var item in Items)
            {
                item.StatusChangedEvent += Method;
            }
        }
    }
}
