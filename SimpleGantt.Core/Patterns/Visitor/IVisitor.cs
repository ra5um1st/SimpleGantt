namespace SimpleGantt.Core.Patterns.Visitor
{
    public interface IVisitor<TObject> where TObject : class
    {
        void Visit(TObject @object);
    }
}
