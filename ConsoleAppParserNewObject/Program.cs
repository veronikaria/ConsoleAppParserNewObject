using System;
using System.Text.RegularExpressions;

namespace ConsoleAppParserNewObject
{
    class Program
    {
        static void Main(string[] args)
        {
            //string e = @"([d-f]+) ([d-f]{1})([1-3]{1}) (=) (new) ([d-f]+)\(((([d-f]{1})([1-3]{1}))*)((, ((([d-f]{1})([1-3]{1}))))*)\);";
            string prs, answer;
            while (true)
            {
                Console.Write("Enter string for analyze: ");
                prs = Console.ReadLine(); // строка от пользователя
                try
                {
                    Parser parser = new Parser(prs); // создание объекта анализатора, передаем строку в анализатор как параметр конструктора
                    parser.Parse(); // выполняем синтаксический анализ строки
                }
                catch (ParserException ex) // ловим кастомную ошибку ParserException, которая определена в файле ParserException.cs
                {
                    Console.WriteLine(ex); // выводим объект ошибки (переопределен метод ToString)
                }

                Console.Write("Do you want to continue? yes/no: ");  
                answer = Console.ReadLine(); // продолжаем ли ввод
                if (answer.ToLower() == "no" || answer.ToLower() == "n")  
                    break;
            }

            Console.ReadLine();
        }
    }


}
