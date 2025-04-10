using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using OOP3;
static class Program
{
    // WinAPI для консоли
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleCP(uint wCodePageID);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetConsoleOutputCP(uint wCodePageID);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetStdHandle(int nStdHandle, IntPtr handle);

    // WinAPI CreateFile для устройства
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile
    );

    // Флаги доступа/разделения
    private const uint GENERIC_READ = 0x80000000;
    private const uint GENERIC_WRITE = 0x40000000;
    private const uint FILE_SHARE_READ = 0x00000001;
    private const uint FILE_SHARE_WRITE = 0x00000002;
    private const uint OPEN_EXISTING = 3;
    private const uint CP_OEM = 866;     // OEM‑866 для кириллицы
    private const int STD_OUTPUT_HANDLE = -11;
    private const int STD_ERROR_HANDLE = -12;

    [STAThread]
    static void Main()
    {
        // 1) Сброс и выделение «чистой» консоли
        FreeConsole();
        AllocConsole();

        // 2) Установка OEM‑866 для кириллицы
        SetConsoleCP(CP_OEM);
        SetConsoleOutputCP(CP_OEM);
        Console.InputEncoding = Encoding.GetEncoding((int)CP_OEM);
        Console.OutputEncoding = Encoding.GetEncoding((int)CP_OEM);

        // 3) Открываем дескриптор консоли
        var consoleHandle = CreateFile(
            "CONOUT$",
            GENERIC_READ | GENERIC_WRITE,
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            IntPtr.Zero,
            OPEN_EXISTING,
            0,
            IntPtr.Zero
        );
        if (consoleHandle.IsInvalid)
            throw new IOException("Не удалось открыть CONOUT$", Marshal.GetLastWin32Error());

        // ───────────────────────────────────────────────────────────────────
        // Важное дополнение: переустанавливаем стандартные хэндлы ОС
        // чтобы внутренние методы Console (включая Clear) работали правильно
        SetStdHandle(STD_OUTPUT_HANDLE, consoleHandle.DangerousGetHandle());
        SetStdHandle(STD_ERROR_HANDLE, consoleHandle.DangerousGetHandle());
        // ───────────────────────────────────────────────────────────────────

        // 4) Привязываем StreamWriter к консоли
        var fs = new FileStream(consoleHandle, FileAccess.Write);
        var writer = new StreamWriter(fs, Console.OutputEncoding) { AutoFlush = true };
        Console.SetOut(writer);
        Console.SetError(writer);

        // 5) Пробный вывод
        Console.Title = "Debug Console";
        Console.WriteLine("=== Консоль разработчика ===");

        // 6) Запускаем WinForms
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }
}
