using System.Runtime.CompilerServices;

namespace System.Configuration
{
    // if users want to get actions that are done internal, such as error handling.
    public class DataContainerEvents
    {
        public delegate void OnEventDelegate(string type, string err);
        public static event OnEventDelegate OnEvent;

        internal static void NotifyError(string message, [CallerMemberName] string method = "")
        {
            OnEvent?.Invoke("Error", $"{method} => {message}");
        }

        internal static void NotifyInformation(string message, [CallerMemberName] string method = "")
        {
            OnEvent?.Invoke("Information", $"{method} => {message}");
        }

    }
}
