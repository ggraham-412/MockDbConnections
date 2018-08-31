using MockDbConnections.Mocks;
using NSubstitute;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

/// <summary>
///     Example program demonstrating use of NSubstitute with abstract classes to mock 
///     a database.
/// </summary>
namespace MockDbConnections
{
    class Program
    {
        static void Main(string[] args)
        {
            // The example is of accounts and portfolios.  A portfolio contains zero or 
            // many accounts.  An account belongs to one portfolio.

            // Mock the logger
            List<string> loggerStatements = new List<string>();
            var logImpl = Substitute.For<ILog>();
            logImpl
                .When(x => x.Log(Arg.Any<string>()))
                .Do(x => loggerStatements.Add((string)x[0]));
            Logger.LoggerImpl = logImpl;

            // Use a real connection factory
            var provider = new NPGConnectionFactory();

            var cxnString = "Host=localhost; Port=5432; Database=postgres; Username=postgres; Password=postgres;";

            // prep
            using (var cxn = provider.CreateConnection(cxnString))
            {
                cxn.Open();

                using (var cmd = provider.CreateCommand(@"select public.populatetestcase();",cxn))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            // Create the object under test
            GetAccounts getAccounts = new GetAccounts(provider, cxnString);

            // Do a bunch of positive test cases
            var accts = getAccounts.GetAccountsForPortfolio("One");
            Debug.Assert(accts.Count == 2);
            accts.ForEach(x => Debug.Assert(x.PortfolioId == 1));

            accts = getAccounts.GetAccountsForPortfolio("Two");
            Debug.Assert(accts.Count == 2);
            accts.ForEach(x => Debug.Assert(x.PortfolioId == 2));

            accts = getAccounts.GetAccountsForPortfolio("Three");
            Debug.Assert(accts.Count == 1);
            accts.ForEach(x => Debug.Assert(x.PortfolioId == 3));

            // Nothing should have been logged
            Debug.Assert(loggerStatements.Count == 0);

            // Do a negative test case
            accts = getAccounts.GetAccountsForPortfolio("Four");
            Debug.Assert(accts.Count == 0);
            Debug.Assert(loggerStatements.Count == 1);
            Debug.Assert(loggerStatements[0] == @"Could not find portfolio id for 'Four'");
            loggerStatements.Clear();
        }
    }
}
