using System.Collections.Generic;

namespace Ragnar.Mock.Dispatcher
{
    public class CommandResponse : ICommandResponse
    {
        public bool IsAuthorized { get; set; }

        public List<Validation.ValidationError> Errors { get; set; }

        public bool IsValid
        {
            get { return Errors == null || Errors.Count == 0; }
        }

        public object Content { get; set; }

        public CommandResponse()
        {
            IsAuthorized = false;
        }
    }
}