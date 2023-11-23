using System;
using System.Collections.Generic;

namespace DocumentRecognizerTest.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public Exception Exception;
        private string _PreviousPage;
        public string PreviousPage { get { return Exception?.Data?["page"]?.ToString() ?? _PreviousPage ?? null; } set { _PreviousPage = value; } }
        //public dynamic ErrorInformation { set { Exception = value?.Error; PreviousPage = value?.Path; } }

        public int ErrorCode = 0;
        public bool IsErrorCode => ErrorCode != 0;
        private Dictionary<int, string> CommonCodes = new Dictionary<int, string>()
        {
            { 401, "You are not correctly authenticated. Please refresh the page and try again." },//Unauthorized
            { 403, "You are not authorized to view this page." },//Forbidden
            { 404, "The page requested was not found." }//Not Found
        };

        private string _ErrorMessage;
        public string ErrorMessage
        {
            get
            {
                if (IsErrorCode)
                {
                    if (CommonCodes.ContainsKey(ErrorCode))
                        return CommonCodes[ErrorCode];
                    else
                        return "Error code: " + ErrorCode;
                }
                else
                {
                    return _ErrorMessage == null || _ErrorMessage == "" ? Exception != null ? Exception.Message : "Unhandled Error" : _ErrorMessage;
                }
            }
            set
            {
                _ErrorMessage = value;
            }
        }
    }
}