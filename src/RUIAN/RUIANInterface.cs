using RUIAN.QueryBuidlers;
using RUIAN.Types;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RUIAN
{
    /// <summary>
    /// Static class for querying RUIAN REST API.
    /// </summary>
    public static class RUIANInterface
    {
        private static readonly UrlEncoder encoder = UrlEncoder.Create();
        private static readonly WebClient client = new WebClient() { Encoding = System.Text.Encoding.UTF8 };

        /// <summary>
        /// Returns list of parcels based on condition.
        /// </summary>
        /// <param name="query">SQL condition (probably MSSQL)</param>
        /// <returns>Collection of parcels matching condition</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ICollection<RuianParcel> GetParcels(string query)
        {
            var sQuery = Encode(query);
            string request = $"https://ags.cuzk.cz/arcgis/rest/services/RUIAN/Vyhledavaci_sluzba_nad_daty_RUIAN/MapServer/0/query?where={sQuery}&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry=&geometryType=esriGeometryPolygon&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Meter&relationParam=&outFields=*&returnGeometry=false&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=true&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=geojson";

            var json = client.DownloadString(request);
            return JsonSerializer.Deserialize<RuianParcelResponse>(json).Parcely;
        }

        /// <summary>
        /// Returns list of parcels based on condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RUIANQueryBuilder">RUIANQuery builder from which the query is created.</param>
        /// <returns>Collection of parcels matching condition</returns>
        public static ICollection<RuianParcel> GetParcels<T>(IRUIANQueryBuilder<T> RUIANQueryBuilder) where T : IRUIANQueryBuilder<T>
            => GetParcels(RUIANQueryBuilder.CreateQuery());

        /// <summary>
        /// Returns first parcel based on condition. Null if no found.
        /// </summary>
        /// <param name="query">SQL condition (probably MSSQL)</param>
        /// <returns>Parcel matching condition</returns>
        public static RuianParcel GetParcel(string query)
        {
            var parcels = GetParcels(query);
            return parcels != null && parcels.Any() ? parcels.First() : null;
        }

        /// <summary>
        /// Returns first parcel based on condition. Null if no found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="RUIANQueryBuilder">RUIANQuery builder from which the query is created.</param>
        /// <returns>Parcel matching condition</returns>
        public static RuianParcel GetParcel<T>(IRUIANQueryBuilder<T> RUIANQueryBuilder) where T : IRUIANQueryBuilder<T>
            => GetParcel(RUIANQueryBuilder.ToString());


        [MethodImpl(MethodImplOptions.Synchronized)]
        private static string Encode(string s)
            => encoder.Encode(s);
    }
}
