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
        bool list = false;
        bool run = false;
        bool debug = false;
        bool dump = false;
        string filename = string.Empty;


        private static void Main(string[] args)
        {
            (new Program()).Run(args);
        }
        public void Run(string [] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("usage: ");
                Console.WriteLine(" --list | -l : print the program listing as interpreted (optional)");
                Console.WriteLine(" --run  | -r : execute the program (optional)");
                Console.WriteLine(" --debug| -d : print visitor debug information (optional)");
                Console.WriteLine(" --dump | -u : dump the program variables after execution (optional)");
                Console.WriteLine(" --filename=<fullpath> | -f=<fullpath> : the 'MyLanguage' file that contains the program");
            }

            foreach (var arg in args)
            {
                if (arg == "--list" || arg == "-l")
                {
                    list = true;
                }

                if (arg == "--run" || arg == "-r")
                {
                    run = true;
                }

                if (arg == "--debug" || arg == "-d")
                {
                    debug = true;
                }

                if (arg == "--dump" || arg == "-u")
                {
                    dump = true;
                }

                if (arg.StartsWith("--filename=") || arg.StartsWith("-f="))
                {
                    filename = arg.Split('=')[1];
                    Console.WriteLine("running: {0}", filename);
                }
            }

            try
            {
                Console.WriteLine("START");
                RunParser(run, list, debug, dump, filename);
                Console.Write("DONE. Hit RETURN to exit: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
                Console.Write("Hit RETURN to exit: ");
            }
            Console.ReadLine();
        }
        private void RunParser(bool run, bool list, bool debug, bool dump, string filename)
        {

            FileStream fs = new FileStream(filename, FileMode.Open);
            AntlrInputStream inputStream = new AntlrInputStream(fs);
            MyLangV4Lexer lexer = new MyLangV4Lexer(inputStream);
            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
            MyLangV4Parser parser = new MyLangV4Parser(commonTokenStream);

            var programContext = parser.program();

            RuntimeVisitor visitor = new RuntimeVisitor(debug);
            MyProgramDeclNode program = visitor.Visit(programContext) as MyProgramDeclNode;
            MyContext runtimeContext = new MyContext();

            if (list)
                program.List(runtimeContext);


            // "compile" the program
            program.Evaluate(runtimeContext);


            if (run)
            {
                // run the the program
                var runProgramNode = new MyRunProgramNode(program.Name);
                runProgramNode.Evaluate(runtimeContext);

                // dump the program's memory - can we say heap?
                if (dump)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (var varkey in runtimeContext.variables.Keys)
                    {
                        sb.AppendLine(varkey + " => " + runtimeContext.GetVariable(varkey));
                    }

                    Console.WriteLine(sb);
                    Console.WriteLine(runtimeContext.ListLog.ToString());
                }
            }

            Console.ReadLine(); ;
        }
    }
}
