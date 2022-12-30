namespace RUIAN.QueryBuidlers
{
    /// <summary>
    /// An interface for classes used for creating RUIAN queries.
    /// </summary>
    /// <typeparam name="T">The same type as the object that implements this interface.</typeparam>
    public interface IRUIANQueryBuilder<T> where T : IRUIANQueryBuilder<T>
    {
        /// <summary>
        /// Restores the original state of the builder.
        /// </summary>
        /// <returns>This object.</returns>
        T ClearAll();
        /// <summary>
        /// Crates query for RUIAN based on current state of this object.
        /// </summary>
        /// <returns>Query for RUIAN.</returns>
        string CreateQuery();
    }
}
