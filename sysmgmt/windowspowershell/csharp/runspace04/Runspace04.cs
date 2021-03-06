//
// Copyright (c) 2006 Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
// PARTICULAR PURPOSE.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

namespace Microsoft.Samples.PowerShell.Runspaces
{
    using PowerShell = System.Management.Automation.PowerShell;

    class Runspace04
    {
        /// <summary>
        /// This sample uses the PowerShell class to execute
        /// a script. This script will generate a terminating
        /// exception that the caller should catch and process.
        /// </summary>
        /// <param name="args">Unused</param>
        /// <remarks>
        /// This sample demonstrates the following:
        /// 1. Creating an instance of the PowerShell class.
        /// 2. Using this instance to execute a string as a PowerShell script.
        /// 3. Passing input objects to the script from the calling program.
        /// 4. Using PSObject to extract and display properties from the objects
        ///    returned by this command.
        /// 5. Retrieving and displaying error records that may be generated
        ///    during the execution of that script.
        /// 6. Catching and displaying terminating exceptions generated
        ///    by the script being run.
        /// </remarks>
        static void Main(string[] args)
        {
            // Create an instance of the PowerShell class.
            PowerShell powershell = PowerShell.Create().AddCommand("Get-ChildItem").AddCommand("Select-String").AddArgument("*");

            // Invoke the runspace. Because of the bad regular expression,
            // no objects will be returned. Instead, an exception will be
            // thrown.
            try
            {
                foreach (PSObject result in powershell.Invoke())
                {
                    Console.WriteLine("'{0}'", result.ToString());
                }

                // Now process any error records that were generated while running the script.
                Console.WriteLine("\nThe following non-terminating errors occurred:\n");
                PSDataCollection<ErrorRecord> errors = powershell.Streams.Error;
                if (errors != null && errors.Count > 0)
                {
                    foreach (ErrorRecord err in errors)
                    {
                        System.Console.WriteLine("    error: {0}", err.ToString());
                    }
                }
            }
            catch (RuntimeException runtimeException)
            {
                // Trap any exception generated by the script. These exceptions
                // will all be derived from RuntimeException.
                System.Console.WriteLine("Runtime exception: {0}: {1}\n{2}",
                    runtimeException.ErrorRecord.InvocationInfo.InvocationName,
                    runtimeException.Message,
                    runtimeException.ErrorRecord.InvocationInfo.PositionMessage
                    );
            }

            System.Console.WriteLine("\nHit any key to exit...");
            System.Console.ReadKey();
        }
    }
}

