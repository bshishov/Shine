using System.Linq;
using System.Text;

namespace Shine.Http
{
    /// <summary>
    /// https://tools.ietf.org/html/rfc5234#appendix-B.1
    /// </summary>
    public static class HttpNotation
    {
        public static Encoding CommonEncoding = Encoding.GetEncoding("us-ascii");
        public static char CR = (char)0x0D; // carriage return
        public static char LF = (char)0x0A; // linefeed
        public static string CRLF = new string(CR, LF); // Internet standard newline
        public static char HTAB = (char)0x09; // Horizontal tab
        public static char SP = (char)0x20; // Space
        public static char DQUOTE = (char)0x22; // Double Quote "

        public static char[] DELIMETERS =
        {
            '(', ')', ',', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '{',
            '}'
        };

        //  %x21-7E; visible(printing) characters

        public static bool IsVCHAR(char ch) =>
            ch >= 0x21 && ch <= 0x7e;

        // %x41-5A / %x61-7A   ; A-Z / a-z

        public static bool IsALPHA(char ch) =>
            (ch >= 0x41 && ch <= 0x5a) || (ch >= 0x61 && ch <= 0x7a);

        // %x00-1F / %x7F controls

        public static bool IsCTL(char ch) =>
            ch <= 0x1f || ch == 0x7f;

        // %x30-39; 0-9

        public static bool IsDIGIT(char ch) =>
            ch >= 0x30 && ch <= 0x39;

        // DIGIT / "A" / "B" / "C" / "D" / "E" / "F"

        public static bool IsHEXDIG(char ch) => IsDIGIT(ch) || ch == 'A' || ch == 'B' || ch == 'C' || ch == 'D' || ch == 'E' || ch == 'F';

        // SP / HTAB; white space

        public static bool IsWSP(char ch) =>
            ch == SP || ch == HTAB;

        // %x00-FF; 8 bits of data

        public static bool IsOCTET(char ch) =>
            ch >= 0x00 && ch <= 0xFF;

        public static bool IsDELIMETER(char ch) => DELIMETERS.Contains(ch);

        // any vchar, except delimeters

        public static bool IsTCHAR(char ch) => IsVCHAR(ch) && !IsDELIMETER(ch) && ch != DQUOTE;
    }
}