namespace Sleep0.Tools
{
    public static class CodeTools
    {
        public static bool IsOverloadingToString<T>(T checkObject)
        {
            return checkObject.GetType().ToString() != checkObject.ToString();
        }
    }
}