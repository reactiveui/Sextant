using System;
using ReactiveUI;

namespace SextantSample.ViewModels
{
    public static class Interactions
    {
        public static readonly Interaction<Exception, bool> ErrorMessage = new();
    }
}
