using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;


namespace SampleSQLVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader txtRdr = new StreamReader("foo.sql");
            TSql110Parser parser = new TSql110Parser(true);


            IList<ParseError> errors;
            TSqlFragment sqlFragment = parser.Parse(txtRdr, out errors);
            // TODO report the parsing errors generated (if any)


            SQLVisitor myVisitor = new SQLVisitor();
            sqlFragment.Accept(myVisitor);


            myVisitor.DumpStatistics();
        }
    }


    internal class SQLVisitor : TSqlFragmentVisitor
    {
        private int SELECTcount = 0;
        private int INSERTcount = 0;
        private int UPDATEcount = 0;
        private int DELETEcount = 0;


        private string GetNodeTokenText(TSqlFragment fragment)
        {
            StringBuilder tokenText = new StringBuilder();
            for (int counter = fragment.FirstTokenIndex; counter <= fragment.LastTokenIndex; counter++)
            {
                tokenText.Append(fragment.ScriptTokenStream[counter].Text);
            }


            return tokenText.ToString();
        }


        // SELECTs
        public override void ExplicitVisit(SelectStatement node)
        {
            //Console.WriteLine("found SELECT statement with text: " + GetNodeTokenText(node));
            SELECTcount++;
        }


        // INSERTs
        public override void ExplicitVisit(InsertStatement node)
        {
            INSERTcount++;
        }


        // UPDATEs
        public override void ExplicitVisit(UpdateStatement node)
        {
            UPDATEcount++;
        }


        // DELETEs
        public override void ExplicitVisit(DeleteStatement node)
        {
            DELETEcount++;
        }


        public void DumpStatistics()
        {
            Console.WriteLine(string.Format("Found {0} SELECTs, {1} INSERTs, {2} UPDATEs & {3} DELETEs",
                this.SELECTcount,
                this.INSERTcount,
                this.UPDATEcount,
                this.DELETEcount));
        }
    }
}
