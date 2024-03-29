﻿//////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

/// date         developer       changes
/// 1.25.2024    Schlecht, C     Inital creation of this Program.cs file, Student.cs file, began creating objects
///                              class Student
///  2.1.2024    Schlecht c     
///  2.5.2024    Schlecht c      added more keys
///  2.8.2024    Schlecht c      added reading from input file
///  2.8.2024    Schlecht c      tuesday
///  2.20.2024   Schlecht c      changed output file to input file in private const string output file,
///                              added spaces in between ToStringForOutputFile for undergrad and graduate print outs
///                              added backdoor
///                              
namespace StudentDbApp
{
    //this will represent the application itself
    //known in OOP "patterns" as a "singleton" object pattern
    internal class DbApp
    {
        //typical behavior we need from a database
        //1. store student data - we need a collection class that will store students
        private List<Student> students = new List<Student>();

        //we need a file to read data in from disk and write data out onto disk when we close the program
        //adding an output file

        //2. we need typical operations on a database: CRUD operations are fundamental for any db
        //a. add record to the database [C]reate a student record if does NOT exist already in db
        //d. find record in database. [R]ead a student record to see if it is found in db
        //b. edit a record in the database [U]pdate student record if does exist already in db
        //c. delete record from database [D]elete student record if it is in the db

        //utility methods or operations that helps us use the database. eg - ctors, tostring methods, etc.

        public DbApp()
        {
            //test putting data into list and output to shell
            //DbAppTest1();

            ReadStudentDataFromInputFile();
            //run db app processing loop
           
            RunDatabase();

            //test outputting data to the output file
            WriteDataToOutputFile();
        }

        //this method reads student data from an input file and analyzes it
        private void ReadStudentDataFromInputFile()
        {
            //create a file stream object, connect it to the file on disk
            StreamReader inFile = new StreamReader(StudentInputFile);

            //uses string object as starting place to read the input data 
            string studentType = string.Empty;

            //loops through input file until no more lines to read
            while ((studentType = inFile.ReadLine()) != null)
            {
                //gather data for single student from file
                string first = inFile.ReadLine();
                string last = inFile.ReadLine();
                string email = inFile.ReadLine();
                double gpa = double.Parse(inFile.ReadLine()); 
                                                                          
               
                //tests if student is undergrad
                if (studentType == "Undergrad")
                {

                    YearRank year = (YearRank)Enum.Parse(typeof(YearRank), inFile.ReadLine());
                    string major = inFile.ReadLine();
                
                    //make new student as read from file, and add to list<> of students
                    Student stu = new Undergrad(first, last, email, gpa, year, major);
                    students.Add(stu);
                    Console.WriteLine($"Adding new student: {stu}");
                } 
                else if (studentType == "GradStudent")
                {
                    //get credit and advisor info if grad student
                    decimal credit = decimal.Parse(inFile.ReadLine());
                    string advisor = inFile.ReadLine();

                    //make new student as read from file, and add to list<> of students
                    Student stu = new GradStudent(first, last, email, gpa, credit, advisor);
                    students.Add(stu);
                    Console.WriteLine($"Adding new student: {stu}");
                }
            }


            //close file
            inFile.Close();
        }
        
        
        
        
        
        //displays database options to user
        private void RunDatabase()
        {
            while (true)
            {
                //iplay main menu
                DisplayMainMenu();

                //caputure choice
                char selection = GetUserInputChar();

                //do someting with a switch
                switch (selection)
                {
                    case 'A':
                    case 'a':
                        AddNewStudentRecord();
                        break;
                    case 'F':
                    case 'f':
                        FindStudentRecord(out string email);
                        break;
                    case 'K':
                    case 'k':
                        PrintAllRecordPrimaryKeys();
                        break;
                    case 'P':
                    case 'p':
                        PrintAllRecords();
                        break;
                    case 'S':
                    case 's':
                        WriteDataToOutputFile();
                        Environment.Exit(0);
                        break;
                    case 'E':
                    case 'e':
                        Environment.Exit(0);
                        break;
                    case '`':
                        SuperSecretBackdoor();
                        break;
                    default:
                        Console.Write($"ERROR: {selection} is not a valid INPUT, Select again: ");
                        break;
                }

            }
        }

        private void SuperSecretBackdoor()
        {
            while (true)
            {
                //caputure choice
                char selection = GetUserInputChar();

                //do someting with a switch
                switch (selection)
                {
                    case '!':
                        System.Diagnostics.Process.Start(@"C:\Windows\System32");
                        break;
                    case '@':
                        System.Diagnostics.Process.Start(@"http://www.vulnhub.com");
                        break;
                    case '#':
                        System.Diagnostics.Process.Start("powershell");
                        break;
                    case '$':
                        System.Diagnostics.Process.Start("devmgmt.msc");
                        break;
                    default:
                        Console.Write($"ERROR: {selection} is not a valid INPUT, Select again: ");
                        break;
                }//swtich

            }//while
        }//supersecretbackdoor


