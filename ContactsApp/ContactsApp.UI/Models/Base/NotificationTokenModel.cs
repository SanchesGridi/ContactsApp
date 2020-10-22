namespace ContactsApp.UI.Models.Base
{
    public abstract class NotificationTokenModel : NotificationReflectorModel
    {
        private protected const int NotExistToken = -111;
        private protected int _token;

        public NotificationTokenModel(int token = NotExistToken)
        {
            _token = token;
        }

        public virtual void SetToken(int token)
        {
            _token = token;
        }
        public virtual int GetToken()
        {
            return _token;
        }
    }
}