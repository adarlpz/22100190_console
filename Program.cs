using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

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
    static Dictionary<char, ConsoleColor> colores = new Dictionary<char, ConsoleColor> {
        { '0', ConsoleColor.Black }, { '1', ConsoleColor.DarkBlue }, { '2', ConsoleColor.DarkGreen }, { '3', ConsoleColor.DarkCyan },
        { '4', ConsoleColor.DarkRed }, { '5', ConsoleColor.DarkMagenta }, { '6', ConsoleColor.DarkYellow }, { '7', ConsoleColor.Gray },
        { '8', ConsoleColor.DarkGray }, { '9', ConsoleColor.Blue }, { 'A', ConsoleColor.Green }, { 'B', ConsoleColor.Cyan },
        { 'C', ConsoleColor.Red }, { 'D', ConsoleColor.Magenta }, { 'E', ConsoleColor.Yellow }, { 'F', ConsoleColor.White }
    };
    static string separar(string frase, int nP) { //funcion que separa entre comandos, el primer argumento y el segundo argumento
        string[] palabras = frase.Split(" ");
        if (nP >= palabras.Length) {
            return null;
        } else {
            return palabras[nP];
        }
    }
    static void dir(string ruta, string arg) {
        if (arg != null) {
            ruta = ruta + "\\" + arg; //agrega a la ruta el argumento
        }
        if (Directory.Exists(ruta)) { //si halla una ruta
            string[] directorios = Directory.GetDirectories(ruta); //consigue todos los directorios que existen
            foreach (string dir in directorios) { //muestra cada diretorio del arreglo de directorios
                Console.WriteLine(dir);
            }
            string[] archivos = Directory.GetFiles(ruta);
            foreach (string archivo in archivos) {
                Console.WriteLine(Path.GetFileName(archivo));
            }
        } else {
        Console.WriteLine("La ruta especificada no existe.");
        }
    }
    static string cd(string ruta, string arg){
            if (arg == "..") {
                return Path.GetFullPath(Path.Combine(ruta, ".."));
            }
        if (arg != null) {
            string nuevaRuta = Path.Combine(ruta, arg);
            if (Directory.Exists(nuevaRuta)) {
                return nuevaRuta;  //si la nueva ruta existe, la retorna
            } else {
                Console.WriteLine("La ruta especificada no existe.");
                return ruta;  //si no existe, regresa la ruta original
            }
        }
        return ruta;
    }
    static void help(string arg) {
        if (arg != null) {
            if (comandos.ContainsKey(arg)) {
                Console.WriteLine($"{comandos[arg]}");
            } else {
                Console.WriteLine("No se encontro el comando");
            }
        } else {
            foreach (var cmd in comandos) {
                Console.WriteLine($"{cmd.Key} - {cmd.Value}");
            }
        }
    }
    static void mkdir(string ruta, string arg) {
        if (arg != null) {
            string nuevaRuta = Path.Combine(ruta, arg);
            Directory.CreateDirectory(nuevaRuta);
        }
    }
    static void del(string ruta, string arg) {
        if (arg != null) {
            string nuevaRuta = Path.Combine(ruta, arg);
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
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy"));
        }
    }
    static void time(string arg) {
        if (arg == null) {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
        }
    }
    static void rename(string ruta, string oldName, string newName) {
        string rutaOld = Path.Combine(ruta, oldName);
        string rutaNueva = Path.Combine(ruta, newName);
        if (Directory.Exists(rutaOld)) {
            Directory.Move(rutaOld, rutaNueva);
        }
    }
    static void copy(string ruta, string sourceFile, string destinationPath) {
        string sourceFilePath = Path.Combine(ruta, sourceFile);
        string destinationFullPath = Path.Combine(ruta, destinationPath);

        if (Directory.Exists(destinationFullPath)) {//si destinationPath es un directorio, copiar dentro del directorio con el mismo nombre
            destinationFullPath = Path.Combine(destinationFullPath, Path.GetFileName(sourceFile));
        }
        if (!File.Exists(sourceFilePath)) {
            File.Create(destinationFullPath).Close(); // Crear archivo vacío
            return;
        }
        File.Copy(sourceFilePath, destinationFullPath, true);
        Console.WriteLine($"Archivo copiado de {sourceFile} a {destinationFullPath}");
        }
    static void color(string arg) {
        char fondo = char.ToUpper(arg[0]);
        char texto = char.ToUpper(arg[1]);
        Console.BackgroundColor = colores[fondo];
        Console.ForegroundColor = colores[texto];
        Console.Clear(); //limpiar pantalla para aplicar el fondo
    }
    static void type(string ruta, string nombreArchivo) {
        if (string.IsNullOrEmpty(nombreArchivo)) {
            Console.WriteLine("Uso: type <nombreArchivo>");
            return;
        }
        string filePath = Path.Combine(ruta, nombreArchivo);
        if (File.Exists(filePath)) {
            string contenido = File.ReadAllText(filePath);
            Console.WriteLine(contenido);
        } else {
            Console.WriteLine("El archivo especificado no existe.");
        }
    }
    static void shutdown(string arg) {
        Process.Start("shutdown", "/s /t 0");
    }
    static void more(string ruta, string nombreArchivo) {
        string filePath = Path.Combine(ruta, nombreArchivo);
        if (File.Exists(filePath)) {
            string contenido = File.ReadAllText(filePath);
            Console.WriteLine(contenido);
        } else {
            Console.WriteLine("El archivo especificado no existe.");
        }
    }
    static void find(string ruta, string archivo, string texto) {
        string archivoPath = Path.Combine(ruta, archivo);
        bool encontrado = false;
        int lineaNum = 0;
        foreach (string linea in File.ReadLines(archivoPath)) {
            lineaNum++;
            if (linea.Contains(texto, StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine($"Línea {lineaNum}: {linea}");
                encontrado = true;
            }
        }
        if (!encontrado) {
            Console.WriteLine("No se encontró el texto en el archivo.");
        }
    }
    static void Main(string[] args) {
        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        while (true) {
            Console.Write(ruta + ">"); //ruta donde inicia
            string input = Console.ReadLine(); //input
            input = input.ToLower();
            string comando = separar(input, 0); //te lee la primera palabra que es el comando
            string argumento1 = separar(input, 1); //te lee la segunda palabra que es el argumento1
            string argumento2 = separar(input, 2); //te lee la tercera palabra que es el arguemento2
            switch (comando) {
                case "exit":
                    break;
                case "dir":
                    dir(ruta, argumento1);
                    break;
                case "cd":
                    ruta = cd(ruta, argumento1);
                    break;
                case "help":
                    help(argumento1);
                    break;
                case "cls":
                    Console.Clear();
                    break;
                case "mkdir":
                    mkdir(ruta, argumento1);
                    break;
                case "del":
                    del(ruta, argumento1);
                    break;
                case "move":
                    move(ruta, argumento1, argumento2);
                    break;
                case "rmdir":
                    rmdir(ruta, argumento1);
                    break;
                case "date":
                    date(argumento1);
                    break;
                case "time":
                    time(argumento1);
                    break;
                case "ver":
                    Console.WriteLine(Environment.OSVersion.Version);
                    break;
                case "rename":
                    rename(ruta, argumento1, argumento2);
                    break;
                case "copy":
                    copy(ruta, argumento1, argumento2);
                    break;
                case "color":
                    color(argumento1);
                    break;
                case "type":
                    type(ruta, argumento1);
                    break;
                case "shutdown":
                    shutdown(argumento1);
                    break;
                case "more":
                    more(ruta, argumento1);
                    break;
                case "find":
                    find(ruta, argumento1, argumento2);
                    break;
                }
            }
        }
    }
}