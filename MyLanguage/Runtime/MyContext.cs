using System;
using System.Collections.Generic;
using System.Text;

namespace MyLang.Runtime
{
    public class MyContext
    {
        public Dictionary<string, MyFunctionDecleration> functionsRVD;

        
        public Dictionary<string, MyProgramDeclNode> programs;
        public Dictionary<string, MyFunctionDecleration> functions;
        public object returnValue;
        public bool returned;

        public Dictionary<string, IConvertible> variables; 
        
        public StringBuilder sb = new StringBuilder();

        public ListLog ListLog { get; set; }


        public MyContext()
        {
            variables = new Dictionary<string, IConvertible>();
            functions = new Dictionary<string, MyFunctionDecleration>();

            functionsRVD = new Dictionary<string, MyFunctionDecleration>();

            programs = new Dictionary<string, MyProgramDeclNode>();
            ListLog = new ListLog();
        }

        public void AddLog(string log)
        {
            sb.Append(log);
        }

        public bool ContainsVariable(string name)
        {
            return variables.ContainsKey(name);
        }

        public void AddVariable(string name, IConvertible value)
        {
            variables.Add(name, value);
        }

        public void SetVariable(string name, IConvertible value)
        {
            variables[name] = value;
        }

        public IConvertible GetVariable(string name)
        {
            return variables[name];
        }

        public void AddFunction(string name, MyFunctionDecleration func)
        {
            functions.Add(name, func);
        }

        public void AddFunctionRVD(string name, MyFunctionDecleration func)
        {
            functionsRVD.Add(name, func);
        }

        public MyFunctionDecleration GetFunction(string name)
        {
            return functions[name];
        }

        public MyFunctionDecleration GetFunctionRVD(string name)
        {
            return functionsRVD[name];
        }

        public void AddProgram(string name, MyProgramDeclNode myProgramNode)
        {
            programs.Add(name, myProgramNode);
        }

        public MyProgramDeclNode GetProgramDeclNode(string name)
        {
            return programs[name];
        }
    }
}