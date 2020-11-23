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
    public class ProgramV7
    {
        public static string files;
        private static ClientSet ClientSet = new ClientSet(3,3);

        public static string ValidateInt()
        {
            string intValue = "";
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key != ConsoleKey.Backspace)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.D0:
                            Console.Write("0");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D1:
                            Console.Write("1");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D2:
                            Console.Write("2");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D3:
                            Console.Write("3");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D4:
                            Console.Write("4");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D5:
                            Console.Write("5");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D6:
                            Console.Write("6");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D7:
                            Console.Write("7");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D8:
                            Console.Write("8");
                            intValue += key.KeyChar;
                            break;
                        case ConsoleKey.D9:
                            Console.Write("9");
                            intValue += key.KeyChar;
                            break;
                    }
                }
                else
                {
                    if (intValue.Length > 0)
                    {
                        intValue = intValue.Remove(intValue.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                key = Console.ReadKey(true);
            }
            Console.WriteLine();
            return intValue;
        }
        public static string ValidateDouble()
        {
            string doubleValue = "";
            int dotCount = 0;
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key != ConsoleKey.Backspace)
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.NumPad0:
                        case ConsoleKey.D0:
                            Console.Write("0");

                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad1:
                        case ConsoleKey.D1:
                            Console.Write("1");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad2:
                        case ConsoleKey.D2:
                            Console.Write("2");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad3:
                        case ConsoleKey.D3:
                            Console.Write("3");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad4:
                        case ConsoleKey.D4:
                            Console.Write("4");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad5:
                        case ConsoleKey.D5:
                            Console.Write("5");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad6:
                        case ConsoleKey.D6:
                            Console.Write("6");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad7:
                        case ConsoleKey.D7:
                            Console.Write("7");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad8:
                        case ConsoleKey.D8:
                            Console.Write("8");
                            doubleValue += key.KeyChar;
                            break;
                        case ConsoleKey.NumPad9:
                        case ConsoleKey.D9:
                            Console.Write("9");
                            doubleValue += key.KeyChar;
                            break;

                        case ConsoleKey.OemPeriod:
                            if (dotCount == 0)
                            {
                                dotCount++;
                                Console.Write(".");
                                doubleValue += key.KeyChar;

                            }
                            break;

                    }
                }
                else
                {
                    if (doubleValue.Length > 0)
                    {
                        char character = doubleValue.Last();
                        if (character == '.')
                            dotCount = 0;
                        doubleValue = doubleValue.Remove(doubleValue.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                key = Console.ReadKey(true);
            }
            Console.WriteLine();
            return doubleValue;
        }

        public static string MaskPassword()
        {
            string psw = "";
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Enter)
            {
                if (key.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    psw += key.KeyChar;
                }
                else
                {
                    if (psw.Length > 0)
                    {
                        psw = psw.Remove(psw.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                key = Console.ReadKey(true);

            }
            Console.WriteLine();
            return psw;
        }
        public static void ModifyData()
        {
            Console.Write("Digite Identificador: ");
            var id = ValidateInt();
            var oldClient = new Client(id);
            if (ClientSet.Contains(oldClient))
            {
                oldClient = ClientSet.Get(oldClient);
                Console.WriteLine(oldClient.ToString());
                var newClient = Client.ConvertToClient(Campos(false, oldClient.ID));
                ClientSet.Replace(oldClient,newClient);
            }
            else
            {
                Console.WriteLine("No se encontro el registro, Desea continuar? S/N");
                var rs = Console.ReadLine();
                switch (rs.ToUpper())
                {
                    case "S":
                        ModifyData();
                        break;
                    case "N":
                        break;
                }
            }
        }

        public static void DeleteData()
        {
            Console.Write("Digite Identificador: ");
            var id = ValidateInt();
            var client = new Client(id);
            if (ClientSet.Contains(client))
            {
                client = ClientSet.Get(client);
                Console.WriteLine(client.ToString());
                ClientSet.Remove(client);
            }
            else
            {
                Console.WriteLine("No se encontro el registro. Desea continuar? S/N");
                var rs = Console.ReadLine();
                switch (rs.ToUpper())
                {
                    case "S":
                        DeleteData();
                        break;
                    case "N":
                        break;
                }
            }
        }

        public static string Campos(bool isNew = true, string oldId = null)
        {
            Console.WriteLine("Recopilación de Datos");
            var idd = "";
            if (isNew)
            {
                Console.Write("Cedula: ");
                idd = ValidateInt();
            }
            else
                idd = oldId;

            Console.Write("Nombre: ");
            var namee = Console.ReadLine();
            Console.Write("Apellido: ");
            var lastnamee = Console.ReadLine();
            Console.Write("Ahorros: ");
            var ahorross = ValidateDouble();
            passwordInput:
            Console.Write("Contraseña: ");
            var password = MaskPassword();
            Console.Write("Confirmar Contraseña: ");
            var confirmPassword = MaskPassword();
            if ((string.IsNullOrEmpty(password)) || !(password.Equals(confirmPassword)))
            {
                Console.WriteLine("Las contraseñas no coinciden");
                goto passwordInput;
            }
            Console.Write("Edad: ");
            int age = int.Parse(ValidateInt());

            genderInput:
            Console.WriteLine("Sexo: M|F ");
            int gender = 0;
            switch (Console.ReadLine().ToUpper())
            {
                case "F":
                    gender = 8;
                    break;
                case "M":
                    break;
                default:
                    goto genderInput;
            }
            statusInput:
            Console.Write("Estado Civil: S|C ");
            int status = 0;
            switch (Console.ReadLine().ToUpper())
            {
                case "C":
                    status = 4;
                    break;
                case "S":
                    break;
                default:
                    goto statusInput;
            }
            gradeInput:
            Console.WriteLine("Grado Academico");
            Console.WriteLine("(I) Inicial");
            Console.WriteLine("(M) Media");
            Console.WriteLine("(G) Grado");
            Console.WriteLine("(P) PostGrado");
            Console.Write("Opcion: ");
            int grade = 0;
            switch (Console.ReadLine().ToUpper())
            {
                case "I":
                    break;
                case "M":
                    grade = 1;
                    break;
                case "G":
                    grade = 2;
                    break;
                case "P":
                    grade = 3;
                    break;
                default:
                    goto gradeInput;
            }
            return $"{idd},{namee},{lastnamee},{ahorross},{password},{age},{gender},{status},{grade}";
        }

        public static void CSVToList(string files)
        {
            var exists = !(File.Exists(files));
            if (exists)
            {
                File.Create(files).Close();
            }
            else
            {
                var lines = File.ReadLines(files).ToArray();
                for(int i =0;i<lines.Length;i++)
                {
                    if (i > 0)
                    {
                        lines[i] += $",{i}";
                        var clients = Client.ConvertCSVToClient(lines[i]);
                        ClientSet.Add(clients);
                        Console.WriteLine(clients.ToString());
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            string question;
            files = (args[0]);
                CSVToList(files);
            do
            {
                Console.WriteLine("1. Agregar 2. Buscar 3. Listar 4. Editar 5. Eliminar");
                int option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        var form1 = Campos();
                        var client1 = Client.ConvertToClient(form1);
                        client1.Insert();
                       break;

                    case 2:
                        Console.Write("DIgite el identificador: ");
                        var id= ValidateInt();
                        var client = new Client(id);
                        var record = ClientSet.Get(client);
                        if (record == null)
                            Console.WriteLine("No existe el cliente");
                        else 
                            Console.WriteLine(record.ToString());
                        break;
                     
                    case 3:
                       foreach (var obj in ClientSet.ToSortedArray())
                        {
                            Console.WriteLine(obj.ToString());
                        }
                        break;

                    case 4:
                        ModifyData();
                        break;

                    case 5:
                        DeleteData();
                        break;
                   
                }

                Console.WriteLine("Do you want to go back to the menu? Y/N");
                question = Console.ReadLine().ToLower();
                Client.Save(files);
            }
            while (question == "yes");
        }
    }
}