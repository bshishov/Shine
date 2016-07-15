using System.Linq;

namespace Shine.Http
{
    public static class HttpValidationUtilities
    {
        // https://tools.ietf.org/html/rfc7230#section-3.2
        public static bool IsValidHeaderFieldValue(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue))
                return false;
            
            if(!HttpNotation.IsVCHAR(fieldValue[0]))
                return false;

            for (var i = 1; i < fieldValue.Length; i++)
            {
                if (!(HttpNotation.IsVCHAR(fieldValue[i]) || HttpNotation.IsWSP(fieldValue[i])))
                    return false;
            }

            return true;
        }

        // https://tools.ietf.org/html/rfc7230#section-3.2.6
        public static bool IsValidToken(string token)
        {
            if(string.IsNullOrEmpty(token))
                return false;

            if (token.Any(t => !HttpNotation.IsTCHAR(t)))
                return false;

            return true;
        }
    }
}