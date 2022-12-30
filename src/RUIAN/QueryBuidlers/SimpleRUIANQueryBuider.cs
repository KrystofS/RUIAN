using RUIAN.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUIAN.QueryBuidlers
{
    /// <summary>
    /// Creates query selecting items that have region number within the set of
    /// region numbers and the parcel number within the set of parcel numbers.
    /// No restriction is applied if the set is left empty. It is not thread-safe.
    /// </summary>
    public class SimpleRUIANQueryBuider : IRUIANQueryBuilder<SimpleRUIANQueryBuider>
    {
        private List<int> regionNumbers;
        private List<(int, int?, NumberingType)> parcelNumbers;

        /// <summary>
        /// Creates an empty instance of SimpleRUIANQueryBuider.
        /// </summary>
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

        /// <summary>
        /// Clears region numbers.
        /// </summary>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider ClearRegionNumbers()
        {
            regionNumbers = new List<int>();
            return this;
        }

        /// <summary>
        /// Clears parcel numbers.
        /// </summary>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider ClearParcelNumbers()
        {
            parcelNumbers = new List<(int, int?, NumberingType)>();
            return this;
        }

        /// <summary>
        /// Sets region collection to contain region specified in the parameter.
        /// </summary>
        /// <param name="regionNumber">Region number to set.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider SetRegion(int regionNumber)
        {
            regionNumbers = new List<int>() { regionNumber };
            return this;
        }

        /// <summary>
        /// Sets region collection to contain regions specified in the parameter.
        /// </summary>
        /// <param name="regionNumbers">Region numbers to set.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider SetRegions(IEnumerable<int> regionNumbers)
        {
            this.regionNumbers = regionNumbers.ToList();
            return this;
        }

        /// <summary>
        /// Adds region to region collection.
        /// </summary>
        /// <param name="regionNumber">Region number to add.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider AddRegion(int regionNumber)
        {
            regionNumbers.Add(regionNumber);
            return this;
        }

        /// <summary>
        /// Adds regions to region collection.
        /// </summary>
        /// <param name="regionNumbers">Region numbers to add.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider AddRegions(IEnumerable<int> regionNumbers)
        {
            this.regionNumbers.AddRange(regionNumbers);
            return this;
        }

        /// <summary>
        /// Sets parcel number collection to contain parcel specified in parameters.
        /// </summary>
        /// <param name="rootNumber">Root number of the parcel.</param>
        /// <param name="subdivisionNumber">Subdivision number of the parcel.</param>
        /// <param name="numberingType">Numbering type of the parcel.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider SetParcelNumber(int rootNumber, int? subdivisionNumber = null,
                                                      NumberingType numberingType = NumberingType.Nespecifikovana)
        {
            parcelNumbers = new List<(int, int?, NumberingType)> { (rootNumber, subdivisionNumber, numberingType) };
            return this;
        }

        /// <summary>
        /// Sets parcel number collection to contain parcels specified in the parameter.
        /// </summary>
        /// <param name="parcelNumbers">Parcel numbers to set.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider SetParcelNumbers(IEnumerable<(int, int?, NumberingType)> parcelNumbers)
        {
            this.parcelNumbers = parcelNumbers.ToList();
            return this;
        }

        /// <summary>
        /// Adds parcel to parcel number collection.
        /// </summary>
        /// <param name="rootNumber">Root number of the parcel.</param>
        /// <param name="subdivisionNumber">Subdivision number of the parcel.</param>
        /// <param name="numberingType">Numbering type of the parcel.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider AddParcelNumber(int rootNumber, int? subdivisionNumber = null,
                                                      NumberingType numberingType = NumberingType.Nespecifikovana)
        {
            parcelNumbers.Add((rootNumber, subdivisionNumber, numberingType));
            return this;
        }

        /// <summary>
        /// Adds parcels to parcel number collection.
        /// </summary>
        /// <param name="parcelNumbers">Parcel numbers to add.</param>
        /// <returns>This object.</returns>
        public SimpleRUIANQueryBuider AddParcelNumbers(IEnumerable<(int, int?, NumberingType)> parcelNumbers)
        {
            this.parcelNumbers.AddRange(parcelNumbers);
            return this;
        }

        /// <summary>
        /// Creates query that will find parcels having their parcel number and region
        /// withing collection of parcel numbers and regions of this object.
        /// </summary>
        /// <returns>Query that will find parcels having their parcel number and region
        /// withing collection of parcel numbers and regions of this object.</returns>
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

        /// <summary>
        /// Returns query that will find parcels having their parcel number and region
        /// withing collection of parcel numbers and regions of this object. (same as CreateQuery)
        /// </summary>
        /// <returns>Query that will find parcels having their parcel number and region
        /// withing collection of parcel numbers and regions of this object. (same as CreateQuery)</returns>
        public override string ToString() => CreateQuery();
    }
}
