using System;
using System.Reflection;

namespace lunge.Library.Scripting;

public class EventProxy
{
    public object ProxyObject { get; }
    public Type ProxyType { get; }

    public EventProxy(object obj)
    {
        ProxyObject = obj;
        ProxyType = obj.GetType();
    }

    public Subscription Subscribe(Delegate handler, string eventName)
    {
        var e = ProxyType.GetEvent(eventName);
        if (e != null)
        {
            e.AddEventHandler(ProxyObject, handler);

            return new Subscription(e, ProxyObject, handler);
        }

        throw new EntryPointNotFoundException($"No event found with name '{eventName}'");
    }
}

public class Subscription
{
    internal EventInfo EventInfo { get; }
    private object _targetObject;
    private Delegate _handler;

    internal Subscription(EventInfo eventInfo, object targetObject, Delegate handler)
    {
        EventInfo = eventInfo;
        _targetObject = targetObject;
        _handler = handler;
    }

    public void Unsubscribe()
    {
        EventInfo.RemoveEventHandler(_targetObject, _handler);
    }
}