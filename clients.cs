using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Identity
{
    public class Client
    {
        public static List<Client> clients { get; set; } = new List<Client>();

        public string ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{Name} {LastName}";
        public double Ahorros { get; set; }
        public string Password { get; set; }
        private int DataBits = 0;
        public int Age => (DataBits >> 4);
        public Gender Gender => (Gender)(DataBits & 0b1000);
        public MaritalStatus Status => (MaritalStatus)(DataBits & 0b0100);
        public Grade AcademicGrade => (Grade)(DataBits & 0b11);

        public Client(in string id, in int age = 0, in string name = null, in string lastname = null, in double ahorros = 0, in string password = null, in Gender gender = Gender.Male, in MaritalStatus ms = MaritalStatus.Single, in Grade ag = Grade.Elementary)
        {
            ID = id;
            Name = name;
            LastName = lastname;
            Ahorros = ahorros;
            Password = password;
            DataBits = (age << 4) | (int)gender | (int)ms | (int)ag;

        }
        public override string ToString()
        => $"{GetType().Name} ({nameof(ID)}: {ID}; {nameof(Name)}: {Name}; {nameof(LastName)}: {LastName}; {nameof(Ahorros)}: {Ahorros}; {nameof(Password)}: {Password}; {nameof(Age)}: {Age}; {nameof(Gender)}: {Gender}; {nameof(Status)}: {Status}; {nameof(AcademicGrade)}: {AcademicGrade};";
        public override bool Equals(object obj)
        {
            if (obj is Client other)
                return ID.Equals(other.ID);
            else
                return false;
        }
        public override int GetHashCode()
        {
           int hash = 0;
           hash = ID.Substring(0, 3).GetHashCode();
           
           return Math.Abs(hash);
        }
        public int CompareTo(object obj) {
            if (this.GetHashCode() < obj.GetHashCode())
                return -1;
            else if (this.GetHashCode() > obj.GetHashCode())
                return 1;
            else 
                return 0;
        }


        // desde archivo CSV
        internal static Client ConvertCSVToClient(string line)
        {
            string[] tokens = line.Split(',');
            (
                string id,
                string name,
                string lastName,
                double ahorros,
                string password,
                int DataBits
            )
            =
            (
                tokens[0],
                tokens[1],
                tokens[2],
                double.Parse(tokens[3]),
                tokens[4],
                int.Parse(tokens[5])
            );
            int age = (DataBits >> 4);
            Gender gender = (Gender)(DataBits & 0b1000);
            MaritalStatus ms = (MaritalStatus)(DataBits & 0b0100);
            Grade ag = (Grade)(DataBits & 0b11);
            return new Client(id, age, name, lastName, ahorros, password, gender, ms, ag);
        }
        //Desde el menú de captura

        internal static Client ConvertToClient(string registro)
        {
            string[] tokens = registro.Split(',');
            (
                string id,
                string name,
                string lastName,
                double ahorros,
                string password,
                int age,
                Gender gender,
                MaritalStatus maritalstatus,
                Grade academicgrade
            )
            =
            (
                tokens[0],
                tokens[1],
                tokens[2],
                double.Parse(tokens[3]),
                tokens[4],
                int.Parse(tokens[5]),
                (Gender)int.Parse(tokens[6]),
                (MaritalStatus)int.Parse(tokens[7]),
                (Grade)int.Parse(tokens[8])
            );

            return new Client(id, age, name, lastName, ahorros, password, gender, maritalstatus, academicgrade);
        }

        internal static Client GetClient(string ID)
            => clients?.Where(p => p.ID == ID).SingleOrDefault();

        internal void Insert()
        {
            if (clients.Contains(this))
            {
                Console.WriteLine("No se pudo agregar el cliente.");
            }
            else
            {
                clients.Add(this);
            }
        }

        public static void Save(string files)
        {
            using (StreamWriter sw = new StreamWriter(files))
            {
                sw.WriteLine("ID, Name, Last Name, Ahorros, Contraseña, Datos");
                foreach (Client ct in clients)
                {
                    string[] values = { ct.ID, ct.Name, ct.LastName, ct.Ahorros.ToString(), ct.Password, ct.DataBits.ToString() };
                    string line = string.Join(",", values);
                    sw.WriteLine(line);
                }
                sw.Flush();
            }
        }
       

    }
    public enum Gender
    {
        Male = 0,
        Female = 8
    }
    public enum MaritalStatus
    {
        Single = 0,
        Married = 4
    }
    public enum Grade
    {
        Elementary = 0,
        Highschool = 1,
        Graduate = 2,
        Degree = 3
    }
}