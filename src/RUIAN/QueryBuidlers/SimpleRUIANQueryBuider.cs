using RUIAN.QueryBuidlers;
using RUIAN.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUIAN
{
    /// <summary>
    /// Crates query selecting items that have region number within the set of
    /// region numbers and the parcel number within the set of parcel numbers.
    /// No restriction is applied if set is left empty. It is not thread safe.
    /// </summary>
    public class SimpleRUIANQueryBuider : IRUIANQueryBuilder<SimpleRUIANQueryBuider>
    {
        private List<int> regionNumbers;
        private List<(int, int?, NumberingType)> parcelNumbers;

        public SimpleRUIANQueryBuider()
        {
            ClearAll();
        }

        public SimpleRUIANQueryBuider ClearAll()
        {
            ClearRegionNumbers();
            ClearParcelNumbers();
            return this;
        }

        public SimpleRUIANQueryBuider ClearRegionNumbers()
        {
            regionNumbers = new List<int>();
            return this;
        }
        public SimpleRUIANQueryBuider ClearParcelNumbers()
        {
            parcelNumbers = new List<(int, int?, NumberingType)>();
            return this;
        }

        public SimpleRUIANQueryBuider SetRegion(int regionNumber)
        {
            regionNumbers = new List<int>() { regionNumber };
            return this;
        }

        public SimpleRUIANQueryBuider SetRegions(IEnumerable<int> regionNumbers)
        {
            this.regionNumbers = regionNumbers.ToList();
            return this;
        }

        public SimpleRUIANQueryBuider AddRegion(int regionNumber)
        {
            regionNumbers.Add(regionNumber);
            return this;
        }

        public SimpleRUIANQueryBuider AddRegions(IEnumerable<int> regionNumbers)
        {
            this.regionNumbers.AddRange(regionNumbers);
            return this;
        }

        public SimpleRUIANQueryBuider SetParcelNumber(int rootNumber, int? subdivisionNumber = null,
                                                      NumberingType numberingType = NumberingType.Nespecifikovana)
        {
            parcelNumbers = new List<(int, int?, NumberingType)> { (rootNumber, subdivisionNumber, numberingType) };
            return this;
        }

        public SimpleRUIANQueryBuider SetParcelNumbers(IEnumerable<(int, int?, NumberingType)> parcelNumbers)
        {
            this.parcelNumbers = parcelNumbers.ToList();
            return this;
        }

        public SimpleRUIANQueryBuider AddParcelNumber(int rootNumber, int? subdivisionNumber = null,
                                                      NumberingType numberingType = NumberingType.Nespecifikovana)
        {
            parcelNumbers.Add((rootNumber, subdivisionNumber, numberingType));
            return this;
        }

        public SimpleRUIANQueryBuider AddParcelNumbers(IEnumerable<(int, int?, NumberingType)> parcelNumbers)
        {
            this.parcelNumbers.AddRange(parcelNumbers);
            return this;
        }

        public string CreateQuery()
        {
            StringBuilder query = new StringBuilder();

            var regionNumbersQuery = CreateRegionNumbersQuery();
            var parcelNumbersQuery = CreateParcelNumbersQuery();

            query.Append(regionNumbersQuery);

            if (query.Length > 0 && parcelNumbers.Count > 0)
            {
                query.Append(" and ");
            }
            query.Append(parcelNumbersQuery);

            return query.ToString();
        }

        private static string CreateGenericEnumerableQuery<T>(IEnumerable<T> items, Func<T, string> subqueryCreator)
        {
            StringBuilder query = new StringBuilder();
            int itemCount = items.Count();

            if (itemCount == 1)
            {
                query.Append(subqueryCreator(items.First()));
            }
            else if (itemCount > 0)
            {
                query.Append('(');
                foreach (var item in items)
                {
                    query.Append(subqueryCreator(item));
                    query.Append(" or ");
                }
                query.Length -= 4;
                query.Append(')');
            }


            return query.ToString();
        }


        private string CreateRegionNumbersQuery()
        {
            var regionNumberQueryCreator = (int regionNumber) => "katastralniuzemi = " + regionNumber.ToString();
            return CreateGenericEnumerableQuery(regionNumbers, regionNumberQueryCreator);
        }

        private string CreateParcelNumbersQuery()
        {
            var parcelNumberQueryCreator = ((int rootNumber, int? subdivisionNumber, NumberingType numberingType) parcelNumber) =>
            {
                var query = new StringBuilder("(");
                query.Append("kmenovecislo = ");
                query.Append(parcelNumber.rootNumber.ToString());
                if (parcelNumber.subdivisionNumber.HasValue)
                {
                    query.Append(" and poddelenicisla = ");
                    query.Append(parcelNumber.subdivisionNumber);
                }
                else
                {
                    query.Append(" and poddelenicisla IS NULL");
                }

                var numberingType = parcelNumber.numberingType;
                if (parcelNumber.numberingType != NumberingType.Nespecifikovana)
                {
                    query.Append(" and druhcislovanikod = ");
                    query.Append((int)numberingType);
                }

                query.Append(")");
                return query.ToString();
            };

            return CreateGenericEnumerableQuery(parcelNumbers, parcelNumberQueryCreator);
        }

        public override string ToString() => CreateQuery();
    }
}
