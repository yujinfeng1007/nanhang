using System.Text;

namespace ZHXY.Common.IsNumeric
{
    public class IsNumeric
    {
        public static bool isNumeric(string str)
        {
            if (str == null || str.Length == 0)    
                return false;                          
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] bytestr = ascii.GetBytes(str);        

            foreach (byte c in bytestr)                 
            {
                if (c < 48 || c > 57)                       
                {
                    return false;                             
                }
            }
            return true;                                       
        }
    }
}
