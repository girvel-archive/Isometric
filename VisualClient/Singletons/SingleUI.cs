using System;
using VisualConsole;
using CommandInterface;
using System.Collections.Generic;

namespace VisualClient.Singletons
{
    public static class SingleUI
	{
        private static ConsoleUI _instance;
        public static ConsoleUI Instance {
            get {
                if (_instance == null)
                {
                    _instance = _getInstance();
                }

                return _instance;
            }
        }

        private static ConsoleUI _getInstance() 
        {
            return new ConsoleUI(
                () => Program.Territory.Buildings,
                new List<Command<ConsoleUI>> {
                new Command<ConsoleUI>(
                    "na",
                    "(new account) creates new account",
                    "@login,password,email,permission",
                    ConsoleHelper._newAccount),

                new Command<ConsoleUI>(
                    "va",
                    "(view accounts) views all accounts",
                    "",
                    ConsoleHelper._viewAccounts),

                new Command<ConsoleUI>(
                    "c",
                    "(clear) clears the console",
                    "",
                    ConsoleHelper._clear),

                new Command<ConsoleUI>(
                    "ba",
                    "adds type of log message to blacklist",
                    "@type",
                    ConsoleHelper._blackListAdd),

                new Command<ConsoleUI>(
                    "br",
                    "removes type of log message from blacklist",
                    "@type",
                    ConsoleHelper._blackListRemove),

                new Command<ConsoleUI>(
                    "sv",
                    "saves current session",
                    "",
                    ConsoleHelper._save),

                new Command<ConsoleUI>(
                    "on",
                    "opens new session from file",
                    "",
                    ConsoleHelper._open),
                
                #if !DEBUG
                new Command<ConsoleUI>(
                    "bat",
                    "adds type of exception to blacklist",
                    "@type",
                    ConsoleHelper._blackListAddType),

                new Command<ConsoleUI>(
                    "brt",
                    "removes exception type from blacklist",
                    "@type",
                    ConsoleHelper._blackListRemoveType),
                #endif
            },
            new Dictionary<ConsoleKey, Action<ConsoleUI>> {
                [ConsoleKey.Escape] = (ui => ui.Mode = UIMode.Messages),
            });
        }
	}
}

