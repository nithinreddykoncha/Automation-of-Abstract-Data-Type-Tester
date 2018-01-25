using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

namespace AbstractDataTypeTester
{
    class Program
    {
        static StreamReader sr = null;

        static string TimeStamp()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        static string skipBlankLines(string line)
        {
            while (true)
            {
                if (line == null)
                    return null;
                else if (line.Trim() == "")//skips blank lines
                    line = sr.ReadLine();
                else
                    return line.Trim();
            }
        }

        static int Main(string[] args)
        {
            #region Variable Declaration
            string logs = ConfigurationManager.AppSettings["logFile"].Trim();
            StreamWriter logFile = null;
            StreamWriter routeFile = null;
            try
            {
                logFile = new StreamWriter(logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading log file path from config file" + ex.Message);
                Console.ReadKey();
                return -1;
            }
            logFile.WriteLine("INFO :: Program started");
            int retVal = 0;
            int retryCount = 0;
            logFile.WriteLine("INFO :: Reading values from the configuration file");
            string showRouting = ConfigurationManager.AppSettings["showRouting"].ToUpper().Trim();
            string routingFile = ConfigurationManager.AppSettings["routingFile"].ToUpper().Trim();
            string outFile = ConfigurationManager.AppSettings["outputFile"].Trim();
            string inputFile = ConfigurationManager.AppSettings["inputFile"].Trim();
            string globalState = ConfigurationManager.AppSettings["globalState"].Trim();
            if (outFile == "" || inputFile == "" || globalState == "")
            {
                logFile.WriteLine("ERROR :: Error reading values from config file");
                Console.WriteLine("Error reading values from config file");
                Console.ReadKey();
                return -1;
            }
            bool writeToRouteFile = false;
            if (routingFile == "")
            {
                logFile.WriteLine("INFO :: Option not to write the routing paths selected");
                routeFile = null;
            }
            else
            {
                routeFile = new StreamWriter(routingFile);
                logFile.WriteLine("INFO :: Option to write the routing paths selected");
                logFile.WriteLine("INFO :: The routing paths are written to " + routingFile);
                writeToRouteFile = true;
            }
            if (showRouting.ToLower() == "yes")
                logFile.WriteLine("INFO :: Option to show the routing path in console selected");
            else
                logFile.WriteLine("INFO :: Option not to show the routing path in console selected");
            int visitedCount = 0;
            try
            {
                visitedCount = Convert.ToInt32(ConfigurationManager.AppSettings["visitedCount"]);
                if (visitedCount <= 0)
                {
                    logFile.WriteLine("ERROR :: Visited count is 0 or less than 0, none of the states visited. Exiting code without reading input file");
                    Console.WriteLine("Visited count is 0 or less than 0, none of the states visited. Exiting code without reading input file");
                    Console.ReadKey();
                    return -1;
                }
                else
                    logFile.WriteLine("INFO :: Each state defined in the input file will be visited exxactly  " + visitedCount + " times");
            }
            catch (Exception ex)
            {
                logFile.WriteLine("ERROR :: Error reading visitedcount from config file" + ex.Message);
                Console.WriteLine("Error reading visitedcount from config file");
                Console.ReadKey();
                return -1;
            }
            logFile.WriteLine("INFO :: All the values from config file read successfully");
            string startTime = TimeStamp();
            bool uses = true;
            bool states = false;
            bool transactions = false;
            string line = "";//variable to read line from the input file
            string storageInit = "";//variable to store initial of the generic collection used in the input file
            string storage = "";//variable to store the value of the generic collection used in the input file
            string functionName;
            string functionVar = "";
            List<string> stateType = new List<string>();//types of states
            List<string> actions = new List<string>();//types of actions listed in input file
            List<string> uniqueActions = new List<string>();//types of unique actions listed in input file
            string[,] routeMatrix = null;
            logFile.WriteLine("INFO :: Initializing stream writers");
            StreamWriter sw = new StreamWriter(outFile);
            string initiation = null;
            #endregion
            try
            {
                using (sr = new StreamReader(inputFile))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        line = skipBlankLines(line);
                        #region Reading C++ code from the input file and writing it to the output file
                        try
                        {
                            if (uses == true)
                            {
                                if (line.ToLower() == "states")
                                {
                                    logFile.WriteLine("INFO :: Headers and Variable initiation read successfully");
                                    uses = false;
                                    states = true;
                                    storage = skipBlankLines(sr.ReadLine());
                                    storageInit = storage.Substring(0, 1);
                                    logFile.WriteLine("INFO :: Creating Verify() method");//Creating a verify() method in output file
                                    sw.WriteLine();
                                    sw.WriteLine("void Verify(string S, bool B)");
                                    sw.WriteLine("{");
                                    sw.WriteLine("\tif (!B){");
                                    sw.WriteLine("\t\tcout << \"Rule Failed: \"<< S << endl;");
                                    sw.WriteLine("\t}");
                                    sw.WriteLine("\telse {");
                                    sw.WriteLine("\t\tcout << \"Rule Succeeded: \"<< S << endl;");
                                    sw.WriteLine("\t}");
                                    sw.WriteLine("}");//End of verify() method
                                    logFile.WriteLine("INFO :: Verify() method created successfully");
                                    logFile.WriteLine("INFO :: Reading the definition of the states");
                                }
                                else
                                {
                                    sw.WriteLine(line);
                                    initiation = line;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            retVal = 1;
                            retryCount++;
                            logFile.WriteLine("ERROR :: " + ex.Message);
                            break;
                        }
                        try
                        {
                            if (states == true)
                            {
                                while ((line = skipBlankLines(sr.ReadLine())) != null)
                                {
                                    if (line.ToLower() == "endstates") //End of the state definition
                                    {
                                        logFile.WriteLine("INFO :: Definition of the states read successfully");
                                        states = false;
                                        transactions = true;
                                        break;
                                    }
                                    else //Reading the state definition
                                    {
                                        sw.WriteLine();
                                        functionName = line.Substring(0, line.IndexOf(')') + 1).Trim();
                                        functionVar = line.Substring(line.Trim().IndexOf('(') + 1, line.Trim().IndexOf(')') - (line.Trim().IndexOf('(') + 1)).Trim();
                                        if (functionName.Contains(globalState)) //Checking for a global state
                                            logFile.WriteLine("INFO :: Reading global state definition.");
                                        else
                                            stateType.Add(functionName.Replace(":", "").Trim());
                                        sw.WriteLine("bool " + functionName.Substring(0, functionName.IndexOf('(')) + "(" + storage + " " + functionVar + "){");
                                        sw.WriteLine("bool b;");
                                        sw.WriteLine("b = true;");
                                        line = skipBlankLines(sr.ReadLine());
                                        string[] stateDefinitions = line.Split(',');
                                        foreach (var stateDefinition in stateDefinitions)
                                            sw.WriteLine("b = (" + stateDefinition + ") && b;");
                                        sw.WriteLine("return b;");
                                        sw.WriteLine("}");
                                        logFile.WriteLine("INFO :: Method for the state " + functionName + " created successfully");
                                        sw.WriteLine();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            retVal = 2;
                            retryCount++;
                            logFile.WriteLine("ERROR :: " + ex.Message);
                            break;
                        }
                        try
                        {
                            int indexOfRow = 0;
                            int indexOfCol = 0;
                            if (transactions == true)
                            {
                                routeMatrix = new string[stateType.Count, stateType.Count];
                                string[] str;
                                sr.ReadLine();//skipping the line
                                sw.WriteLine("void Rules( " + storage.Trim() + " T" + "){");
                                sw.WriteLine("\t" + functionVar + " = T;");
                                sw.WriteLine("\tbool validState = false;" + Environment.NewLine);
                                logFile.WriteLine("INFO :: Reading state transition");
                                while ((line = skipBlankLines(sr.ReadLine())) != null)
                                {
                                    if (line.ToLower() == "endtransitions")
                                    {
                                        logFile.WriteLine("INFO :: Input file read successfully");
                                        logFile.WriteLine("INFO :: Creating 2D Matrix to store the path between two states");
                                    }
                                    else
                                    {
                                        int posOpen = line.IndexOf("{");
                                        int posClose = line.IndexOf("}");
                                        string str1 = line.Substring(0, posOpen - 1).Replace(functionVar, "T").Trim();
                                        string str2 = line.Substring(posOpen + 1, (posClose - posOpen) - 1).Trim();
                                        string str3 = line.Substring(posClose + 2).Replace(functionVar, "T").Trim();
                                        sw.WriteLine("\tif(" + str1 + ") {");
                                        str = str2.Split(';');
                                        foreach (var item in str)
                                        {
                                            if (item.ToLower().Trim().Contains("writeln"))
                                                sw.WriteLine("\t" + item.Replace("writeln", "cout << \"").Replace("('", "").Replace("')", "\" << endl;"));
                                            else
                                            {
                                                sw.WriteLine("\tT" + item.Trim().Substring(1) + ";");
                                                str2 = item.Trim();
                                                sw.WriteLine("\tcout << \"Applying " + item.Trim() + " on " + str1 + " \" << endl;");
                                            }
                                        }
                                        actions.Add(str2);
                                        sw.WriteLine("\tVerify(\"" + str3 + "\"," + str3 + ");");
                                        sw.WriteLine("\tT = " + functionVar + ";");
                                        sw.WriteLine("\tvalidState = true;");
                                        sw.WriteLine("\t}" + Environment.NewLine);
                                        str1 = str1.Replace("&&", "~").Replace("||", "~").Split('~')[0].Replace("T", functionVar).Trim();
                                        str3 = str3.Replace("&&", "~").Replace("||", "~").Split('~')[0].Replace("T", functionVar).Trim();
                                        indexOfRow = stateType.IndexOf(str1);
                                        indexOfCol = stateType.IndexOf(str3);
                                        routeMatrix[indexOfRow, indexOfCol] = str2;
                                    }
                                }
                                sw.WriteLine("\tif(!validState)");
                                sw.WriteLine("\t{");
                                sw.WriteLine("\t\tcout << \"Rule Failed: None of the state definition met\" << endl;");
                                sw.WriteLine("\t}");
                                sw.WriteLine("}");
                            }
                        }
                        catch (Exception ex)
                        {
                            retVal = 3;
                            retryCount++;
                            logFile.WriteLine("ERROR :: " + ex.Message);
                            break;
                        }
                    }
                    #endregion
                }
                if (retVal == 0)///executes further only if the above code worked fine!!!
                {
                    Console.WriteLine("ROUTE MATRIX");
                    Console.WriteLine("------------");
                    logFile.WriteLine("INFO :: Matrix(" + stateType.Count + ", " + stateType.Count + ") to store the path between two states is created successfully");
                    for (int y = 0; y < stateType.Count; y++)
                    {
                        Console.Write("\t\t\t" + stateType[y]);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    //Display Matrix
                    for (int x = 0; x < stateType.Count; x++)
                    {
                        for (int y = 0; y < stateType.Count; y++)
                        {
                            if (routeMatrix[x, y] == null)
                                routeMatrix[x, y] = "-1";
                        }
                    }
                    for (int x = 0; x < stateType.Count; x++)
                    {
                        Console.Write(stateType[x] + "\t\t\t");
                        for (int y = 0; y < stateType.Count; y++)
                        {
                            Console.Write(routeMatrix[x, y] + "\t\t\t");
                        }
                        Console.WriteLine();
                    }
                    //Making the null values to V i.e. Visited
                    for (int x = 0; x < stateType.Count; x++)
                    {
                        for (int y = 0; y < stateType.Count; y++)
                        {
                            if (x == y)
                                routeMatrix[x, y] = "V";
                            if (routeMatrix[x, y] == "-1")
                                routeMatrix[x, y] = "V";
                        }
                    }
                    logFile.WriteLine("INFO :: Creating Main() method");
                    sw.WriteLine("void main(){");
                    sw.WriteLine("\t" + storage + " T;");
                    sw.WriteLine("\tT = " + functionVar + ";");
                    string route =  stateType[0];
                    int[] visited = new int[stateType.Count];
                    List<string> routes = new List<string>();
                    bool writeMainMethodStr = false;
                    string[,] routeMatrix1 = null;
                    routeMatrix1 = routeMatrix.Clone() as string[,];
                    for (int x = 0; x < stateType.Count; x++)
                    {
                        for (int y = 0; y < stateType.Count; y++)
                        {
                            if (routeMatrix[x, y] != null && routeMatrix[x, y] != "V")
                            {
                                if (x == y)//self loops are visited and ignored in the main method
                                    routeMatrix[x, x] = "V";
                                else
                                {   route += "," + stateType[y];
                                    int b = route.Split(',').Length;
                                    if (route.Split(',').Length == stateType.Count)
                                    {
                                        routes.Clear();
                                        routes.Add(route);
                                        x = y = stateType.Count+1;
                                        break;
                                    }
                                    for (int m = 0; m < stateType.Count; m++)
                                    {
                                        if (routeMatrix[y, m] == null || y == m || (route.Contains(stateType[m])))
                                            routeMatrix[y, m] = "V";
                                        if (routeMatrix[y, m] != "V")
                                        {
                                            writeMainMethodStr = true;
                                            break;
                                        }
                                    }
                                    if (!writeMainMethodStr)
                                    {
                                        writeMainMethodStr = true;
                                        routeMatrix[x, y] = "V";
                                        if (routes.Count>0)
                                        {
                                            foreach (var item in routes)
                                            {
                                                if (item.Contains(route))
                                                {
                                                    writeMainMethodStr = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (writeMainMethodStr)
                                            routes.Add(route);
                                        route = stateType[0];
                                        x = -1;
                                        writeMainMethodStr = false;
                                        break;
                                    }
                                    else
                                    {
                                        x = y;
                                        y = -1;
                                        writeMainMethodStr = false;
                                    }
                                }
                            }
                            else//empty matrices will be visited and ignored in the main method
                                routeMatrix[x, y] = "V";
                        }
                    }
                    Console.WriteLine();
                    if (showRouting == "YES")
                    {
                        Console.WriteLine("The different routes between the states is listed below.");
                        foreach (var item in routes)
                        {
                            Console.WriteLine(item.Replace(",", " ------>> "));
                        }
                    }
                    if (routeFile != null)
                    {
                        foreach (var item in routes)
                        {
                            routeFile.WriteLine(item.Replace(","," ------>> "));
                        }
                    }
                    try
                    {
                        sw.WriteLine("\t" + "Rules(" + functionVar + ");");
                        string[] routeItems;
                        int x, y = 0;
                        foreach (var item in routes)
                        {
                            routeItems = item.Split(',');
                            x = -1; y = -1;
                            Console.WriteLine();
                            foreach (var item1 in routeItems)
                            {
                                if (x == -1)
                                    x = stateType.IndexOf(item1);
                                else
                                {
                                    y = stateType.IndexOf(item1);
                                    sw.WriteLine("\t" + routeMatrix1[x, y] + ";");
                                    sw.WriteLine("\t" + "Rules(" + functionVar + ");");
                                    x = y;

                                }
                            }
                            sw.WriteLine();
                            sw.WriteLine("\t" + functionVar + " = T;");
                        }
                        sw.WriteLine("}");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine();
                    }
                }
                else
                    logFile.WriteLine("Error :: Error encountered at the time of reading the input file");
                Console.ReadKey();
                return retVal;
            }
            catch (Exception ex)
            {
                retryCount++;
                logFile.WriteLine("ERROR :: " + ex.Message);
                retVal = 4;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sw.Close();
                logFile.WriteLine("INFO :: Program executed successfully with error code " + retVal);
                logFile.Close();
                if (writeToRouteFile)
                    routeFile.Close();
            }
            return retVal;
        }
    }
}