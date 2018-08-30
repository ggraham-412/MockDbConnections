using MockDbConnections.Mocks;
using NSubstitute;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;

namespace MockDbConnections
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> portfolioIds = new Dictionary<string, int>()
            {
                {"One", 1 },
                {"Two", 2 },
                {"Three", 3 }
            };

            List<object[]> testCase = new List<object[]>();
            testCase.Add(new object[] { 1, 1, 2, "ABC123", 100.00m });
            testCase.Add(new object[] { 2, 2, 3, "DEF456", 200.00m });
            testCase.Add(new object[] { 3, 3, 2, "GHI789", 300.00m });
            testCase.Add(new object[] { 4, 2, 3, "JKL012", 400.00m });
            testCase.Add(new object[] { 5, 1, 3, "MNO345", 500.00m });

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

                    var regexGetPort = new Regex(@"select id from portfolio where port_name = '(\w+)';");
                    if ( regexGetPort.IsMatch(retval.CommandText) ) 
                    {
                        var mtGetPort = regexGetPort.Match(retval.CommandText);
                        retval.ScalarResult = portfolioIds[mtGetPort.Groups[1].Value];
                    }

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

            GetAccounts getAccounts = new GetAccounts(provider, "ConnectionString");

            var accts = getAccounts.GetAccountsForPortfolio("One");
            Debug.Assert(accts.Count == 2);
            accts.ForEach(x => Debug.Assert(x.PortfolioId == 1));

            accts = getAccounts.GetAccountsForPortfolio("Two");
            Debug.Assert(accts.Count == 2);
            accts.ForEach(x => Debug.Assert(x.PortfolioId == 2));

            accts = getAccounts.GetAccountsForPortfolio("Three");
            Debug.Assert(accts.Count == 1);
            accts.ForEach(x => Debug.Assert(x.PortfolioId == 3));

        }
    }
}
