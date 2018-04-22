namespace Utils
{
    public class ConversionProvider
    {
        public string ConvertIdToShortenString(int input)
        {
            input = MakeConguentConversion(input);
            
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";

            string result = "";

            while (input > 0)
            {
                result += chars[input % 36];
                input /= 36;
            }

            return result;
        }
        
        private int MakeConguentConversion(int input)
        {
            // coefficients for linear congruent conversion
            const int coefficientA = 16807;

            const int coefficientM = int.MaxValue;

            const int coefficientC = 0;
            
            return (input * coefficientA + coefficientC) % coefficientM;
        }
    }
}