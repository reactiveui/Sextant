using System;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Interface that defines a view model for a page for the navigation stack.
    /// </summary>
    public interface IPageViewModel
    {
        string Id
        {
            get;
        }
    }
}