namespace lunge.DtsGenerator.TestTypes;

public abstract class TestAbstractClass
{
    public abstract string AbstractProp { get; set; }
    public virtual string VirtualProp { get; set; }
    public string Prop { get; set; }

    public virtual string VirtualMethod(string param) => param;
    public abstract string AbstractMethod(string param);
    public string Method(string param) => param;
}

public interface ITestInterface1
{
    void InterfaceMethod1();
    string InterfaceMethod2();
    int InterfaceMethod3(double param);
}

public interface ITestInterface2<T>
{
    T InterfaceProp { get; set; }
    void AnotherMethod1();
}

[Flags]
public enum TestEnum
{
    One = 1,
    Two = 2,
    Three = 4,
    Four = 8
}

public class TestInheritance<T> : TestAbstractClass, ITestInterface1, ITestInterface2<T>
{
    public override string AbstractProp { get; set; }
    public T InterfaceProp { get; set; }

    public T TestField;
    public string TestField2;
    public readonly int TestReadonlyInt = 32;

    public List<int> PublicListField;
    public List<string> PublicListProp { get; }

    public event EventHandler<string>? TestEvent1;
    public event EventHandler<double?>? TestEvent2;

    public void SubscribeToTestEvent1(EventHandler<string> eh)
    {
        TestEvent1 += eh;
    }

    public List<double> PublicListMethod()
    {
        throw new NotImplementedException();
    }

    public override string AbstractMethod(string param)
    {
        throw new NotImplementedException();
    }

    public override string VirtualMethod(string param)
    {
        return base.VirtualMethod(param);
    }

    public void InterfaceMethod1()
    {
        throw new NotImplementedException();
    }

    public string InterfaceMethod2()
    {
        throw new NotImplementedException();
    }

    public int InterfaceMethod3(double param)
    {
        throw new NotImplementedException();
    }

    public void AnotherMethod1()
    {
        throw new NotImplementedException();
    }
}

public class TestInheritance2 : TestInheritance<string> {}

public class TestInheritance3<T> : TestInheritance<T> { }

public class TestInheritance4<T, K> : TestInheritance3<K>
{
    public T TestTVal { get; }
    public K TestKVal { get; }
}