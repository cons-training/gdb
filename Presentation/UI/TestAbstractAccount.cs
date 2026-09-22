using System;
using gdb.Data;
using gdb.Domain;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;
using gdb.Domain.Models;
using gdb.Infrastructure.Repositories.Implementations;
using System.Data;
using gdb.Presentation;

namespace gdb
{
    public class TestAccount
    {

        public static bool TransferFunds(
            Account fromAcc,
            Account toAcc,
            decimal amount,
            string pin)
        {
            try
            {
                fromAcc.Withdraw(amount, pin);
                toAcc.Deposit(amount);

                return true;
            }
            catch (AccountException ex)
            {
                Console.WriteLine($"Transfer failed: {ex.Message}");
                return false;
            }
        }

        public static void Main(string[] args)
        {

            var ds = AccountStore.CreateDataSet();

            Console.WriteLine("=== DATASET CREATED: " + ds.DataSetName + " ===");
            ListTables(ds);

            Console.WriteLine();
            PrintTableSchema(ds.Tables["ACCOUNT"]);

            Console.WriteLine();
            Console.WriteLine("=== SAMPLE QUERIES / OPERATIONS ===");

            // List first 5 account rows
            Console.WriteLine("\n-- Accounts (first 5) --");
            PrintRows(ds.Tables["ACCOUNT"], 5);


            void ListTables(DataSet ds)
            {
                Console.WriteLine("Tables in DataSet:");
                foreach (DataTable t in ds.Tables)
                {
                    Console.WriteLine($"- {t.TableName} (Rows: {t.Rows.Count})");
                }
            }

            void PrintTableSchema(DataTable table)
            {
                Console.WriteLine($"Schema for {table.TableName}:");
                foreach (DataColumn c in table.Columns)
                {
                    Console.WriteLine($"  {c.ColumnName} ({c.DataType.Name}) AllowNull={c.AllowDBNull} MaxLen={c.MaxLength}");
                }
            }

            void PrintRows(DataTable table, int maxRows = int.MaxValue)
            {
                int count = 0;
                foreach (DataRow r in table.Rows)
                {
                    PrintRow(r);
                    if (++count >= maxRows) break;
                }
            }

            void PrintRow(DataRow? row)
            {
                if (row == null)
                {
                    Console.WriteLine("  <null>");
                    return;
                }

                var values = row.Table.Columns.Cast<DataColumn>().Select(c => $"{c.ColumnName}={row[c]}");
                Console.WriteLine("  " + string.Join(", ", values));
            }

            new Home().Start();
        }
    }
}