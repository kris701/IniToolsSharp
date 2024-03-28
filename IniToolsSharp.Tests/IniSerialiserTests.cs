using IniToolsSharp.Tests.TestClasses;

namespace IniToolsSharp.Tests
{
    [TestClass]
    public class IniSerialiserTests
    {
        public static IEnumerable<object[]> NormalData()
        {
            yield return new object[] {
                new IniDocument1(),
                $"[SectionName]{Environment.NewLine}" +
                $"Value1=False{Environment.NewLine}" +
                $"Value2=-1{Environment.NewLine}"
            };
            yield return new object[] {
                new IniDocument1()
                {
                    Section = new Section1()
                    {
                        Value1 = true,
                        Value2 = 6434521
                    }
                },
                $"[SectionName]{Environment.NewLine}" +
                $"Value1=True{Environment.NewLine}" +
                $"Value2=6434521{Environment.NewLine}"
            };
            yield return new object[] {
                new IniDocument2()
                {
                    Section = new Section1(),
                    Section2 = new Section1()
                    {
                        Value1 = true,
                        Value2 = 6434521
                    }
                },
                $"[SectionName]{Environment.NewLine}" +
                $"Value1=False{Environment.NewLine}" +
                $"Value2=-1{Environment.NewLine}" +
                $"[SectionName2]{Environment.NewLine}" +
                $"Value1=True{Environment.NewLine}" +
                $"Value2=6434521{Environment.NewLine}"
            };
            yield return new object[] {
                new IniDocument3(),
                $"[SectionName]{Environment.NewLine}" +
                $"Values=[1,5,1341]{Environment.NewLine}"
            };
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Serialise_1(dynamic deserialised, string serialised)
        {
            // ARRANGE
            // ACT
            var text = IniSerialiser.Serialise(deserialised);

            // ASSERT
            Assert.AreEqual(serialised, text);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Serialise_2(dynamic deserialised, string serialised)
        {
            // ARRANGE
            var type = deserialised.GetType();

            // ACT
            var text = IniSerialiser.Serialise(deserialised, type);

            // ASSERT
            Assert.AreEqual(serialised, text);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise(dynamic deserialised, string serialised)
        {
            // ARRANGE
            var type = deserialised.GetType();

            // ACT
            var result = IniSerialiser.Deserialise(serialised, type);

            // ASSERT
            Assert.IsInstanceOfType(result, type);
            Assert.AreEqual(deserialised, result);
        }

        [TestMethod]
        [DynamicData(nameof(NormalData), DynamicDataSourceType.Method)]
        public void Can_Deserialise_BackAndForth(dynamic deserialised, string serialised)
        {
            // ARRANGE
            var type = deserialised.GetType();

            // ACT
            var des = IniSerialiser.Deserialise(serialised, type);
            var ser = IniSerialiser.Serialise(des, type);

            // ASSERT
            Assert.IsInstanceOfType(des, type);
            Assert.AreEqual(deserialised, des);
            Assert.AreEqual(serialised, ser);
        }
    }
}