using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CommandInterface;
using VectorNet;

namespace VisualConsole 
{
    [Serializable]
    public class ConsoleUI
    {
        public UIMode Mode { get; set; }
        public bool Active { get; set; }

        public Interface<ConsoleUI> CommandInterface { get; set; }

        public Dictionary<ConsoleKey, Action> KeyActions { get; set; }



        #region Grid 

        public IConsolePoint[,] Grid { get; set; }

        public IntVector GridOffset 
        { 
            get 
            {
                return _gridOffset;
            }
            set 
            {
                if (CanMoveOutside
                    || ( value.X > 0
                        && value.Y > 0
                        && value.X < Grid.GetLength(0) - GridSize.X
                        && value.Y < Grid.GetLength(1) - GridSize.Y ))
                {
                    _gridOffset = value;
                }
            }
        }
        private IntVector _gridOffset;

        public IntVector GridBeginPosition 
        {
            get {
                return _gridBeginPosition;
            }

            set {
                if (value.X > Console.BufferWidth
                    || value.Y > Console.BufferHeight)
                {
                    throw new ArgumentOutOfRangeException(
                        "GridBeginPosition", "Can't be outside the buffer area");
                }

                _gridBeginPosition = value;
                GridSize = GridEndPosition - GridBeginPosition;
            }
        }
        private IntVector _gridBeginPosition;

        public IntVector GridEndPosition
        {
            get {
                return _gridEndPosition;
            }

            set {
                if (value.X > Console.BufferWidth
                    || value.Y > Console.BufferHeight)
                {
                    throw new ArgumentOutOfRangeException(
                        "GridEndPosition", "Can't be outside the buffer area");
                }

                _gridEndPosition = value;
                GridSize = GridEndPosition - GridBeginPosition;
            }
        }
        private IntVector _gridEndPosition;

        public IntVector GridSize { get; private set; }

        public bool CanMoveOutside { get; set; }

        public IConsolePoint DefaultFiller { get; set; }

        #endregion



        public string WrongCommandMessage = "Error: Wrong command";

        public ConsoleColor ErrorColor = ConsoleColor.DarkYellow;



        public ConsoleUI() 
        {
            
        }

        public ConsoleUI(
            List<Command<ConsoleUI>> commands, Dictionary<ConsoleKey, Action> keyActions)
        {
            CommandInterface = new Interface<ConsoleUI>(commands);
            KeyActions = keyActions;
        }

        public void Control()
        {
            Active = true;
            while (Active)
            {
                switch (Mode)
                {
                    case UIMode.Grid:
                        GridStep();
                        break;

                    case UIMode.Messages:
                        MessagesStep();
                        break;
                }
            }
        }

        private void GridStep()
        {
            Console.Clear();

            ConsoleUIHelper.WriteGrid(
                Grid, GridBeginPosition, GridEndPosition, GridOffset, DefaultFiller);
        }

        private void MessagesStep()
        {
            try
            {
                Console.WriteLine();

                ConsoleUIHelper.WriteMessage(
                    new string('-', Console.BufferWidth),
                    ConsoleColor.DarkGray);
                
                Console.WriteLine();

                CommandInterface.UseCommand(Console.ReadLine(), this);
            }
            catch (ArgumentException)
            {
                ConsoleUIHelper.WriteMessage(
                    WrongCommandMessage, ConsoleColor.DarkYellow);
            }
        }
    }
}
