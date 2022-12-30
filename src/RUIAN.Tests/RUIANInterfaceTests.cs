using FluentAssertions;
using RUIAN.QueryBuidlers;
using RUIAN.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace RUIAN.Tests
{
    /// <summary>
    /// Test covering basic RUIAN interface functionality.
    /// </summary>
    public class RUIANInterfaceTests
    {
        private static string expectedParcelsText = File.ReadAllText("./Resources/query_result.txt");
        private IEnumerable<RuianParcel> expectedParcels = JsonSerializer.Deserialize<RuianParcelResponse>(expectedParcelsText).Parcely;

        [Fact]
        public void StringRequest()
        {
            string query = "(katastralniuzemi = 700703 or katastralniuzemi = 746304) and (kmenovecislo = 366 and poddelenicisla IS NULL)";
            var parcels = RUIANInterface.GetParcels(query);
            parcels.Should().BeEquivalentTo(expectedParcels);
        }

        [Fact]
        public void QueryBuilderRequest()
        {
            var queryBuilder = new SimpleRUIANQueryBuider();
            queryBuilder.SetParcelNumbers(new (int, int?, NumberingType)[] { (366, null, NumberingType.Nespecifikovana) })
                        .SetRegions(new int[] { 700703, 746304 }); ;

            var parcels = RUIANInterface.GetParcels(queryBuilder);
            parcels.Should().BeEquivalentTo(expectedParcels);
        }

        [Fact]
        public void StavebniQuery()
        {
            var queryBuilder = new SimpleRUIANQueryBuider();
            queryBuilder.SetParcelNumber(498, numberingType: NumberingType.Stavebni)
                        .SetRegion(624217);
            var parcels = RUIANInterface.GetParcels(queryBuilder);
            parcels.Should().HaveCount(1);
            Assert.Equal(2811881306, parcels.First().Properties.Id);
        }

        [Fact]
        public void PozemkovaQuery()
        {
            var queryBuilder = new SimpleRUIANQueryBuider();
            queryBuilder.SetParcelNumber(498, numberingType: NumberingType.Pozemkova)
                        .SetRegion(624217);
            var parcels = RUIANInterface.GetParcels(queryBuilder);
            parcels.Should().HaveCount(1);
            Assert.Equal(2812321306, parcels.First().Properties.Id);
        }
    }
}
