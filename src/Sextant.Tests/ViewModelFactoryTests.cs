using Sextant.Abstractions;
using Shouldly;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Tests the <see cref="ViewModelFactory"/>.
    /// </summary>
    public sealed class ViewModelFactoryTests
    {
        /// <summary>
        /// Tests the currently registered view model factory parameter.
        /// </summary>
        public class CurrentPropertyTests
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CurrentPropertyTests"/> class.
            /// </summary>
            public CurrentPropertyTests()
            {
                Splat.Locator.CurrentMutable.UnregisterAll(typeof(IViewModelFactory));
            }

            /// <summary>
            /// Should throw if the IViewFactory is not registered.
            /// </summary>
            [Fact]
            public void Should_Throw_If_Not_Registered()
            {
                // Given, When
                var result = Should.Throw<ViewModelFactoryNotFoundException>(() => ViewModelFactory.Current);

                // Then
                result.Message.ShouldBe("Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken");
            }

            /// <summary>
            /// Should return the default view model factory.
            /// </summary>
            [Fact]
            public void Should_Return_View_Model_Factory()
            {
                // Given, When
                Splat.Locator.CurrentMutable.Register(() => new DefaultViewModelFactory(), typeof(IViewModelFactory));
                var viewModelFactory = ViewModelFactory.Current;

                // Then
                viewModelFactory.ShouldBeAssignableTo<IViewModelFactory>();
                viewModelFactory.ShouldBeOfType<DefaultViewModelFactory>();
            }
        }
    }
}
