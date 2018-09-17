using System;

namespace lunge.Library.Input
{
    public class InputCommandAlreadyRegisteredException : Exception
    {
        public Action InputCommand { get; }

        public InputCommandAlreadyRegisteredException(Action inputCommand)
        {
            InputCommand = inputCommand;
        }
    }
}
