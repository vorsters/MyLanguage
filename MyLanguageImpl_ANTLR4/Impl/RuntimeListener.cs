using Antlr4.Runtime.Misc;
using MyLanguageGrammar_ANTLR4.Grammar;


namespace MyLanguageImpl_ANTLR4.Impl
{
    public class RuntimeListener : MyLangV4ParserBaseListener
    {
        public override void EnterProgram([NotNull] MyLangV4Parser.ProgramContext context)
        {
            base.EnterProgram(context);
        }


        public override void ExitProgram([NotNull] MyLangV4Parser.ProgramContext context)
        {

            base.ExitProgram(context);
        }

        public override void EnterAssignment([NotNull] MyLangV4Parser.AssignmentContext context)
        {
            base.EnterAssignment(context);
        }

        public override void ExitAssignment([NotNull] MyLangV4Parser.AssignmentContext context)
        {
            base.ExitAssignment(context);
        }






    }
}
