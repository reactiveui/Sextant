using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Genesis.Logging;

namespace System.Reactive.Linq
{
    public static class SubscribeSafeExtensions
    {
        public static IDisposable SubscribeSafe<T>(
            this IObservable<T> @this,
            [CallerMemberName]string callerMemberName = null,
            [CallerFilePath]string callerFilePath = null,
            [CallerLineNumber]int callerLineNumber = 0)
        {
            return @this
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        var logger = LoggerService.GetLogger(typeof(SubscribeSafeExtensions));
                        logger.Error(ex, "An exception went unhandled. Caller member name: '{0}', caller file path: '{1}', caller line number: {2}.", callerMemberName, callerFilePath, callerLineNumber);

                        Debugger.Break();
                    });
        }
    }
}

