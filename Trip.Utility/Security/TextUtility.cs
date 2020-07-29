
#region Using Directives

using System;
using System.Text.RegularExpressions;

#endregion

namespace Trip.Utility.Security
{
    /// <summary>
    /// Common string formatting and conversion utility functions
    /// </summary>
    public class TextUtility
    {
        // No instance of this class should be created.
        private TextUtility()
        {
        }
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        #region GetCurrentDateTimeAsString
        public static string GetCurrentDateTimeAsString()
        {
            return DateTime.Now.ToString(DateTimeFormat);
        }
        public static DateTime GetCurrentDateTimeFromString(string dateTimeFormattedString)
        {
            System.Globalization.CultureInfo provider = new System.Globalization.CultureInfo("en-US");
            return DateTime.ParseExact(dateTimeFormattedString, DateTimeFormat, provider);
        }
        #endregion

        #region PruneText
        /// <summary>
        /// Trim the inputString.  Return null if the inputString is null or empty after trimming.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        static public string PruneText(string inputString)
        {
            string outputString = null;

            if (inputString != null && inputString.Trim().Length != 0)
                outputString = inputString.Trim();

            return outputString;
        }

        /// <summary>
        /// Trim the inputString, removing maskChar(s).  Return null if the inputString is null or the trimmed string is empty
        /// </summary>
        /// <param name="inputString">the string to be unmasked and trimmed</param>
        /// <param name="maskChar">the mask character to be removed</param>
        /// <returns>null or a trimmed string that does not contain the maskChar</returns>
        static public string PruneText(string inputString, char maskChar)
        {
            if (inputString == null) return null;

            var sb = new System.Text.StringBuilder();
            foreach (var c in inputString.ToCharArray())
                if (c != maskChar)
                    sb.Append(c);

            if (sb.Length == 0 || sb.ToString().Trim().Length == 0) return null;
            return sb.ToString().Trim();
        }
        #endregion

        #region NumbersOnly
        /// <summary>
        /// Returns a string containing only the digits found in the input string, or null if no digits.
        /// </summary>
        /// <param name="inputString">The input string to parse.</param>
        /// <returns></returns>
        static public string NumbersOnly(string inputString)
        {
            if (inputString == null) return null;
            var sb = new System.Text.StringBuilder();
            foreach (var c in inputString.ToCharArray())
                if (char.IsDigit(c))
                    sb.Append(c);
            if (sb.Length == 0) return null;
            return sb.ToString();
        }
        #endregion

        #region FormatZip
        /// <summary>
        /// Attempts to format numbers in string as a zip code - will not cause error and 
        /// will still process invalid zips.
        /// </summary>
        /// <param name="ZipCode"></param>
        /// <returns>Null if no digits, or attempted formatting of the digits.</returns>    
        static public string FormatZip(string ZipCode)
        {
            string zipCodeNumbers = TextUtility.NumbersOnly(ZipCode);
            if (zipCodeNumbers == null)
                return null;
            else if (zipCodeNumbers.Length == 5)
                return zipCodeNumbers;
            else if (zipCodeNumbers.Length > 5) // May be invalid but don't care.
                return zipCodeNumbers.Substring(0, 5) + "-" + zipCodeNumbers.Substring(5);
            else// Invalid zip code with less than 5 digits.
                return zipCodeNumbers;
        }
        #endregion

