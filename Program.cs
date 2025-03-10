using System.IO;
namespace Consolacooperativa
{
    internal class Program
    {
        static Dictionary<string, string> comandos = new Dictionary<string, string> {
        { "ASSOC", "Displays or modifies file extension associations." },
        { "ATTRIB", "Displays or changes file attributes." },
        { "BREAK", "Sets or clears extended CTRL+C checking." },
        { "BCDEDIT", "Sets properties in boot database to control boot loading." },
        { "CACLS", "Displays or modifies access control lists (ACLs) of files." },
        { "CD", "Displays the name of or changes the current directory." },
        { "CHKDSK", "Checks a disk and displays a status report." },
        { "CLS", "Clears the screen." },
        { "CMD", "Starts a new instance of the Windows command interpreter." },
        { "COPY", "Copies one or more files to another location." },
        { "DEL", "Deletes one or more files." },
        { "DIR", "Displays a list of files and subdirectories in a directory." },
        { "ECHO", "Displays messages, or turns command echoing on or off." },
        { "EXIT", "Quits the CMD.EXE program (command interpreter)." },
        { "FIND", "Searches for a text string in a file or files." },
        { "FORMAT", "Formats a disk for use with Windows." },
        { "HELP", "Provides Help information for Windows commands." },
        { "MKDIR", "Creates a directory." },
        { "MOVE", "Moves one or more files from one directory to another." },
        { "PAUSE", "Suspends processing of a batch file and displays a message." },
        { "RD", "Removes a directory." },
        { "REN", "Renames a file or files." },
        { "RMDIR", "Removes a directory." },
        { "START", "Starts a separate window to run a specified program or command." },
        { "TASKLIST", "Displays all currently running tasks including services." },
        { "TIME", "Displays or sets the system time." },
        { "TREE", "Graphically displays the directory structure of a drive or path." },
        { "XCOPY", "Copies files and directory trees." }
        };
        static string separar(string frase, int nP)
        { //funcion que va a retornar lo que quiere buscar
            string[] palabras = frase.Split(" "); //
            if (nP >= palabras.Length)
            {
                return null;
            }
            else
            {
                return palabras[nP];
            }
        }
        static void dir(string ruta, string arg) //dir
        {
            if (arg != null) //verifica que exista un argumento
            {
                ruta = ruta + "\\" + arg; //agrega a la ruta el argumento
            }
            if (Directory.Exists(ruta)) // si halla una ruta
            {
                string[] directorios = Directory.GetDirectories(ruta); //consigue todos los directorios que existen
                Console.WriteLine("\nDirectorios encontrados:\n");
                foreach (string dir in directorios) //muestra cada diretorio del arreglo de directorios
                {
                    Console.WriteLine(dir);
                }
            }
            else
            {
                Console.WriteLine("La ruta especificada no existe.");
            }
        }

        static string cd(string ruta, string arg)
        {
            if (arg != null)
            {
                string nuevaRuta = Path.Combine(ruta, arg); // Combina la ruta actual con el subdirectorio

                if (Directory.Exists(nuevaRuta))
                {
                    return nuevaRuta;  // Si la nueva ruta existe, la retorna
                }
                else
                {
                    Console.WriteLine("La ruta especificada no existe.");
                    return ruta;  // Si no existe, regresa la ruta original
                }
            }
            return ruta;  // Si no hay argumento, no cambia la ruta
        }

        static void help(string arg)
        {
            if (arg != null)
            {
                if (comandos.ContainsKey(arg))
                {
                    Console.WriteLine($"{comandos[arg]}");
                }
                else
                {
                    Console.WriteLine("No se encontro el comando");
                }
            }
            else
            {
                foreach (var cmd in comandos)
                {
                    Console.WriteLine($"{cmd.Key} - {cmd.Value}");
                }
            }
        }

        static void Main(string[] args)
        {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            while (true)
            {
                Console.Write(ruta + ">"); //ruta donde inicia
                string input = Console.ReadLine(); //input
                string comando = separar(input, 0); //te lee la primera palabra que es el comando
                string argumento = separar(input, 1); //te lee la segunda palabra que es el argumento

                if (comando == "exit")
                {
                    break;
                }
                switch (comando)
                {
                    case "dir":
                        dir(ruta, argumento);
                        break;
                    case "cd":
                        ruta = cd(ruta, argumento);
                        break;
                    case "help":
                        help(argumento);
                        break;
                    case "cls":
                        Console.Clear();
                        break;
                }
            }
        }
    }
}