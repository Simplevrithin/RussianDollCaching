namespace RussianDollCaching.Attributes
{
    public class NestedDollAttribute : Attribute
    {
        public readonly bool SingleObject;
        public NestedDollAttribute(bool _SingleObject = true) 
        {
            SingleObject = _SingleObject;
        }
    }
}
