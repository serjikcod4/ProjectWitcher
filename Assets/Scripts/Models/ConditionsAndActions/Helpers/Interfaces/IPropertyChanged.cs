using Assets.Scripts.Models.ConditionsAndActions.Helpers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers.Interfaces
{
    public interface IPropertyChanged
    {
        event StatusProperty StatusChangedEvent;
    }
}
