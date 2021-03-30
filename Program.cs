using System;

namespace VendingMachine
{
    class Program
    {

        static FinanceManager financeMgr = new FinanceManager();
        static ProductManager productMgr = new ProductManager();

        static void Main(string[] args)
        {
            initInventory();
            while (true)
            {
                Console.WriteLine("Are you restocking or buying?");
                string modeSelection = Console.ReadLine();
                if (modeSelection == "restocking")
                {
                    Console.WriteLine("Please enter the password");
                    //the password is password
                    string password = Console.ReadLine();
                    if (password == "password")
                    {
                        productMgr.ClearStocks();  // Clears all the stocks
                        initInventory();     // re-stocks everything
                        Console.WriteLine("You have now re-stocked the machine!");
                        System.Threading.Thread.Sleep(5000);
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password");
                    }
                }
                else if (modeSelection == "buying")
                {
                    productMgr.PrintMenu(financeMgr);

                    // Enter money
                    int insertedCoins = financeMgr.AcceptCoins();

                    Console.WriteLine("Current balance: {0}.", financeMgr.Format(insertedCoins));
                    productMgr.PrintMenu(financeMgr);
                    Console.WriteLine("Please select the product you want");
                    int selectedSlot;
                    while (true)
                    {
                        string slotInput = ReadInput("Enter a product slot");

                        if (Int32.TryParse(slotInput, out selectedSlot) && productMgr.IsSlotValid(selectedSlot))
                            break; // Slot is valid, exit while loop
                        Console.WriteLine("Entered slot number is invalid. Please try again.");
                    }
                    var product = productMgr.GetProduct(selectedSlot);
                    var refund = insertedCoins;
                    var remaining = insertedCoins - product.Price;

                    if (product.Price > insertedCoins)
                    {
                        Console.WriteLine("You cannot buy this product, insufficient funds");
                        Console.WriteLine("You have been refunded {0}.", financeMgr.Format(refund));
                    }
                    else
                    {
                        if (productMgr.Dispense(selectedSlot))
                        {
                            Console.WriteLine("Your change is {0}.", financeMgr.Format(remaining));
                        }
                        else
                        {
                            Console.WriteLine("You have been refunded {0}.", financeMgr.Format(refund));
                        }
                    }
                }


                else
                {
                    Console.WriteLine("Unknown mode!");
                }





            }
        }

        static void initInventory()
        {
            productMgr.StockItem(new Product("Coca-Cola", 125, 10));
            productMgr.StockItem(new Product("Pepsi", 125, 10));
            productMgr.StockItem(new Product("Sprite", 125, 10));
            productMgr.StockItem(new Product("Orange Juice", 105, 10));
            productMgr.StockItem(new Product("Diet Coke", 115, 10));
        }
        public static string ReadInput(string msg)
        {
            if (msg != null)
                Console.Write(msg + ": ");
            return Console.ReadLine().Trim().ToLower();

        }

    }
}

