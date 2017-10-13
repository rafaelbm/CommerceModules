using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.Common.Modules
{
    public class CommerceEvents
    {
       public CommerceModuleDelegate<OrderItemProcessedEventArgs> OrderItemProcessed { get; set; }
    }
}
