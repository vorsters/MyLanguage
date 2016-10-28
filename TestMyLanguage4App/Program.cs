using Antlr4.Runtime;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLanguageGrammar_ANTLR4.Grammar;
using MyLanguageImpl_ANTLR4.Impl;
using MyLanguageImpl.Runtime;


namespace TestMyLanguage4App
{
    class Program
    {
        private static void Main(string[] args)
        {
            (new Program()).Run();
        }
        public void Run()
        {
            try
            {
                Console.WriteLine("START");
                RunParser();
                Console.Write("DONE. Hit RETURN to exit: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                Console.Write("Hit RETURN to exit: ");
            }
            Console.ReadLine();
        }
        private void RunParser()
        {
            string filename = @"D:\development\cvsout\MyLanguage\TestMyLanguageApp\Sample Programs\prog7_myl.myl";

            FileStream fs = new FileStream(filename, FileMode.Open);
            AntlrInputStream inputStream = new AntlrInputStream(fs);
            MyLangV4Lexer lexer = new MyLangV4Lexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            MyLangV4Parser parser = new MyLangV4Parser(commonTokenStream);

            var programContext = parser.program();

            RuntimeVisitor visitor = new RuntimeVisitor();
            //RuntimeVisitor2 visitor = new RuntimeVisitor2();
            MyProgramDeclNode program = visitor.Visit(programContext) as MyProgramDeclNode;

            MyContext runtimeContext = new MyContext();
            program.List(runtimeContext);

            Console.WriteLine("-----=====-----=====-----");

            program.Evaluate(runtimeContext);

            var runProgramNode = new MyRunProgramNode(program.Name);
            runProgramNode.Evaluate(runtimeContext);


            StringBuilder sb = new StringBuilder();

            foreach (var varkey in runtimeContext.variables.Keys)
            {
                sb.AppendLine(varkey + " => " + runtimeContext.GetVariable(varkey));
            }
            Console.WriteLine(runtimeContext.ListLog.ToString());

            Console.WriteLine(sb);

            Console.WriteLine("-----");
            Console.ReadLine(); ;

        }
    }
}
