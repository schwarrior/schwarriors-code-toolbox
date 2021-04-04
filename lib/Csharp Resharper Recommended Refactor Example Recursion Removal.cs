		/// <summary>
        /// Recursively examines an exception and all inner exceptions to see if the message contains
        /// the given text. If so, returns the found (inner) exception
        /// </summary>
        public static Exception FindException(Exception outerException, string messageContains)
        {
            if (outerException.Message.Contains(messageContains)) return outerException;
            if (outerException.InnerException != null)
            {
                return FindException(outerException.InnerException, messageContains);
            }
            return null;
        }
		
		
		/// resharper refactored version -- no recursion
        public static Exception FindException(Exception outerException, string messageContains)
        {
            while (true)
            {
                if (outerException.Message.ToLower().Contains(messageContains.ToLower())) return outerException;
                if (outerException.InnerException != null)
                {
                    outerException = outerException.InnerException;
                    continue;
                }
                return null;
                break;
            }
        }