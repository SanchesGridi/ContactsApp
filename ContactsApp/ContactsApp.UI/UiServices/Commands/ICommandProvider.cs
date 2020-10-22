using System.Windows.Input;
using System.Collections.Generic;

namespace ContactsApp.UI.UiServices.Commands
{
    public interface ICommandProvider
    {
        public IDictionary<string, ICommand> Commands { get; }

        void RemoveCommand(string key, bool check = true);
        void AddCommand(string key, ICommand command, bool check = true);
        void ReplaceCommand(string key, ICommand command, bool skipCheck = false);
        bool RaiseRelayDefaultCommand(string commandName);
        bool RaiseRelayGenericCommand<TAny>(string commandName);
    }
}