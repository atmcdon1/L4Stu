
// 2/1/2024 started db database
// 2/6/2024 class update?


using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Instrumentation;
using System.Security;

//Use Case 0




namespace StudentDbApp
{
    // This will represent the application itself
    // known in OO "pattern" as a singleton object pattern.

    internal class DdApp
    {

        //STATE OF THE STUDENTS
        //what is the typical BEHAVIOR that we need from a database.
        // 1 - store data (students) - we need some kind of collection class that will store students
                                                   //PARA
        private List<Student> students = new List<Student>();

        //OPERATIONS OF STUDENTS
        // 2- we need typical operations on a database? CRUD operations are fudamenta to any DB
        // a) add a student record to a database [C]reate a student record - if it isnt already in the db
        // b) Find a student record in a database [R]ead a student record - print if the rec IS in the database
        // c) edit a student record in the database [U]pdate a student record  - if it IS in the db
        // d) delete a student record to a the database [D]elete a student record - if it IS in the db
        // 
        //Utility type methods or operations - eg - ctor, tostring methods, etc.
        //
        // ctor basic contstuctor
        public DdApp()
        {


            ReadStudentDataFromInputFile();
            //run the main database app "pocesing "loop""
            RunDatabaseApp();



            //test putting data in to the list - and put to the shell
            //DBAppTest1();

            //test outputing to the output file
            WriteDataToOutputFile();
        }

        private void ReadStudentDataFromInputFile()
        {
            // 1 - Create a file stream object and connect it to the file on disk
                                                   //Stream Consant 
            StreamReader inFile = new StreamReader(StudentInputeFile);

            // 2 - Use the file object to actually read the input data
            string studentType = string.Empty;

            // dual purpose statement here
            // 1 - read in a string form the file
            // 2 - set the condition for the loop to continue by comparing to null
            // looks for end of file null

            //looking for TYPE of student
            while((studentType = inFile.ReadLine()) != null) 
            {
                //gather all the data for a single student from the file
                
                string first = inFile.ReadLine();
                string last = inFile.ReadLine();
                string email = inFile.ReadLine();
                double gpa = double.Parse(inFile.ReadLine());

                if (studentType == "StudentDbApp.Undergrad")
                {
                    YearRank year = (YearRank)Enum.Parse(typeof(YearRank), inFile.ReadLine());
                    string major = inFile.ReadLine();

                    //now make a new student and add them to the list<>
                    Student stu = new Undergrad(first, last, email, gpa, year, major);
                    students.Add(stu);
                    Console.Write($"Added new student: {stu}");
                }
                else if (studentType == "StudentDbApp.GradStudent")
                {
                    //go grab the stuff that is specific to grads only
                    decimal credit = decimal.Parse(inFile.ReadLine());
                    string advisor = inFile.ReadLine();

                    //now make a new student and add them to the list<>
                    //A form of Polymorphism 
                    Student stu = new GradStudent(first, last, email, gpa, credit, advisor);
                    students.Add(stu);
                    Console.Write($"Added new student: {stu}");
                }
                else
                {
                    Console.WriteLine($"{studentType} is not a valid Student Type. Check the student_input_file.");
                }


                

                //handle to a student
                //how make a new student and add them to a list<>
/*                Student stu = new Student(first, last, email, gpa, year);
                students.Add(stu);*/
            }

            
            // 3 - Release the resource by closing the file
            inFile.Close();
        }

        private void RunDatabaseApp()
        {
            // good for incase we forget to code it
            //throw new NotImplementedException();
            
            while (true) 
            {
                // display the main menu
                DisplayMainMenu();

                //capture the users choice form the menu selection
                //char selection = Console.ReadKey().KeyChar;
                char selection = GetUserInputChar();



                // This is a switch menu
                switch (selection)
                {
                    case 'E':
                    case 'e':
                        Environment.Exit(0);
                        break;
                    case 'A':
                    case 'a':
                        AddNewStudentRecord();
                        break;
                    case 'M':
                    case 'm':
                        //TODO: FIND students confirm this is who you want to modify.ASK which part to modify find that part then modify.
                        break;
                    case 'D':
                    case 'd':  
                        //TODO: FIND students confirm this is who you want to delete THEN delete them. 
                        break;
                    case 'P':
                    case 'p':
                        //TODO: print the contents of the ALL database or Search for user and print
                        break;
                    case 'S':
                    case 's':
                        Console.WriteLine("Save and exit");
                        WriteDataToOutputFile();
                        Environment.Exit(0);
                        break;

                        //''literal
                    case 'F':
                    case 'f':
                        Console.WriteLine("\nThis is F for find a record");
                        //TODO: look for a person in the database
                        break;
                    default:
                        Console.WriteLine($"{selection} is not a valid INPUT, SELECT AGAIN: ");
                        break;
                }

            }

        }

