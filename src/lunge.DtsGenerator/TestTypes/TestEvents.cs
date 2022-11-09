namespace lunge.DtsGenerator.TestTypes;

public class TestEventObj
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void Add() {}
    public void Remove() {}
}

public class TestEvents
{
    public event EventHandler? EventEmpty;
    public event EventHandler<string>? EventString;
    public event EventHandler<TestEventObj>? EventObj; 

    public void Subscribe(Action<string> callback)
    {
        EventString += (sender, str) => callback(str);
    }

    public void SubSuper(Func<object, int, string, int> myFunc) {}

    public void Unsubscribe()
    {
        
    }
}