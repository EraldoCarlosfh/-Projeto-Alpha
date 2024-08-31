using Alpha.Framework.MediatR.Notifications;
using Alpha.Framework.MediatR.Validations;

namespace Alpha.Framework.MediatR.Contracts
{
    public abstract class Contract : OctaNotifiable
    {
        protected Contract()
        {
            ValidationContract = new ValidationContract();
        }

        public ValidationContract ValidationContract { get; set; }
    }
}