        #region FormatPhone
        /// <summary>
        /// Takes in the provided phone number string and attempts to parse it and apply a standard format.
        /// Standard Format: (area) prefix-suffix [Ext.: therest]
        /// </summary>
        /// <param name="PhoneNumber">The string to parse as  phone number.</param>
        /// <returns>A formatted phone number or the origional string if it has numbers that can't be parsed as a phone number.</returns>
        static public string FormatPhone(string PhoneNumber)
        {
            if (String.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Trim().Length == 0)
                return null;
            // No numbers - can't parse anything meaningful.
            if (NumbersOnly(PhoneNumber) == null)
                return PhoneNumber;

            string fixedPhone;
            foreach (Match m in GetPhoneRegex().Matches(PhoneNumber))
            {
                fixedPhone = "(" + m.Groups["area"].Value + ") " + m.Groups["prefix"].Value + "-" + m.Groups["suffix"].Value;
                if (m.Groups["therest"].Value.Length > 0)
                    fixedPhone += " Ext.:" + m.Groups["therest"].Value.Trim();
                return fixedPhone;
            }
            // Can't figure it out.
            return PhoneNumber;
        }
        /// <summary>
        /// Given the parts of a phone number, return a formatted string.  Each part is trimmed.  
        /// For the areaCode and the localNumber, only digits are used.
        /// Standard Format: [(area) ]prefix-suffix [Ext.: extension]
        /// </summary>
        /// <param name="areaCode">a possibly untrimmed string containing the digits of the area code</param>
        /// <param name="localNumber">a possibly untrimmed string containing the digits of the local number</param>
        /// <param name="extension">a possibly empty or untrimmed extension</param>
        /// <returns>a formatted phone number string</returns>
        static public string FormatPhone(string areaCode, string localNumber, string extension)
        {
            string returnValue = null;

            areaCode = NumbersOnly(PruneText(areaCode));
            localNumber = NumbersOnly(PruneText(localNumber));
            extension = NumbersOnly(PruneText(extension));

            if (!string.IsNullOrEmpty(areaCode))
                returnValue = string.Format("({0})", areaCode);

            if (!string.IsNullOrEmpty(localNumber))
            {
                if (!string.IsNullOrEmpty(returnValue)) returnValue += " ";
                returnValue += string.Format("{0}-{1}", localNumber.Substring(0, 3), localNumber.Substring(3));
            }
            if (!string.IsNullOrEmpty(extension))
            {
                if (!string.IsNullOrEmpty(returnValue)) returnValue += " ";
                returnValue += string.Format("Ext.:{0}", extension);
            }

            return returnValue;
        }
        #endregion

        #region GetPhoneRegex
        /// <summary>
        /// Get a regular expression for parsing a phone number into its parts: area, prefix, suffix, therest
        /// </summary>
        /// <returns></returns>
        public static Regex GetPhoneRegex()
        {
            return new Regex("(?<!\\d+)1?\\D?(?<area>\\d{3})\\D*(?<prefix>\\d{3})\\D?(?<suffix>\\d{4})(?!\\d+)(?<therest>(\\D+\\d+)?)", RegexOptions.Singleline);
        }
        #endregion

        #region GetEmailRegex
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Regex GetEmailRegex()
        {
            //           return new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            return new Regex("^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
            //            return new Regex("^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        #endregion

        #region IsValidEmail
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string emailAddress)
        {
            bool rtnVal = false;

            Regex regex = GetEmailRegex();
            if (regex != null)
                rtnVal = regex.IsMatch(emailAddress);

            return rtnVal;
        }
        #endregion

        #region DisplayYesNo
        /// <summary>
        /// Given a boolean, convert it to "Yes" or "No"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public string DisplayYesNo(Boolean value)
        {
            if (value)
                return "Yes";
            else
                return "No";
        }
        #endregion

        #region TextToNullableXXX functions --- long? int? double?
        /// <summary>
        /// Uses Int64.TryParse to parse the inputString, returning a nullable long
        /// </summary>
        /// <param name="inputString">apparently a string representing a long value</param>
        /// <returns>the long value or null</returns>
        static public long? TextToNullableLong(string inputString)
        {
            long? rtnVal = null;

            if (!String.IsNullOrEmpty(inputString))
            {
                long outValue = 0;
                if (Int64.TryParse(inputString, out outValue))
                    rtnVal = outValue;
            }

            return rtnVal;

        }

        /// <summary>
        /// Uses Int32.TryParse to convert the inputString, returning a nullable int.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        static public int? TextToNullableInt(string inputString)
        {
            int? rtnVal = null;

            if (!String.IsNullOrEmpty(inputString))
            {
                int outValue = 0;
                if (Int32.TryParse(inputString, out outValue))
                    rtnVal = outValue;
            }

            return rtnVal;
        }

        /// <summary>
        /// Uses Double.TryParse to convert the inputString, returning a nullable double.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        static public double? TextToNullableDouble(string inputString)
        {
            double? rtnVal = null;

            if (!String.IsNullOrEmpty(inputString))
            {
                double outValue = 0;
                if (Double.TryParse(inputString, out outValue))
                    rtnVal = outValue;
            }

            return rtnVal;
        }
        /// <summary>
        /// Uses float.TryParse to convert the inputString, returning a nullable float.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        static public float? TextToNullableFloat(string inputString)
        {
            float? rtnVal = null;

            if (string.IsNullOrEmpty(inputString))
            {
                float outValue = 0;
                if (float.TryParse(inputString, out outValue))
                    rtnVal = outValue;
            }
            return rtnVal;
        }

        #endregion

