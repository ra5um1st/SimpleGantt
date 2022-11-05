namespace SimpleGantt.Core.Patterns.Visitor
{
    public interface IAcceptable<TObject> where TObject : class
    {
        void Accept(IVisitor<TObject> visitor);
    }
}
