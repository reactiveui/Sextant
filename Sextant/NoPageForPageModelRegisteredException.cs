using System;
namespace Sextant
{
	public class NoPageForPageModelRegisteredException : SextantException
    {
        public NoPageForPageModelRegisteredException(string message) : base(message)
        {
        }
    }
}