        #region TextToBool
        /// <summary>
        /// Return true if the inputString represents a positive integer, or the words "true" or "yes" (case-insensitive). 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        static public bool TextToBool(string inputString)
        {
            bool rtnVal = false;

            if (!String.IsNullOrEmpty(inputString))
            {
                int outValue = 0;
                if (Int32.TryParse(inputString, out outValue))
                {
                    if (outValue > 0)
                        rtnVal = true;
                }
                else if (inputString.ToLower() == "true" || inputString.ToLower() == "yes")
                    rtnVal = true;
            }

            return rtnVal;
        }
        #endregion

        #region EncodeJsString
        /// <summary>
        /// Encodes a string to be represented as a string literal for javascript - including the outer double-quotes.
        /// </summary>
        /// <param name="s">The string to javascript encode.</param>
        /// <returns></returns>
        /// <remarks>The string returned includes outer quotes.</remarks>
        /// <seealso cref="https://www.west-wind.com/Weblog/posts/114530.aspx"/>
        public static string EncodeJsString(string s)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }
        #endregion

        #region GetFileSizeString
        /// <summary>
        /// Return a formatted string representing the file size in B, KB, MB, or GB.
        /// Except for B, the size is rounded to one decimal place
        /// </summary>
        /// <param name="SizeInBytes"></param>
        /// <returns>formatted size string</returns>
        public static string GetFileSizeString(long SizeInBytes)
        {
            const int GigaByte = 1073741824;
            const int MegaByte = 1048573;
            const int KiloByte = 1024;
            if (SizeInBytes >= GigaByte)
                return String.Format("{0:0.0} GB", (double)SizeInBytes / (double)GigaByte);
            if (SizeInBytes >= MegaByte)
                return String.Format("{0:0.0} MB", (double)SizeInBytes / (double)MegaByte);
            if (SizeInBytes >= KiloByte)
                return String.Format("{0:0.0} KB", (double)SizeInBytes / (double)KiloByte);
            return String.Format("{0:0} B", SizeInBytes);
        }
        #endregion

        #region EncodingType enum
        /// <summary>
        /// Encoding Types.
        /// </summary>
        public enum EncodingType
        {
            ASCII,
            Unicode,
            UTF7,
            UTF8
        }
        #endregion

        #region StringToByteArray
        /// <summary>
        /// Converts a string to a byte array using specified encoding.
        /// </summary>
        /// <param name="str">String to be converted.</param>
        /// <param name="encodingType">EncodingType enum.</param>
        /// <returns>byte array</returns>
        public static byte[] StringToByteArray(string str, EncodingType encodingType)
        {
            System.Text.Encoding encoding = null;
            switch (encodingType)
            {
                case EncodingType.ASCII:
                    encoding = new System.Text.ASCIIEncoding();
                    break;
                case EncodingType.Unicode:
                    encoding = new System.Text.UnicodeEncoding();
                    break;
                case EncodingType.UTF7:
                    encoding = new System.Text.UTF7Encoding();
                    break;
                case EncodingType.UTF8:
                    encoding = new System.Text.UTF8Encoding();
                    break;
            }
            return encoding.GetBytes(str);
        }
        #endregion

        #region ByteArrayToString
        /// <summary>
        /// Converts a byte array to a string using Unicode encoding.
        /// </summary>
        /// <param name="bytes">Array of bytes to be converted.</param>
        /// <returns>string</returns>
        public static string ByteArrayToString(byte[] bytes)
        {
            return ByteArrayToString(bytes, EncodingType.Unicode);
        }
        /// <summary>
        /// Converts a byte array to a string using specified encoding.
        /// </summary>
        /// <param name="bytes">Array of bytes to be converted.</param>
        /// <param name="encodingType">EncodingType enum.</param>
        /// <returns>string</returns>
        public static string ByteArrayToString(byte[] bytes, EncodingType encodingType)
        {
            System.Text.Encoding encoding = null;
            switch (encodingType)
            {
                case EncodingType.ASCII:
                    encoding = new System.Text.ASCIIEncoding();
                    break;
                case EncodingType.Unicode:
                    encoding = new System.Text.UnicodeEncoding();
                    break;
                case EncodingType.UTF7:
                    encoding = new System.Text.UTF7Encoding();
                    break;
                case EncodingType.UTF8:
                    encoding = new System.Text.UTF8Encoding();
                    break;
            }
            return encoding.GetString(bytes);
        }
        #endregion

        #region TextToNullableDate
        /// <summary>
        /// Using DateTime.TryParse, return a nullable DateTime.
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public static DateTime? TextToNullableDate(string strDate)
        {
            DateTime? rtnVal = null;
            DateTime date;

            if (DateTime.TryParse(strDate, out date))
                rtnVal = date;

            return rtnVal;
        }
        #endregion

