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

            // Maps a portfolio name to an id for the test
            Dictionary<string, int> portfolioIds = new Dictionary<string, int>()
            {
                {"One", 1 },
                {"Two", 2 },
                {"Three", 3 }
            };

            // A set of test accounts
            List<object[]> testCase = new List<object[]>();
            testCase.Add(new object[] { 1, 1, 2, "ABC123", 100.00m });
            testCase.Add(new object[] { 2, 2, 3, "DEF456", 200.00m });
            testCase.Add(new object[] { 3, 3, 2, "GHI789", 300.00m });
            testCase.Add(new object[] { 4, 2, 3, "JKL012", 400.00m });
            testCase.Add(new object[] { 5, 1, 3, "MNO345", 500.00m });

            // Mock the logger
            List<string> loggerStatements = new List<string>();
            var logImpl = Substitute.For<ILog>();
            logImpl
                .When(x => x.Log(Arg.Any<string>()))
                .Do(x => loggerStatements.Add((string)x[0]));
            Logger.LoggerImpl = logImpl;

            // Mock a connection factory
            var provider = Substitute.For<IDbConnectionFactory>();
            provider.CreateConnection(Arg.Any<string>()).Returns(
                x =>
                {
                    var retval = Substitute.For<ADbConnection>();
                    retval.ConnectionString = (string)x[0];
                    return retval;
                });
            provider.CreateCommand(Arg.Any<string>(), Arg.Any<IDbConnection>()).Returns(
                x =>
                {
                    var retval = Substitute.For<ADbCommand>();
                    retval.CommandText = (string)x[0];
                    retval.Connection = (IDbConnection)x[1];

                    // condition the command based on the SQL statement

                    // Is it asking for a portfolio id?
                    var regexGetPort = new Regex(@"select id from portfolio where port_name = '(\w+)';");
                    if ( regexGetPort.IsMatch(retval.CommandText) ) 
                    {
                        var mtGetPort = regexGetPort.Match(retval.CommandText);
                        string portName = mtGetPort.Groups[1].Value;
                        if (portfolioIds.ContainsKey(portName))
                            retval.ScalarResult = portfolioIds[mtGetPort.Groups[1].Value];
                        else
                            retval.ScalarResult = null;
                    }

                    // Or is it asking for a list of accounts?
                    var regexGetAcct = new Regex(@"select id, portfolio_id, owner_id, account_number, balance from account where portfolio_id = (\d+);");
                    if ( regexGetAcct.IsMatch(retval.CommandText))
                    {
                        var mtGetAcct = regexGetAcct.Match(retval.CommandText);
                        var readerResult = Substitute.For<ADataReader>(); 
                        readerResult.RowSet = testCase.Where(y => ((int)y[1]) == int.Parse(mtGetAcct.Groups[1].Value)).ToList();
                        retval.ReaderResult = readerResult;
                    }

                    return retval;
                });

            // Create the object under test
            GetAccounts getAccounts = new GetAccounts(provider, "ConnectionString");

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

            // Do an exception case - should skip the bad record and return the others
            testCase.Insert(0, new object[] { 6, 1, 3, "PQR678", "BadValue" });
            accts = getAccounts.GetAccountsForPortfolio("One");
            Debug.Assert(accts.Count == 2);
            Debug.Assert(loggerStatements.Count == 1);
            Debug.Assert(loggerStatements[0].StartsWith("Caught parse exception:"));
        }
    }
}
