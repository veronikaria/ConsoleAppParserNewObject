using System;

namespace ConsoleAppParserNewObject
{
    class ParserException : ApplicationException
    {
        string message; // сообщение, которое будет приходить вторым параметром при создании объекта
        public ParserException(string str, string mess) : base(str) 
        {
            this.message = mess;
        }
        public override string ToString()
        { return Message + message; }
    }
}
