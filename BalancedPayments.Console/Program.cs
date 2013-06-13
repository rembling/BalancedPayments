using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BalancedPayments.Lib.core;
using BalancedPayments.Lib;

namespace BalancedPayments.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BalancedService service = new BalancedService("1",
                    "https://api.balancedpayments.com",
                    "[YOUR API KEY]",
                    "/v1/marketplaces/[YOUR MARKETPLACE ID]"); 

                Console.WriteLine(string.Format("Marketplace: {0}", service.Marketplace.name));

                // test buyer account
                string testBuyerName = "Test Buyer";
                string testBuyerEmail = "test@test.com";
                string existing_account_id = "ACI1SOWvjTPkhcnENvrLoKc"; // user your account id
                string existing_account_uri = string.Format("{0}/accounts/{1}", service.Settings.marketplace_url, existing_account_id);
                var testBuyerAccount = service.Marketplace.GetAccount(existing_account_uri);
                if (testBuyerAccount.uri != null)
                {
                    Console.WriteLine(string.Format("Got buyer: {0}", testBuyerAccount.name));
                }
                else
                {
                    Console.WriteLine(string.Format("Could not find buyer by existing uri: {0}", existing_account_uri));
                    var newTestBuyerAccount = service.Marketplace.CreateAccount(testBuyerName, testBuyerEmail, null);
                    if (newTestBuyerAccount.uri != null)
                    {
                        Console.WriteLine(string.Format("Created buyer: {0}", testBuyerName));
                        testBuyerAccount = newTestBuyerAccount;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Could not create Test buyer, the email address ({0}) may be assigned to a different account", testBuyerEmail));
                    }
                }

                // tokenize card
                // good cards: 5105105105105100 123, 4111111111111111 123, 341111111111111 1234
                // bad cards (tokenize failure): 4222222222222220 123
                // bad cards (process failure): 4444444444444448 123
                var card = service.Marketplace.TokenizeCard(testBuyerName, "5105105105105100", "123", 1, 2014);
                if (card.uri != null)
                {
                    Console.WriteLine(string.Format("Tokenized card: {0}, {1}", card.last_four, card.uri));

                    // associate card with test account
                    testBuyerAccount.AssociateCard(card.uri);
                    Console.WriteLine("Associated credit card with test buyer account");

                    // charge credit card
                    var debit = testBuyerAccount.Debit(Convert.ToInt32(2000), "$20 Concert Ticket", card.uri, "Acme Online Inc.", null);
                    Console.WriteLine("Charged credit card");

                    // refund the transaction
                    var refund = testBuyerAccount.Refund(Convert.ToInt32(1000), "$10 Concert Ticket Refund", null, debit.uri);
                    Console.WriteLine("Refunded credit card");
                }

                // test buyer merchant
                string testMerchantName = "Test Merchant";
                string testMerchantEmail = "testMerchant@test.com";
                string existing_account_id2 = "AC5BpUDiWn3PsHUBYfygwPfW"; // user your merchant account id
                string existing_account_uri2 = string.Format("{0}/accounts/{1}", service.Settings.marketplace_url, existing_account_id2);
                var testMerchantAccount = service.Marketplace.GetAccount(existing_account_uri2);
                if (testMerchantAccount.uri != null)
                {
                    Console.WriteLine(string.Format("Got merchant: {0}", testMerchantAccount.name));
                }
                else
                {
                    Console.WriteLine(string.Format("Could not find merchant by existing uri: {0}", existing_account_uri2));
                    var newTestMerchantAccount = service.Marketplace.CreateAccount(testMerchantName, testMerchantEmail, null);
                    if (newTestMerchantAccount.uri != null)
                    {
                        Console.WriteLine(string.Format("Created merchant: {0}", testMerchantName));
                        testMerchantAccount = newTestMerchantAccount;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Could not create Test merchat, the email address ({0}) may be assigned to a different account", testMerchantEmail));
                    }
                }

                // tokenize bank account
                var bankAccount = service.Marketplace.TokenizeBankAccount(testMerchantName, "555-8002-555", "555555559");
                Console.WriteLine("Tokenized bank account");

                testMerchantAccount.AssociateBankAccount(bankAccount.uri);
                Console.WriteLine("Associated bank account with test merchant account");

                testMerchantAccount.Credit(Convert.ToInt32(2000), "$20 Merchant Payment", bankAccount.uri, "Acme Online Inc.", null);
                Console.WriteLine("Paid merchant account");

                // delete a credit card
                //card.is_valid = false;
                //card.save();
                //Console.WriteLine("credit card was deleted");

                // delete bank account (TODO: THIS DOES NOT APPEAR TO WORK)
                //bankAccount.meta.Add("is_valid", "false");
                //bankAccount.save();
                //Console.WriteLine("bank account was deleted");

                // delete account (TODO: THIS DOES NOT APPEAR TO WORK: possibly need to "invalidate" instead)
                //var accountUriToDelete = testBuyerAccount.uri;
                //var accountUriToDelete = string.Format("{0}/accounts/{1}", service.Settings.marketplace_url, "AC5V88LLansmlrf7hrdiJW3K");
                //var accountToDetele = service.Marketplace.GetAccount(accountUriToDelete);
                //if (accountToDetele.uri != null)
                //{
                //    accountToDetele.delete();
                //    Console.WriteLine(string.Format("Deleted account: {0}", accountToDetele.name));
                //}
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
