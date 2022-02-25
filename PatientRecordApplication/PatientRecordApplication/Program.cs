using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
namespace PatientRecordApplication
{
     class Program
     {
          static void Main(string[] args)
          {
               FileOperations();
               DirectoryOperations();
               FileStreamOperations();
               SequentialAccessWriteOperation();
               ReadSequentialAccessOperation();
               FindPatients();
               SerializableDemonstration();
          }
          //File operations
          static void FileOperations()
          {
               string fileName;
               Write("Enter a filename >> ");
               fileName = ReadLine();
               if (File.Exists(fileName))
               {
                    WriteLine("File exists");
                    WriteLine("File was created " +
                       File.GetCreationTime(fileName));
                    WriteLine("File was last written to " +
                       File.GetLastWriteTime(fileName));
               }
               else
               {
                    WriteLine("File does not exist");
               }
          }
          //Directory Operations
          static void DirectoryOperations()
          {
               //Directory operations
               string directoryName;
               string[] listOfFiles;
               Write("Enter a folder >> ");
               directoryName = ReadLine();
               if (Directory.Exists(directoryName))
               {
                    WriteLine("Directory exists, and it contains the following:");
                    listOfFiles = Directory.GetFiles(directoryName);
                    for (int x = 0; x < listOfFiles.Length; ++x)
                         WriteLine("   {0}", listOfFiles[x]);
               }
               else
               {
                    WriteLine("Directory does not exist");
               }
          }
          //Using FileStream to create and write some text into it
          static void FileStreamOperations()
          {
               FileStream outFile = new
               FileStream("SomeText.txt", FileMode.Create,
               FileAccess.Write);
               StreamWriter writer = new StreamWriter(outFile);
               Write("Enter some text >> ");
               string text = ReadLine();
               writer.WriteLine(text);
               // Error occurs if the next two statements are reversed
               writer.Close();
               outFile.Close();
          }
          //Writing data to a Sequential Access text file
          static void SequentialAccessWriteOperation()
          {
               const int END = 999;
               const string DELIM = ",";
               const string FILENAME = "PatientData.txt";
               Staff emp = new Staff();
               FileStream outFile = new FileStream(FILENAME,
                  FileMode.Create, FileAccess.Write);
               StreamWriter writer = new StreamWriter(outFile);
               Write("Enter Patient ID or " + END +
                  " to quit. >> ");
               emp.PatientID = Convert.ToInt32(ReadLine());
               while (emp.PatientID != END)
               {
                    Write("Enter patient name. >> ");
                    emp.PatientName = ReadLine();
                    Write("Enter balance owed. >> ");
                    emp.BalanaceOwed = Convert.ToDouble(ReadLine());
                    writer.WriteLine(emp.PatientID + DELIM + emp.PatientName +
                       DELIM + emp.BalanaceOwed);
                    Write("Enter next patient ID or " +
                       END + " to quit. >> ");
                    emp.PatientID = Convert.ToInt32(ReadLine());
               }
               writer.Close();
               outFile.Close();
          }
          //Read data from a Sequential Access File
          static void ReadSequentialAccessOperation()
          {
               const char DELIM = ',';
               const string FILENAME = "PatientData.txt";
               Staff emp = new Staff();
               FileStream inFile = new FileStream(FILENAME,
                  FileMode.Open, FileAccess.Read);
               StreamReader reader = new StreamReader(inFile);
               string recordIn;
               string[] fields;
               WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                  "Patient ID", " Patient Name", " Balance Owed");
               recordIn = reader.ReadLine();
               while (recordIn != null)
               {
                    fields = recordIn.Split(DELIM);
                    emp.PatientID = Convert.ToInt32(fields[0]);
                    emp.PatientName = fields[1];
                    emp.BalanaceOwed = Convert.ToDouble(fields[2]);
                    WriteLine("{0,-5}{1,-12}{2,8}",
                       emp.PatientID, emp.PatientName, emp.BalanaceOwed.ToString("C"));
                    recordIn = reader.ReadLine();
               }
               reader.Close();
               inFile.Close();
          }
          //repeatedly searches a file to produce 
          //lists of patients with minimum balance
          static void FindPatients()
          {
               const char DELIM = ',';
               const int END = 999;
               const string FILENAME = "PatientData.txt";
               Staff emp = new Staff();
               FileStream inFile = new FileStream(FILENAME,
                  FileMode.Open, FileAccess.Read);
               StreamReader reader = new StreamReader(inFile);
               string recordIn;
               string[] fields;
               double minBalance;
               Write("Enter minimum balance owed to find or " +
                  END + " to quit >> ");
               minBalance = Convert.ToDouble(Console.ReadLine());
               while (minBalance != END)
               {
                    WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                       "Patient ID", " Patient Name", " Balance Owed");
                    inFile.Seek(0, SeekOrigin.Begin);
                    recordIn = reader.ReadLine();
                    while (recordIn != null)
                    {
                         fields = recordIn.Split(DELIM);
                         emp.PatientID = Convert.ToInt32(fields[0]);
                         emp.PatientName = fields[1];
                         emp.BalanaceOwed = Convert.ToDouble(fields[2]);
                         if (emp.BalanaceOwed >= minBalance)
                              WriteLine("{0,-5}{1,-12}{2,8}", emp.PatientID,
                                 emp.PatientName, emp.BalanaceOwed.ToString("C"));
                         recordIn = reader.ReadLine();
                    }
                    Write("\nEnter minimum balance owed to find or " +
                       END + " to quit >> ");
                    minBalance = Convert.ToDouble(Console.ReadLine());
               }
               reader.Close();  // Error occurs if
               inFile.Close(); //these two statements are reversed
          }
          //Serializable Demonstration
          /// <summary>
          /// writes Person class objects to a file and later reads them 
          /// from the file into the program
          /// </summary>
          static void SerializableDemonstration()
          {
               const int END = 999;
               const string FILENAME = "Data.ser";
               Patient emp = new Patient();
               FileStream outFile = new FileStream(FILENAME,
                  FileMode.Create, FileAccess.Write);
               BinaryFormatter bFormatter = new BinaryFormatter();
               Write("Enter Patient ID or " + END +
                  " to quit >> ");
               emp.PatientID = Convert.ToInt32(ReadLine());
               while (emp.PatientID != END)
               {
                    Write("Enter patient name >> ");
                    emp.PatientName = ReadLine();
                    Write("Enter balance owed >> ");
                    emp.BalanceOwed = Convert.ToDouble(ReadLine());
                    bFormatter.Serialize(outFile, emp);
                    Write("Enter patient ID or " + END +
                       " to quit >> ");
                    emp.PatientID = Convert.ToInt32(ReadLine());
               }
               outFile.Close();
               FileStream inFile = new FileStream(FILENAME,
                  FileMode.Open, FileAccess.Read);
               WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                  "Patient ID", " Patient Name", " Balance Owed");
               while (inFile.Position < inFile.Length)
               {
                    emp = (Patient)bFormatter.Deserialize(inFile);
                    WriteLine("{0,-5}{1,-12}{2,8}",
                       emp.PatientID, emp.PatientName, emp.BalanceOwed.ToString("C"));
               }
               inFile.Close();
          }
     }
}
