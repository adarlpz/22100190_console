using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
namespace Consolacooperativa {
    internal class Program {
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
        static string separar(string frase, int nP) { //funcion que va a retornar lo que quiere buscar
            string[] palabras = frase.Split(" "); //
            if (nP >= palabras.Length) {
                return null;
            }
            else {
                return palabras[nP];
            }
        }
        static void dir(string ruta, string arg) {
            if (arg != null) {
                ruta = ruta + "\\" + arg; //agrega a la ruta el argumento
            }
            if (Directory.Exists(ruta)) { // si halla una ruta
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

        static string cd(string ruta, string arg) {
            if (arg != null) {
                string nuevaRuta = Path.Combine(ruta, arg); // Combina la ruta actual con el subdirectorio

                if (Directory.Exists(nuevaRuta)) {
                    return nuevaRuta;  // Si la nueva ruta existe, la retorna
                }
                else {
                    Console.WriteLine("La ruta especificada no existe.");
                    return ruta;  // Si no existe, regresa la ruta original
                }
            }
            return ruta;  // Si no hay argumento, no cambia la ruta
        }
        static void help(string arg) {
            if (arg != null) {
                if (comandos.ContainsKey(arg)) {
                    Console.WriteLine($"{comandos[arg]}");
                }
                else {
                    Console.WriteLine("No se encontro el comando");
                }
            }
            else {
                foreach (var cmd in comandos) {
                    Console.WriteLine($"{cmd.Key} - {cmd.Value}");
                }
            }
        }
        static void mkdir(string ruta, string arg) {
            if (arg != null) {
                string nuevaRuta = Path.Combine(ruta, arg); // Combina la ruta actual con el subdirectorio
                Directory.CreateDirectory(nuevaRuta);
                }
        
        }
        static void del(string ruta, string arg) {
            if (arg != null) {
                string nuevaRuta = Path.Combine(ruta, arg); // Combina la ruta actual con el subdirectorio
                Directory.Delete(nuevaRuta);
            }
        }
        static void move(string ruta, string arg, string destination) {
            string origen = Path.Combine(ruta, arg);
            string destino = Path.Combine(ruta, destination);
            if (Directory.Exists(origen)) {
                if (Directory.Exists(destino)) {
                    destino = Path.Combine(destino, Path.GetFileName(origen));
                }

                Directory.Move(origen, destino);
            }
        }
        static void rmdir(string ruta, string arg) {
            if (arg != null) {
                string nuevaRuta = Path.Combine(ruta, arg); // Combina la ruta actual con el subdirectorio
                if (Directory.GetFiles(nuevaRuta).Length == 0 && Directory.GetDirectories(nuevaRuta).Length == 0) {
                    Directory.Delete(nuevaRuta);
                }
            }
        }
        static void date(string arg) {
            if (arg == null) {
                Console.WriteLine($"Fecha actual: {DateTime.Now.ToString("dd/MM/yyyy")}");
            }
        }
        static void time(string arg) {
            if (arg != null) {
            Console.WriteLine($"Hora actual: {DateTime.Now.ToString("HH:mm:ss")}");}
        }
        static void rename(string ruta, string oldName, string newName) {
            string oldPath = Path.Combine(ruta, oldName);
            string newPath = Path.Combine(ruta, newName);

            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
                Console.WriteLine($"Directorio renombrado de {oldName} a {newName}");
            }
        }
        static void copy(string ruta, string sourceFile, string destinationPath) {
            string sourceFilePath = Path.Combine(ruta, sourceFile);
            string destinationFullPath = Path.Combine(ruta, destinationPath);

            // Si destinationPath es un directorio, copiar dentro del directorio con el mismo nombre
            if (Directory.Exists(destinationFullPath))
            {
                destinationFullPath = Path.Combine(destinationFullPath, Path.GetFileName(sourceFile));
            }

            try
            {
                File.Copy(sourceFilePath, destinationFullPath, true);
                Console.WriteLine($"Archivo copiado de {sourceFile} a {destinationFullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al copiar el archivo: {ex.Message}");
            }
        }
        static void color(string arg) {
            if (string.IsNullOrEmpty(arg) || arg.Length != 2) {
                Console.WriteLine("Uso: color <FB> (F = fondo, B = texto)");
                return;
            }

            Dictionary<char, ConsoleColor> colores = new Dictionary<char, ConsoleColor> {
        { '0', ConsoleColor.Black }, { '1', ConsoleColor.DarkBlue }, { '2', ConsoleColor.DarkGreen }, { '3', ConsoleColor.DarkCyan },
        { '4', ConsoleColor.DarkRed }, { '5', ConsoleColor.DarkMagenta }, { '6', ConsoleColor.DarkYellow }, { '7', ConsoleColor.Gray },
        { '8', ConsoleColor.DarkGray }, { '9', ConsoleColor.Blue }, { 'A', ConsoleColor.Green }, { 'B', ConsoleColor.Cyan },
        { 'C', ConsoleColor.Red }, { 'D', ConsoleColor.Magenta }, { 'E', ConsoleColor.Yellow }, { 'F', ConsoleColor.White }
    };

            char fondo = char.ToUpper(arg[0]);
            char texto = char.ToUpper(arg[1]);

            if (!colores.ContainsKey(fondo) || !colores.ContainsKey(texto))
            {
                Console.WriteLine("Colores no válidos. Usa valores hexadecimales (0-F).");
                return;
            }

            Console.BackgroundColor = colores[fondo];
            Console.ForegroundColor = colores[texto];
            Console.Clear();  // Limpiar pantalla para aplicar el fondo
        }
        static void type(string ruta, string nombreArchivo) {
            if (string.IsNullOrEmpty(nombreArchivo))
            {
                Console.WriteLine("Uso: type <nombreArchivo>");
                return;
            }

            string filePath = Path.Combine(ruta, nombreArchivo);

            if (File.Exists(filePath))
            {
                try
                {
                    string contenido = File.ReadAllText(filePath);
                    Console.WriteLine(contenido);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al leer el archivo: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo especificado no existe.");
            }
        }
        static void shutdown(string arg) {
            Process.Start("shutdown", "/s /t 0");
        }
        static void more(string ruta, string nombreArchivo)
        {
            if (string.IsNullOrEmpty(nombreArchivo))
            {
                Console.WriteLine("Uso: type <nombreArchivo>");
                return;
            }

            string filePath = Path.Combine(ruta, nombreArchivo);

            if (File.Exists(filePath))
            {
                try
                {
                    string contenido = File.ReadAllText(filePath);
                    Console.WriteLine(contenido);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al leer el archivo: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo especificado no existe.");
            }
        }
        static void find(string ruta, string archivo, string texto)
        {
            string archivoPath = Path.Combine(ruta, archivo);

            if (!File.Exists(archivoPath))
            {
                Console.WriteLine("El archivo especificado no existe.");
                return;
            }

            bool encontrado = false;
            int lineaNum = 0;

            try
            {
                foreach (string linea in File.ReadLines(archivoPath))
                {
                    lineaNum++;
                    if (linea.Contains(texto, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Línea {lineaNum}: {linea}");
                        encontrado = true;
                    }
                }

                if (!encontrado)
                {
                    Console.WriteLine("No se encontró el texto en el archivo.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer el archivo: {ex.Message}");
            }
        }

        static void Main(string[] args) {
            string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            while (true) {
                Console.Write(ruta + ">"); //ruta donde inicia
                string input = Console.ReadLine(); //input
                string comando = separar(input, 0); //te lee la primera palabra que es el comando
                string argumento = separar(input, 1); //te lee la segunda palabra que es el argumento

                string destino = separar(input, 2);

                if (comando == "exit") {
                    break;
                }
                switch (comando) {
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
                    case "mkdir":
                        mkdir(ruta, argumento);
                        break;
                    case "del":
                        del(ruta, argumento);
                        break;
                    case "move":
                        move(ruta, argumento, destino);
                        break;
                    case "rmdir":
                        rmdir(ruta, argumento);
                        break;
                    case "date":
                        date(argumento);
                        break;
                    case "time":
                        time(argumento);
                        break;
                    case "ver":
                        Console.WriteLine($"Versión del sistema operativo: {Environment.OSVersion.Version}");
                        break;
                    case "rename":
                        rename(ruta, argumento, destino);
                        break;
                    case "copy":
                        copy(ruta, argumento, destino);
                        break;
                    case "color":
                        color(argumento);
                        break;
                    case "type":
                        type(ruta, argumento);
                        break;
                    case "shutdown":
                        shutdown(argumento);
                        break;
                    case "more":
                        more(ruta, argumento);
                        break;
                    case "find":
                        find(ruta, argumento, destino);
                        break;
                }
            }
        }
    }
}