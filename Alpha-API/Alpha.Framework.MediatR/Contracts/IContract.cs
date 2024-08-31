using Alpha.Framework.MediatR.Validations;

namespace Alpha.Framework.MediatR.Contracts
{
    public interface IContract
    {
        ValidationContract Contract { get; }
    }
}
