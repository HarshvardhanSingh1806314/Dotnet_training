using System;

namespace Assignment_3
{
    enum TransactionType
    {
        Deposit = 1, Withdrawl
    }

    enum AccountType
    {
        Savings = 1, Current
    }

    internal class Accounts
    {
        private readonly long accountNo;
        private readonly string customerName;
        private int amount;
        private readonly TransactionType transactionType;
        private readonly AccountType accountType;

        public Accounts(long accountNo, string customerName, int amount, AccountType accountType)
        {
            this.accountNo = accountNo;
            this.customerName = customerName;
            this.amount = amount > 0 ? amount : 0; 
            this.accountType = accountType;
        }

        public bool PerformTransaction(TransactionType transactionType, int amount)
        {
            switch(transactionType)
            {
                case TransactionType.Deposit:
                    return this.Credit(amount);
                case TransactionType.Withdrawl:
                    return this.Debit(amount);
                default:
                    return false;
            }
        }

        private bool Credit(int amount)
        {
            if(amount >= 0)
            {
                this.amount += amount;
                return true;
            }

            return false;
        }

        private bool Debit(int amount)
        {
            if(amount <= this.amount)
            {
                this.amount -= amount;
                return true;
            }

            return false;
        }

        public int getAmount()
        {
            return this.amount;
        }

        public void DisplayAccountDetails(long accountNo)
        {
            if (accountNo == this.accountNo)
            {
                Console.WriteLine($"Account No: {this.accountNo}");
                Console.WriteLine($"Customer Name: {this.customerName}");
                Console.WriteLine($"Account Type: {this.accountType}");
                Console.WriteLine($"Account Balanace: {this.amount}");
            }
            else
                Console.WriteLine("Please Enter Correct Account Number");
        }

        public static void Main()
        {
            Console.Write("Enter Account Number: ");
            long accountNo = long.Parse(Console.ReadLine());

            Console.Write("Enter Customer Name: ");
            string customerName = Console.ReadLine();

            Console.WriteLine("Enter Account Type");
            Console.WriteLine("1 for savings account");
            Console.WriteLine("2 for current account");
            int choice = int.Parse(Console.ReadLine());

            Console.Write("Enter Amount: ");
            int amount = int.Parse(Console.ReadLine());

            // initailizing account object
            Accounts account = new Accounts(accountNo, customerName, amount, choice == 1 ? AccountType.Savings : AccountType.Current);


            // Displaying details of account
            Console.WriteLine("\nAccount Details");
            account.DisplayAccountDetails(accountNo);

            // performing a deposit transaction
            Console.WriteLine("\nPerforming Deposit Transaction");
            Console.Write("Enter Amount to deposit: ");
            int depositAmount = int.Parse(Console.ReadLine());
            string depositTransactionStatus = account.PerformTransaction(TransactionType.Deposit, depositAmount) ? "Successfull" : "Not Successfull because you entered negative deposit amount";
            Console.WriteLine($"Deposit Transaction for account number {accountNo} was {depositTransactionStatus}");
            Console.WriteLine($"Account Balance after deposit transaction: {account.getAmount()}");

            // performing a withdrawl transaction
            Console.WriteLine("\nPerforming Withdrawl Transction");
            Console.Write("Enter Amount to withdraw: ");
            int withdrawlAmount = int.Parse(Console.ReadLine());
            string withdrawlTransactionStatus = account.PerformTransaction(TransactionType.Withdrawl, withdrawlAmount) ? "Successfull" : "Not Successfull because withdrawl amount was more than account balance";
            Console.WriteLine($"Withdrawl Transaction for account number {accountNo} was {withdrawlTransactionStatus}");
            Console.WriteLine($"Account Balance after withdrawl transaction: {account.getAmount()}");

            Console.ReadLine();
        }
    }
}