        //add a new student record - 
        //precondition: before this is called, we need to determin if the primary key
        //(the disired email address) is already in the database
        private void AddNewStudentRecord()
        {
            // We need to determine some things 
            // 1 - what email does the new student "desire"
            // 2 - is this email available

            string email = string.Empty;
            //looks for email in student
            Student stu = FindStudentRecord(out email);

            if(stu == null)
            {
                //Sunny day scenario for add student - the email is AVAilable
                Console.WriteLine("Enter first name: ");
                string first = Console.ReadLine();
                Console.WriteLine("Enter last name: ");
                string last = Console.ReadLine();
                Console.WriteLine("Enter GPA: ");
                double GPA = double.Parse(Console.ReadLine());

                Console.WriteLine("[U]ndergrad, [G]rad Student");
                Console.WriteLine("Enter the year in school for this student:");
                string studentType = Console.ReadLine();

                if (studentType == "U")
                {



                    //help the user with the enum type
                    Console.WriteLine("[1]Freshman, [2]Sophomore, [3]Junior, [4]Senior ");
                    Console.WriteLine("Enter the year in school for this student:");
                    YearRank year = (YearRank)int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the degree major: ");
                    string major = Console.ReadLine();

                    //have all the info a new student
                    Student newStu = new Undergrad(first, last, email, GPA, year, major);

                    //add them to the list<>
                    students.Add(newStu);

                    Console.WriteLine($"Added new Student to the database: \n{newStu}");
                }
                else if (studentType == "G")
                {



                    //help the user with the enum type
                    Console.WriteLine("Enter the Tuition credit amount (no commas) $");
                    decimal credit = decimal.Parse(Console.ReadLine());

                    Console.WriteLine("Enter Advisor");
       
                    string advisor = Console.ReadLine();

                    //have all the info a new student
                    Student newStu = new GradStudent(first,last,email,GPA,credit,advisor);

                    //add them to the list<>
                    students.Add(newStu);

                    Console.WriteLine($"Added new Student to the database: \n{newStu}");
                }

            }
            else 
            {
                //Email is no avaiable for adding a student 
                Console.WriteLine($"{stu.EmailAddress} Record Found! Can't add student {email}\n" + 
                    $"Record already exists.");
            }
        }

        //out sting
        // This method Finds a student record BY email
        //passes email in out if found.
        //UPDATE MORE
        private Student FindStudentRecord(out string email)
        {

            Console.WriteLine("\nEnter the Email address (primary key ) to search for: ");
            email = Console.ReadLine();

            foreach(Student stu in students) 
            {
                if(email == stu.EmailAddress)
                {
                    Console.WriteLine($"Found email address: {stu.EmailAddress}\n");
                    return stu;
                }
            }
            Console.WriteLine($"{email}: not found.");
            return null;

        }

        private char GetUserInputChar()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            return key.KeyChar;
        }

        private void DisplayMainMenu()
        {
            //throw new NotImplementedException();
            Console.Write(@"
********************** StudentDataBase App ***********************
------------------------------------------------------------------
[A]dd a student record     (C in CRUD - Create)
[F]ind a student record    (R in CRUD - Read)
[M]odify student record    (U in CRUD - Update)
[D]elete Student record    (D in CRUD - Delete)
[P]rint all records in current DB storage
[S]ave and exit            
[E]xit without saving changes

User key Selection: ");

        }

        //without a path C# will write this file to teh current directory.
        private const string StudentOutputFile = "STUDENT_OUTPUT_FILE.txt";
        private const string StudentInputeFile = "STUDENT_INPUT_FILE.txt";
        public void WriteDataToOutputFile()
        {
            //create the output file details
            //Console
            StreamWriter outFile = new StreamWriter(StudentOutputFile);


            //use the file for directing the output of the data to the file
            foreach (Student stu in students)
            {
                // dont need to echo to the console but a nice way to monitor the input.
                Console.WriteLine(stu);
                //prints to output file
                outFile.WriteLine(stu.ToStringForOutputFile());
                Console.WriteLine("Done writing to the file - file has been closed");
            }


            //close the file to release the resource
            //if you end up with a empty output file, check to make
            //sure that you close it.
            outFile.Close();
        }


        //not static to be used by other classs namespaces
        

    }
}