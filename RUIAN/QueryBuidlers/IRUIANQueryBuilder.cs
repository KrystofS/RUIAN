namespace RUIAN.QueryBuidlers
{
    public interface IRUIANQueryBuilder<T> where T : IRUIANQueryBuilder<T>
    {
        T ClearAll();
        string CreateQuery();
    }
}
