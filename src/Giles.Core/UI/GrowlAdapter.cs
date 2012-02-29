using Growl.Connector;
using Growl.CoreLibrary;

namespace Giles.Core.UI
{
    public interface IGrowlAdapter
    {
        void Notify(string id, string title, string text);
        void Notify(string id, string title, string text, Resource icon);
    }

    public class GrowlAdapter : IGrowlAdapter
    {
        private GrowlConnector growl;
        private NotificationType notificationType;
        private Application application;

        public GrowlAdapter()
        {
            Initialize();
        }

        void Initialize()
        {
            notificationType = new NotificationType("BUILD_RESULT_NOTIFICATION", "Sample Notification");
            application = new Application("Giles");
            growl = new GrowlConnector
                    {
                        EncryptionAlgorithm = Cryptography.SymmetricAlgorithmType.PlainText
                    };


            growl.Register(application, new[] { notificationType });
        }

        public void Notify(string id, string title, string text)
        {
            growl.Notify(new Notification(application.Name, notificationType.Name, id, title, text));
        }

        public void Notify(string id, string title, string text, Resource icon)
        {
            growl.Notify(new Notification(application.Name, notificationType.Name, id, title, text) { Icon = icon });
        }
    }
}