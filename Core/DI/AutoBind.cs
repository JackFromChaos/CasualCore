using System;
public class AutoBind : Attribute
{
    public Type bindWithType;

    public AutoBind(Type bindWithType = null)
    {
        this.bindWithType = bindWithType;
    }
}