namespace lunge.Library.Scripting.TsDeclarations.Generator.TsMemberBuilders;

public interface ITsMemberBuilder<in TIn, out TOut> where TOut : IMemberWritable where TIn : class
{
    TOut[] Build(TIn[] data);
}