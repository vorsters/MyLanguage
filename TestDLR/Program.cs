using Antlr4.Runtime;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLanguageGrammar_ANTLR4.Grammar;
using MyLanguageImpl_ANTLR4.Impl;
using System.Linq.Expressions;

namespace TestDLR
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"D:\development\cvsout\MyLanguage\TestMyLanguageApp\Sample Programs\hello.myl";

            FileStream fs = new FileStream(filename, FileMode.Open);
            AntlrInputStream inputStream = new AntlrInputStream(fs);
            MyLangV4Lexer lexer = new MyLangV4Lexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            MyLangV4Parser parser = new MyLangV4Parser(commonTokenStream);

            var programContext = parser.program();

            RuntimeVisitorDLR visitor = new RuntimeVisitorDLR(true);

            Expression e = visitor.Visit(programContext);


            Action callDelegate = Expression.Lambda<Action>(e).Compile();
            callDelegate();

            //Console.WriteLine(Expression.Lambda<Func<int>>(e).Compile()());


        }
    }
}
