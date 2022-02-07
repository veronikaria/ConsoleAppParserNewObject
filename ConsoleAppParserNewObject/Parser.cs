using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleAppParserNewObject
{
    class Parser
    {
        string s;
        List<string> expr = new List<string>();
        string temp = "";

        bool isRight; // переменная отвечает за то, прошел ли успешно синтаксический анализ 
        static readonly Regex repl = new Regex(@"\s\s+"); // паттерн для нескольких пробелов
        string[] pattrens = new string[] { @"([d-f]+)", @"([d-f]{1})([1-3]{1})", @"(=)", @"(new)", @"([d-f]+)\(((([d-f]{1})([1-3]{1}))*)((,((([d-f]{1})([1-3]{1}))))*)\);" }; // паттерны которые понадобятся для проверки. 
        
        enum Errors { REFERENCE, NAME, SYNTAX, TYPE, CLASSNAME, PARAMETERS, ENDEXPRESSION }; // перечисление ошибок связанные с несоответствием паттерну
        public Parser(string str) 
        {
            s = repl.Replace(str.Trim(), " "); // str.Trim() - удаление пробелов с концов строки, Replace - замена несколькиз пробелов одним
            foreach (var item in s.Split(new char[] { ' ' }))  // разбиваем строку по пробелах и добавляем в список
                expr.Add(item);
            isRight = true; 
        }

        public void Parse() // сам синтаксический анализ
        {
            int index = 4; 
            if (expr.Count < 5)  
            {
                if (expr.Count > 0 && !Regex.IsMatch(expr[0], pattrens[3])) // проверка строки expr[0] на несоответствие pattrens[3]
                {
                    SyntaxErr(Errors.SYNTAX, "\nCheck near expression: " + expr[2]); 
                    isRight = false;
                }
                else
                    index = 1;
            }
            else 
            {
                if (!Regex.IsMatch(expr[0], pattrens[0])) // проверка строки expr[0] на несоответствие pattrens[0] (название класса, то есть тип объекта)
                {
                    SyntaxErr(Errors.REFERENCE, "\nCheck near expression: " + expr[0]);
                    isRight = false;
                }
                if (!Regex.IsMatch(expr[1], pattrens[1])) // проверка строки expr[1] на несоответствие pattrens[1] (название переменной)
                {
                    SyntaxErr(Errors.NAME, "\nCheck near expression: " + expr[1]);
                    isRight = false;
                }
                if (!Regex.IsMatch(expr[2], pattrens[2])) // проверка строки expr[2] на несоответствие pattrens[2] ( знак равно )
                {
                    SyntaxErr(Errors.SYNTAX, "\nCheck near expression: " + expr[2]);
                    isRight = false;
                }
                if (!Regex.IsMatch(expr[3], pattrens[3]))   // проверка строки expr[3] на несоответствие pattrens[3] ( ключевое слово new для создания )
                {
                    SyntaxErr(Errors.SYNTAX, "\nCheck near expression: " + expr[3]);
                    isRight = false;
                }

            }
            
            for (int i = index; i < expr.Count; i++) // запись в temp строк типу: dd(d1,d2,d3); - то есть запись всего что после new
            {
                temp += expr[i];
            }
            if (Regex.IsMatch(temp, @"\S*\(\S*\)\S*")) // если внутри строки есть '(' и ')'
            {
                string class_name = temp.Substring(0, temp.IndexOf('(')); // берем название класса
                if (index == 4 && class_name!=expr[0])  // сравниваем название класса с тем, обьект какого типа создавался, это сделано для того чтобы нельзя было написать dd e1 = new ff(); а правильно будет: dd e1 = new dd(); 
                {
                    SyntaxErr(Errors.CLASSNAME, "\nCheck near expression: " + class_name);
                    isRight = false;
                }
            }
            if (!Regex.IsMatch(temp, @"\S*;")) // если нету ';'
            {
                SyntaxErr(Errors.ENDEXPRESSION, "\nYou forgot ';'. Check near expression:" + temp);
                isRight = false;
                
            }
            if (!Regex.IsMatch(temp, pattrens[4])) // проверка параметров
            {
                SyntaxErr(Errors.PARAMETERS, "\nCheck near expression: " + temp);
                isRight = false;
            }

            if (isRight)
                Console.WriteLine("All is right");

        }

        void SyntaxErr(Errors error, string mess)
        {
            string[] err ={
                         "Ссылочная ошибка",
                         "Ошибка название ебъекта",
                         "Синтаксическая ошибка",
                         "Ошибка типов",
                         "Ошибка в названии нового объекта",
                         "Ошибка параметров конструктора",
                         "Ошибка конца строки"
            };
            throw new ParserException(err[(int)error], mess);
        }
    }
}
