namespace hrms_be_backend_business.AppCode
{
    public class StringEvaluatorValue
    {
        public long UpperCaseTotal { get; set; }
        public long LowerCaseTotal { get; set; }
        public long NumberTotal { get; set; }
        public long SpecialCharacterNumber { get; set; }
    }
    public class StringEvaluator
    {
        public static StringEvaluatorValue EvaluateString(string str)
        {
            int upper = 0, lower = 0;
            int number = 0, special = 0;

            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch >= 'A' && ch <= 'Z')
                    upper++;
                else if (ch >= 'a' && ch <= 'z')
                    lower++;
                else if (ch >= '0' && ch <= '9')
                    number++;
                else
                    special++;
            }
            return new StringEvaluatorValue { UpperCaseTotal = upper, LowerCaseTotal = lower, NumberTotal = number,SpecialCharacterNumber=special
            };
        }
    }
}