        #region GetSelectedId

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static int? GetSelectedId(string selectedValue)
        {
            int? rtnVal = TextUtility.TextToNullableInt(selectedValue);

            // If it is the empty row added from the collection, the return value should be null
            if (rtnVal == 0)
                rtnVal = null;

            return rtnVal;
        }

        #endregion

        #region GetSelectedValue

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static string GetSelectedValue(string selectedValue)
        {
            string rtnVal = TextUtility.PruneText(selectedValue);

            // If it is the empty row added from the collection, the return value should be null
            if (String.IsNullOrEmpty(rtnVal))
                rtnVal = null;

            return rtnVal;
        }

        #endregion

        #region ObjectToString
        public static string ObjectToString(object objectToStringify)
        {
            if (objectToStringify == null)
                return string.Empty;
            else
                return objectToStringify.ToString();
        }
        #endregion


        #region TextTimeToNullableDate
        public static DateTime? TextTimeToNullableDate(string strTime, bool roundUp=false)
        {
            DateTime? rtnVal = null;
            strTime = TextUtility.PruneText(strTime);

            if (!String.IsNullOrEmpty(strTime))
            {
                string time = null;
                string strNumber = null;
                string strText = null;

                TimeParser(strTime, out strNumber, out strText);

                // Prepend hours if missing a digit
                if (!String.IsNullOrEmpty(strNumber) && (strNumber.Length == 1 || strNumber.Length == 3))
                    strNumber = "0" + strNumber;

                // Append the minutes if missing
                if (!String.IsNullOrEmpty(strNumber) && strNumber.Length < 4)
                    strNumber = strNumber + "00";

                if (!String.IsNullOrEmpty(strNumber))
                {
                    time = strNumber.Substring(0, 2) + ":" + strNumber.Substring(2, 2);
                    int hour = Int32.Parse(strNumber.Substring(0, 2));
                    
                    // Implys 24 format
                    if (hour > 12)
                    {
                        time = String.Format("{0:##}:{1}pm", hour - 12, strNumber.Substring(2, 2));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(strText))
                        {
                            if (strText[0] == 'p' || strText[0] == 'P')
                            {
                                time += "pm";
                            }
                            else
                            {
                                time += "am";
                            }
                        }
                        else
                        {
                            time += "am";
                        }
                    }
                }            

                DateTime date;

                if (DateTime.TryParse(time, out date))
                {
                    if (roundUp)
                    {
                        rtnVal = RoundTime(date);
                    }
                    else
                    {
                        rtnVal = date;
                    }
                }
            }

            return rtnVal;
        }

        static void TimeParser(string strTime, out string strNumber, out string strText)
        {
            strNumber = null;
            strText = null;
            bool lookingForNumber = true;

            foreach (char c in strTime)
            {
                if (Char.IsNumber(c))
                {
                    if (!lookingForNumber)
                        break;

                    if (strNumber == null)
                    {
                        strNumber = c.ToString();
                    }
                    else
                    {
                        strNumber += c.ToString();
                    }
                }
                else
                {
                    if (lookingForNumber)
                        lookingForNumber = false;

                    if (strText == null)
                    {
                        strText = c.ToString();
                    }
                    else
                    {
                        strText += c.ToString();
                    }
                }
            }
        }

        static DateTime RoundTime(DateTime time)
        {
            DateTime rtnVal = time;

            int minute = time.Minute % 15;

            if (minute > 0)
                rtnVal = time.AddMinutes(15 - minute);

            return rtnVal;
        }
        #endregion

        #region FormatShortTimeField
        public static string FormatShortTimeField(DateTime? date)
        {
            string rtnVal = date.HasValue ? date.Value.ToString("hhmmt").ToLower() : null;

            return rtnVal;
        }
        #endregion

        #region RoundQuantity
        public static Double? RoundQuantity(Double? quantity)
        {
            Double? rtnVal = quantity;

            if (quantity.HasValue)
            {
                double fraction = quantity.Value - Math.Floor(quantity.Value);

                if (fraction > 0 && fraction <= .25)
                {
                    rtnVal = Math.Floor(quantity.Value) + .25;
                }
                else if (fraction > .25 && fraction <= .5)
                {
                    rtnVal = Math.Floor(quantity.Value) + .5;
                }
                else if (fraction > .5 && fraction <= .75)
                {
                    rtnVal = Math.Floor(quantity.Value) + .75;
                }
                else if (fraction > .75)
                {
                    rtnVal = Math.Floor(quantity.Value) + 1.0;
                }
            }

            return rtnVal;
        }
        #endregion
    }
}

