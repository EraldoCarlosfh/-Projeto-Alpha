namespace Alpha.Framework.MediatR.Validations
{
    public partial class ValidationContract
    {
        public ValidationContract AreEquals(Guid val, Guid comparer, string property, string message)
        {
            if (val.ToString() != comparer.ToString())
                AddNotification(property, message);

            return this;
        }

        public ValidationContract AreNotEquals(Guid val, Guid comparer, string property, string message)
        {
            if (val.ToString() == comparer.ToString())
                AddNotification(property, message);

            return this;
        }
    }
}
