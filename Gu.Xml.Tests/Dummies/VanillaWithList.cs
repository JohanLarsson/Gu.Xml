namespace Gu.Xml.Tests.Dummies
{
    using System.Collections.Generic;

    public class VanillaWithList
    {
        private VanillaWithList()
        {
            
        }
        public VanillaWithList(int n)
        {
            Items = new List<MappedSimpleClass>();
            for (int i = 0; i < n; i++)
            {
                Items.Add(new MappedSimpleClass { Value1 = i, Value2 = 2 * i });
            }
        }

        public List<MappedSimpleClass> Items { get; set; }
    }
}
