using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Exceptions
{
    public class NotEnoughQuantityException: Exception
    {
        public NotEnoughQuantityException(string message) : base(message) { }
    }
}
