using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    /// <summary>
    /// Позволяет добавлять Статусы
    /// </summary>
    public interface IGetConditions
    {
        void ApplyCondition(params CurrentCondition[] Characteristics);
    }
}
