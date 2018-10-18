// Copyright (c) 2018 Aurigma Inc. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//
using System;
using System.Globalization;

namespace Aurigma.PhotoKiosk.PhotoPrinter
{
    public class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            Console.WriteLine("Aurigma Photo Kiosk Photo Printer");
            int result = 0;

            if (args.Length != 2)
            {
                Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Error: Wrong arguments count.");
                Console.WriteLine("You should specify the following two argument:");
                Console.WriteLine("    order store folder,");
                Console.WriteLine("    maximum number of copies.");
                result = (int)ErrorCodes.InvalidArgumentsCount;
            }
            else
            {
                string orderFolder = args[0];
                int maxCopiesCount = 0;
                try
                {
                    maxCopiesCount = int.Parse(args[1]);
                }
                catch
                {
                    Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Error: Unable to parse the input argument to int.");
                    return (int)ErrorCodes.ArgumentsParsingError;
                }

                try
                {
                    PhotoPrinter photoPrinter = new PhotoPrinter(orderFolder, maxCopiesCount);

                    Console.WriteLine("\n" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Info: Start printing order " + orderFolder);
                    photoPrinter.Print();
                    Console.WriteLine("\n" + DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Info: Completed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + " Error: " + ex.Message);
                    if (ex.InnerException != null)
                        Console.WriteLine("\n Inner Exception: " + ex.InnerException.Message);
                    result = (int)ErrorCodes.UnexpectedError;
                }
            }

            return result;
        }
    }

    public enum ErrorCodes
    {
        InvalidArgumentsCount = 1,
        ArgumentsParsingError = 2,
        UnexpectedError = 3
    }
}