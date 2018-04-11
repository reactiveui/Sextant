using System;
namespace Sextant
{
	/// <summary>
    /// IBasePage / IBasePageModel navigation events.
    /// </summary>
    public interface INavigationInterceptors : INavigationRemovingFromCache, INavigationCanPush, INavigationPushed,
        INavigationCanPop, INavigationPopped, INavigationCanInsert, INavigationInserted, INavigationCanRemove, INavigationRemoved
    {
    }

    /// <summary>
    /// INavigationRemovingFromCache.
    /// </summary>
    public interface INavigationRemovingFromCache
    {
        /// <summary>
        /// Triggered when removing from cache.
        /// </summary>
        void NavigationRemovingFromCache();
    }

    /// <summary>
    /// INavigationPushing.
    /// </summary>
    public interface INavigationCanPush
    {
        /// <summary>
        /// Triggered when pushing. If <c>false</c>returned push is cancelled.
        /// </summary>
        bool NavigationCanPush();
    }

    /// <summary>
    /// INavigationPushed.
    /// </summary>
    public interface INavigationPushed
    {
        /// <summary>
        /// Triggered when pushed.
        /// </summary>
        void NavigationPushed();
    }

    /// <summary>
    /// INavigationPopping.
    /// </summary>
    public interface INavigationCanPop
    {
        /// <summary>
        /// Triggered when popping. If <c>false</c>returned pop is cancelled.
        /// </summary>
        bool NavigationCanPop();
    }

    /// <summary>
    /// INavigationPopped.
    /// </summary>
    public interface INavigationPopped
    {
        /// <summary>
        /// Triggered when popped.
        /// </summary>
        void NavigationPopped();
    }

    /// <summary>
    /// INavigationInserting.
    /// </summary>
    public interface INavigationCanInsert
    {
        /// <summary>
        /// Triggered when inserting. If <c>false</c>returned insert is cancelled.
        /// </summary>
        bool NavigationCanInsert();
    }

    /// <summary>
    /// INavigationInserted.
    /// </summary>
    public interface INavigationInserted
    {
        /// <summary>
        /// Triggered when inserted.
        /// </summary>
        void NavigationInserted();
    }

    /// <summary>
    /// INavigationRemoving.
    /// </summary>
    public interface INavigationCanRemove
    {
        /// <summary>
        /// Triggered when removing. If <c>false</c>returned remove is cancelled.
        /// </summary>
        bool NavigationCanRemove();
    }

    /// <summary>
    /// INavigationRemoved.
    /// </summary>
    public interface INavigationRemoved
    {
        /// <summary>
        /// Triggered when removed.
        /// </summary>
        void NavigationRemoved();
    }
}
