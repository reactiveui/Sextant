using System;
namespace Sextant
{
    public static class SextantCore
    {
        public static IBaseLogger Logger { get; set; }

        static ISextantNavigationService current;

        public static ISextantNavigationService Instance
        {
            get
            {
                if (current == null)
                {
                    throw new NullReferenceException("CurrentFactory is null. Please initialize it with SetCurrentFactory method");
                }

                return current;
            }
        }

        public static void SetCurrentFactory(ISextantNavigationService factory)
        {
            current = factory;
        }
    }
}