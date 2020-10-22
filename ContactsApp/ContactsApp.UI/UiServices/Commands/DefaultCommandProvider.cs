using System.Windows.Input;
using System.Collections.Generic;

using ContactsApp.UI.WpfExtesions;
using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.UiServices.Commands
{
    public class DefaultCommandProvider : ICommandProvider
    {
        private Dictionary<string, ICommand> _commandsDict;

        public IDictionary<string, ICommand> Commands
        {
            get => _commandsDict;
        }

        public DefaultCommandProvider(params KeyValuePair<string, ICommand>[] commands)
        {
            _commandsDict = commands.InitializeDictionary();
        }

        public void RemoveCommand(string key, bool check = true)
        {
            if (check)
            {
                if (_commandsDict.ContainsKey(key))
                {
                    _commandsDict.Remove(key);
                }
            }
            else
            {
                _commandsDict.Remove(key);
            }
        }
        public void AddCommand(string key, ICommand command, bool check = true)
        {
            if (check)
            {
                if (!_commandsDict.ContainsKey(key))
                {
                    _commandsDict.Add(key, command);
                }
            }
            else
            {
                _commandsDict.Add(key, command);
            }
        }
        public void ReplaceCommand(string key, ICommand command, bool skipCheck = false)
        {
            if ((_commandsDict.ContainsKey(key)))
            {
                if (skipCheck)
                {
                    _commandsDict[key] = command;
                }
                else
                {
                    this.RemoveCommand(key, false);
                    this.AddCommand(key, command, false);
                }
            }
        }
        public bool RaiseRelayDefaultCommand(string commandName)
        {
            var isTrue = this.VerifyCommandName(commandName);

            if (isTrue)
            {
                _commandsDict[commandName].RaiseRelayDefaultCommand();
            }

            return isTrue;
        }
        public bool RaiseRelayGenericCommand<TAny>(string commandName)
        {
            var isTrue = this.VerifyCommandName(commandName);

            if (isTrue)
            {
                _commandsDict[commandName].RaiseRelayGenericCommand<TAny>();
            }

            return isTrue;
        }

        private bool VerifyCommandName(string commandName)
        {
            return _commandsDict.CheckStringKey(commandName);
        }
    }
}