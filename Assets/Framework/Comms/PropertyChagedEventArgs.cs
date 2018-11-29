using System;

public class PropertyChagedEventArgs : EventArgs
{
    public PropertyChagedEventArgs(string propertyName, object oldValue, object newValue)
    {
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
    }

    public bool Cancel { get; set; }
    public string PropertyName { get; private set; }
    public object OldValue { get; private set; }
    public object NewValue { get; set; }
}