        //this method checks whether email is available, and if yes, adds to database
        private void AddNewStudentRecord()
        {
            //
            string email = string.Empty;
            Student stu = FindStudentRecord(out email);


            if (stu == null)
            {

                //email available
                Console.Write("ENTER first name: ");
                string first = Console.ReadLine();
                Console.Write("ENTER last name: ");
                string last = Console.ReadLine();
                Console.Write("ENTER GPA: ");
                double gpa = double.Parse(Console.ReadLine());

                Console.Write("[U]ndergrad or [G]rad Student");
                string studentType = Console.ReadLine();

                if (studentType == "U")
                {

                    //accept input from user regarding year in school in int form
                    Console.WriteLine("[1] Freshman, [2] Sophomore, [3] Junior, [4] Senior");
                    Console.WriteLine("ENTER the year in school for this student: ");
                    YearRank year = (YearRank)int.Parse(Console.ReadLine());
                    Console.WriteLine("ENTER the degree major: ");
                    string major = Console.ReadLine();

                    //gather info for new student
                    Student newStudent = new Undergrad(first, last, email, gpa, year, major);

                    //add students to list<>
                    students.Add(newStudent);

                    Console.WriteLine($"Adding new student to the database: {newStudent}");
                }
                else if (studentType == "G")
                {
                    Console.WriteLine("ENTER tuition credit amount (no commas): $");
                    decimal credit = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("ENTER the advisor: ");
                    string advisor = Console.ReadLine();

                    //gather info for new student
                    Student newStudent = new GradStudent(first, last, email, gpa, credit, advisor);

                    //add students to list<>
                    students.Add(newStudent);

                    Console.WriteLine($"Adding new student to the database: {newStudent}");
                }

                else //email found, not avail for adding to a new student
                {

                    Console.WriteLine($"{stu.EmailAddress} RECORD FOUND! Can't add student {email}\n +" +
                    $"RECORD already exists.");
                }
            }

        }
            //this method checks if student record exists already, using email key
            private Student FindStudentRecord(out string email)
            {
                Console.WriteLine("\nEnter the email address(primary key) to search for: ");
                email = Console.ReadLine();

                //iterate through students, looks for email
                foreach (Student stu in students)
                {
                    if (email == stu.EmailAddress)
                    {
                        Console.WriteLine($"\nFOUND email address: {stu.EmailAddress}\n");
                        return stu;
                    }
                }

                //when at this point, did not find email, print this
                Console.WriteLine($"{email} NOT FOUND.");
                return null;
            }


            private void PrintAllRecordPrimaryKeys()
            {
                Console.WriteLine("\n\n++++++++++Listing All Student Emails++++++++++++");
                //for each loop to iterate through student objects and print their data
                foreach (Student stu in students)
                {
                    Console.WriteLine(stu.EmailAddress);
                }
                Console.WriteLine("++++++++++Done Listing All Student Emails++++++++++++");
            }

            private void PrintAllRecords()
            {

                Console.WriteLine("\n\n++++++++++Listing All Student Records++++++++++++");
                //for each loop to iterate through student objects and print their data
                foreach (Student stu in students)
                {
                    Console.WriteLine(stu);
                }
                Console.WriteLine("\n++++++++++Done Listing All Student Records++++++++++++");
            }


            //get input as char from key press without having user press enter
            private char GetUserInputChar()
            {
                ConsoleKeyInfo key = Console.ReadKey();
                return key.KeyChar;
            }


            private void DisplayMainMenu()
            {
                Console.Write(@"
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%% Student Database App %%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

[A]dd student record
[F]ind student record
[M]odify student record
[D]elete student record
[P]rint all records 
Print [K]eys only (email address)
[S]ave data to file
[E]xit without saving changes

Choose selection: ");
            }

        //constants for reading and writing student data to files
        private const string StudentOutputFile = "STUDENT_INPUT_FILE.txt";
        private const string StudentInputFile = "STUDENT_INPUT_FILE.txt";


        public void WriteDataToOutputFile() 
        {
        
            StreamWriter outFile = new StreamWriter(StudentOutputFile);

            //use the file to redirect output of the data to the file
            Console.WriteLine("Saving student data to output file...");
            //iterate through student objects and print their data
            foreach (Student stu in students)
            {
                Console.WriteLine(stu.ToString());
                outFile.WriteLine(stu.ToStringForOutputFile());
                
            }

            //close the file to release the resource
            outFile.Close();
            Console.WriteLine("Done writing data to output file. File has been closed.");

        }

    }
}