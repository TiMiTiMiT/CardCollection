﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes.Exceptions
{
    public class CardNotFoundException : Exception
    {
        public CardNotFoundException(string message) : base(message)
        {
        }
    }

}
