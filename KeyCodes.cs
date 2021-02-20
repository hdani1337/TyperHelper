using System;

namespace TyperHelper
{
    public class KeyCodes
    {
        public const int VK_UP = 0x26; //up key
        public const int VK_DOWN = 0x28;  //down key
        public const int VK_LEFT = 0x25;
        public const int VK_RIGHT = 0x27;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int VK_CONTROL = 0x11;
        public const int VK_RETURN = 0x0D;
        public const int KEYEVENTF_KEYUP = 0x2;
        public const int KEYEVENTF_KEYDOWN = 0x0;
        public const int VK_SHIFT = 0x10;
        public const int VK_CAPITAL = 0x14;
        public const int VK_SPACE = 0x20;
        public static Int32 WM_KEYDOWN = 0x100;
        public static Int32 WM_KEYUP = 0x101;

        public static byte getHungarianKeys(char c)
        {
            switch (c.ToString().ToLower())
            {
                case "é":
                {
                    return 0xBA;
                }
                case "ó":
                {
                    return 0xBB;
                }
                case "ü":
                {
                    return 0xBF;
                }
                case "ö":
                {
                    return 0xC0;
                }
                case "ő":
                {
                    return 0xDB;
                }
                case "ű":
                {
                    return 0xDC;
                }
                case "ú":
                {
                    return 0xDD;
                }
                case "á":
                {
                    return 0xDE;
                }
                
                case " ":
                {
                    return VK_SPACE;
                }
                
                case ".":
                {
                    return 0XBE;
                }
                
                case "-":
                {
                    return 0XBD;
                }

                case ",":
                {
                    return 0XBC;
                }

                default:
                {
                    ConsoleKey ck;
                    Enum.TryParse<ConsoleKey>(c.ToString().ToUpper(), out ck);
                    return (byte)ck;
                }
            }
        }
    }
}