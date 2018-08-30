using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MockDbConnections
{
    public class GetAccounts
    {
        public const int ID_NOT_FOUND = -1;

        IDbConnectionFactory _cxnFactory;

        string _cxnString;

        public GetAccounts(IDbConnectionFactory cxnFactory, string cxnString) 
        {
            _cxnFactory = cxnFactory;
            _cxnString = cxnString;
        }

        private int GetPortfolioId(IDbConnection cxn, string name)
        {
            int retval = ID_NOT_FOUND;

            using (var cmd = _cxnFactory.CreateCommand($"select id from portfolio where port_name = '{name}';", cxn))
            {
                try
                {
                    object val = cmd.ExecuteScalar();
                    if (val != null)
                        retval = (int)val;
                    else
                        Logger.Log($"Could not find portfolio id for '{name}'");
                }
                catch (Exception ex)
                {
                    Logger.Log("Caught exception: " + ex.Message);
                }
            }

            return retval;
        }

        public List<Account> GetAccountsForPortfolio(string portfolioName)
        {
            List<Account> retval = new List<Account>();

            using (var cxn = _cxnFactory.CreateConnection(_cxnString))
            {
                cxn.Open();

                int portId = GetPortfolioId(cxn, portfolioName);
                if (portId == ID_NOT_FOUND)
                    return retval;

                using (var cmd = _cxnFactory.CreateCommand($"select id, portfolio_id, owner_id, account_number, balance from account where portfolio_id = {portId};", cxn))
                {
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            try
                            {
                                retval.Add(new Account()
                                {
                                    AccountId = reader.GetInt32(0),
                                    PortfolioId = reader.GetInt32(1),
                                    OwnerId = reader.GetInt32(2),
                                    AccountNumber = reader.GetString(3),
                                    Balance = reader.GetDecimal(4)
                                });
                            }
                            catch (Exception ex)
                            {
                                Logger.Log("Caught parse exception: " + ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Caught read exception: " + ex.Message);
                    }
                }
            }

            return retval;
        }
    }
}
