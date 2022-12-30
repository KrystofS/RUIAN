using RUIAN.Types;
using Xunit;

namespace RUIAN.Tests
{
    /// <summary>
    /// Test covering basic query builder functionality.
    /// </summary>
    public class SimpleRUIANQueryBuiderTests
    {
        private SimpleRUIANQueryBuider queryBuilder = new SimpleRUIANQueryBuider();
        /// <summary>
        /// Compares queryBuilder query with expected query.
        /// </summary>
        /// <param name="expectedQuery">expected query</param>
        private void AssertQuery(string expectedQuery)
            => Assert.Equal(expectedQuery, queryBuilder.CreateQuery());

        [Fact]
        public void EmptyQuery()
        {
            AssertQuery("");
        }

        [Fact]
        public void SetSingleRegion()
        {
            queryBuilder.SetRegion(700703);
            AssertQuery("katastralniuzemi = 700703");
        }

        [Fact]
        public void SetSingleParcelRootNumber()
        {
            queryBuilder.SetParcelNumber(366);
            AssertQuery("(kmenovecislo = 366 and poddelenicisla IS NULL)");
        }

        [Fact]
        public void SetSingleParcelRootAndSubdivisionNespecifikovano()
        {
            queryBuilder.SetParcelNumber(366, 1);
            AssertQuery("(kmenovecislo = 366 and poddelenicisla = 1)");
        }

        [Fact]
        public void SetSingleParcelRootSubdivisionStavebni()
        {
            queryBuilder.SetParcelNumber(366, 1, NumberingType.Stavebni);
            AssertQuery("(kmenovecislo = 366 and poddelenicisla = 1 and druhcislovanikod = 1)");
        }

        [Fact]
        public void SetSingleParcelRootSubdivisionPozemkova()
        {
            queryBuilder.SetParcelNumber(366, 1, NumberingType.Pozemkova);
            AssertQuery("(kmenovecislo = 366 and poddelenicisla = 1 and druhcislovanikod = 2)");
        }

        [Fact]
        public void SetMultipleRegions()
        {
            queryBuilder.SetRegions(new int[] { 700703, 746304 });
            AssertQuery("(katastralniuzemi = 700703 or katastralniuzemi = 746304)");
        }

        [Fact]
        public void SetMultipleParcelRootAndSubdivision()
        {
            queryBuilder.SetParcelNumbers(new (int, int?, NumberingType)[] { (366, 1, NumberingType.Nespecifikovana),
                                                                             (367, 2, NumberingType.Nespecifikovana) });
            AssertQuery("((kmenovecislo = 366 and poddelenicisla = 1) or (kmenovecislo = 367 and poddelenicisla = 2))");
        }

        [Fact]
        public void SetMultipleParcelMixed()
        {
            queryBuilder.SetParcelNumbers(new (int, int?, NumberingType)[] { (366, null, NumberingType.Nespecifikovana),
                                                                             (367, 2, NumberingType.Nespecifikovana) });
            AssertQuery("((kmenovecislo = 366 and poddelenicisla IS NULL) or (kmenovecislo = 367 and poddelenicisla = 2))");
        }

        [Fact]
        public void Clear()
        {
            queryBuilder.SetParcelNumbers(new (int, int?, NumberingType)[] { (366, null, NumberingType.Nespecifikovana),
                                                                             (367, 2, NumberingType.Nespecifikovana) })
                        .SetRegions(new int[] { 700703, 746304 }); ;
            queryBuilder.ClearAll();
            AssertQuery("");
        }

        [Fact]
        public void CombinedQuery()
        {
            queryBuilder.SetParcelNumbers(new (int, int?, NumberingType)[] { (366, null, NumberingType.Pozemkova),
                                                                             (367, 2, NumberingType.Stavebni),
                                                                             (367, 2, NumberingType.Nespecifikovana)})
                        .SetRegions(new int[] { 700703, 746304 }); ;
            AssertQuery("(katastralniuzemi = 700703 or katastralniuzemi = 746304) and ((kmenovecislo = 366 and poddelenicisla IS NULL and druhcislovanikod = 2) or (kmenovecislo = 367 and poddelenicisla = 2 and druhcislovanikod = 1) or (kmenovecislo = 367 and poddelenicisla = 2))");
        }
    }
}
