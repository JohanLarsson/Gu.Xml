# Gu.Xml
Extensions for XmlReader &amp; XmlWriter
Prototyping a helper for implementing IXmSerializable. Define a map like (see tests for more samples):

    public virtual XmlMap GetMap()
    {
        return new XmlMap().WithElement(() => Value1)
                           .WithAttribute(() => Value2)
                           .WithAttribute(() => Value3, () => _value3)
                           .WithElement(() => Value4, () => _value4);
    }
Handles:
- Attributes & elements
- private set
- backing (readonly) field.
- IEnumerable&lt;T&gt; backed by List&lt;T&gt;
- Inheritance