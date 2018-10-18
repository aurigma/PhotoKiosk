// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;

namespace Aurigma.PhotoKiosk.OrderManager
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            Console.WriteLine("Aurigma Photo Kiosk Order Manager");

            if (args.Length != 1)
            {
                Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Error: Wrong arguments count.");
                Console.WriteLine("You should specify the following argument:");
                Console.WriteLine("    order folder");
                return -1;
            }

            try
            {
                var manager = new Aurigma.PhotoKiosk.Core.OrderManager.OrderManager();
                Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Info: OrderManager is initialized with the following parameters:");
                Console.WriteLine("    DestinationPath: " + manager.DestinationPath.Value);
                Console.WriteLine("    Template: " + manager.PhotoTemplate.Value);
                Console.WriteLine("    EnableCleanup: " + manager.EnableCleanup.Value);
                Console.WriteLine("    ConvertToJpeg: " + manager.ConvertToJpeg.Value);

                string orderFolder = args[0];
                Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Info: Start processing order " + orderFolder + "...");

                manager.ProcessOrder(orderFolder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Error: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("\n Inner Exception: " + ex.InnerException.Message);
                return 1;
            }

            Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Info: Finish processing order.");

            return 0;
        }
    }
}