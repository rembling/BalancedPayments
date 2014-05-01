using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using Newtonsoft.Json;

namespace BalancedPayments.Lib
{
    public class Collections
    {
        public static List<Account> Accounts(Settings settings)
        {
            var retVal = new List<Account>();

            var accounts_uri = string.Format("{0}/accounts", settings.marketplace_url);
            Client c = new Client(settings.location, settings.key);
            var accounts = c.get(string.Format("{0}", accounts_uri), "");
            retVal = JsonConvert.DeserializeObject<List<Account>>(accounts["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
        public static List<BankAccount> BankAccounts(Settings settings)
        {
            var retVal = new List<BankAccount>();

            var bank_accounts_uri = string.Format("{0}/bank_accounts?limit=50&is_valid=true", settings.marketplace_url);
            Client c = new Client(settings.location, settings.key);
            var bankaccounts = c.get(string.Format("{0}", bank_accounts_uri), "");
            retVal = JsonConvert.DeserializeObject<List<BankAccount>>(bankaccounts["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
        public static List<BankAccount> BankAccounts(Settings settings, string bank_accounts_uri)
        {
            var retVal = new List<BankAccount>();

            Client c = new Client(settings.location, settings.key);
            bank_accounts_uri = bank_accounts_uri + "?limit=50&is_valid=true";
            var bankaccounts = c.get(string.Format("{0}", bank_accounts_uri), "");
            retVal = JsonConvert.DeserializeObject<List<BankAccount>>(bankaccounts["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));

            return retVal;
        }
        public static List<BankAccountVerification> BankAccountVerifications(Settings settings, string bank_account_verifications_uri)
        {
            var retVal = new List<BankAccountVerification>();

            Client c = new Client(settings.location, settings.key);
            var bankaccountsverifications = c.get(string.Format("{0}", bank_account_verifications_uri), "");
            if (bankaccountsverifications.ContainsKey("items"))
            {
                retVal =
                    JsonConvert.DeserializeObject<List<BankAccountVerification>>(
                        bankaccountsverifications["items"].ToString());
                retVal.ForEach(x => x.AttachSettings(settings));
            }
            return retVal;
        }
        public static List<Card> Cards(Settings settings, string cards_uri)
        {
            var retVal = new List<Card>();

            Client c = new Client(settings.location, settings.key);
            var cards = c.get(string.Format("{0}?limit=50&is_valid=true", cards_uri), "");
            retVal = JsonConvert.DeserializeObject<List<Card>>(cards["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
        public static List<Refund> Refunds(Settings settings, string refunds_uri)
        {
            var retVal = new List<Refund>();

            Client c = new Client(settings.location, settings.key);
            var refunds = c.get(string.Format("{0}?limit=50&is_valid=true", refunds_uri), "");
            retVal = JsonConvert.DeserializeObject<List<Refund>>(refunds["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
        public static List<Credit> Credits(Settings settings, string credits_uri)
        {
            var retVal = new List<Credit>();

            Client c = new Client(settings.location, settings.key);
            var credits = c.get(string.Format("{0}?limit=50&is_valid=true", credits_uri), "");
            retVal = JsonConvert.DeserializeObject<List<Credit>>(credits["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
        public static List<Debit> Debits(Settings settings, string debits_uri)
        {
            var retVal = new List<Debit>();

            Client c = new Client(settings.location, settings.key);
            var debits = c.get(string.Format("{0}?limit=50&is_valid=true", debits_uri), "");
            retVal = JsonConvert.DeserializeObject<List<Debit>>(debits["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
        public static List<Hold> Holds(Settings settings, string holds_uri)
        {
            var retVal = new List<Hold>();

            Client c = new Client(settings.location, settings.key);
            var holds = c.get(string.Format("{0}?limit=50&is_valid=true", holds_uri), "");
            retVal = JsonConvert.DeserializeObject<List<Hold>>(holds["items"].ToString());
            retVal.ForEach(x => x.AttachSettings(settings));
            return retVal;
        }
    }
}
