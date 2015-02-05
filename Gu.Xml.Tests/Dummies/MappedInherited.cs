namespace Gu.Xml.Tests.Dummies
{
    public class MappedInherited : MappedSimpleClass
    {
        public int Value3 { get; set; }
        
        public int Value4 { get; set; }

        public override Gu.Xml.XmlMap GetMap()
        {
            return base.GetMap()
                       .WithElement(() => Value3)
                       .WithAttribute(() => Value4);
        }
    }
}
