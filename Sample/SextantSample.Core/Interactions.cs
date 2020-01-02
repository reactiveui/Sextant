using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace SextantSample.Core
{
    public static class Interactions
    {
        public static Interaction<Exception, bool> ErrorMessage = new Interaction<Exception, bool>();
    }
}
