namespace System.Reactive.Linq
{
	public static class WhereNotNullExtension
    {
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> observable)
        {
            return observable.Where(x => x != null);
        }
    }
}